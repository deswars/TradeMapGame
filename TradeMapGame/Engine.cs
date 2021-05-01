using System;
using System.Collections.Generic;
using System.Linq;

namespace TradeMapGame
{
    public class Engine
    {
        public Map Map { get; }
        public List<Settlement> Settlements { get; }
        public int Step { get; private set; }

        public Engine(TradeMapGame.Map map, Configuration configuration)
        {
            Map = map;
            _conf = configuration;
            Settlements = new();

            rnd = new Random();
            Step = 0;
        }

        public void NextTick()
        {
            Step++;
            foreach (var settlement in Settlements)
            {
                DecayResources(settlement);
                if (Step % _conf.ExpansionDelay == 0)
                {
                    ExpandAllSettlements(settlement);
                }
                GatherAll(settlement);
                UpdatePrices(settlement);
            }
            TradeGoods();
        }


        private readonly Configuration _conf;
        private readonly Random rnd;

        private void DecayResources(Settlement settlement)
        {
            foreach (var resource in _conf.ResourceTypes)
            {
                settlement.Resources[resource.Value] *= 1 - resource.Value.DecayRate;
            }
        }

        private void ExpandAllSettlements(Settlement settlement)
        {
            if (settlement.GetFreePopulation() > 0)
            {
                if (rnd.NextDouble() < _conf.ChanceToCreateCollector)
                {
                    BuildCollector(settlement);
                }
                else
                {
                    BuildBuilding(settlement);
                }
            }
        }

        private void BuildCollector(Settlement settlement)
        {
            var position = settlement.Position;
            var possibleLocations = Map.GetNeigborCells(position.X, position.Y).Where(c => c.BuiltCollector != null);
            if (!possibleLocations.Any())
            {
                return;
            }
            int locationIndex = rnd.Next(possibleLocations.Count());
            var location = possibleLocations.ElementAt(locationIndex);

            var possibleCollectors = _conf.CollectorTypes
                .Select(c => c.Value)
                .Where(
                    c => c.RequiredTerrain.Any(t => t == location.Terrain)
                    && (!location.MapFeautres.Any() || location.MapFeautres.Any(f => location.MapFeautres.Contains(f))));
            if (!possibleCollectors.Any())
            {
                return;
            }
            var collectorIndex = rnd.Next(possibleCollectors.Count());
            var collectorType = possibleCollectors.ElementAt(collectorIndex);

            var collector = new Collector(location, collectorType, settlement);
            location.BuiltCollector = collector;
            settlement.Collectors.Add(collector);
        }

        private void BuildBuilding(Settlement settlement)
        {
            var consumption = settlement.GetResourceConsumption();
            var excessResources = settlement.Resources.Where(res => res.Value - consumption[res.Key] > 0).Select(res => res.Key);
            var possibleBuildings = _conf.BuildingTypes.Where(b => b.Value.Input.All(res => excessResources.Contains(res.Key)));
            if (!possibleBuildings.Any())
            {
                return;
            }
            var buildingIndex = rnd.Next(possibleBuildings.Count());
            var buildingType = possibleBuildings.ElementAt(buildingIndex);

            var building = new Building(settlement, buildingType.Value);
            settlement.Buildings.Add(building);
        }

        private void GatherAll(Settlement settlement)
        {
            settlement.GatherResource();
        }

        private void UpdatePrices(Settlement settlement)
        {
            var consumption = settlement.GetResourceConsumption();
            foreach (var resource in settlement.Resources)
            {
                var resourceDesiredBalance = resource.Value - consumption[resource.Key] * _conf.DesiredReserveTurns;
                double resourceRelativeBalance;
                if (consumption[resource.Key] != 0)
                {
                    resourceRelativeBalance = resourceDesiredBalance / consumption[resource.Key];
                }
                else
                {
                    resourceRelativeBalance = _conf.UndesiredEffectiveExcess;
                }

                if (resourceDesiredBalance > 0)
                {
                    var priceDivide = resourceRelativeBalance * _conf.ExcessPriceDivider;
                    if (priceDivide > _conf.MaxExcessPriceDivider)
                    {
                        priceDivide = _conf.MaxExcessPriceDivider;
                    }
                    settlement.Prices[resource.Key] /= priceDivide;
                }
                else
                {
                    settlement.Prices[resource.Key] *= (-resourceRelativeBalance) * _conf.StarvePriceMultiply;
                }
            }
        }

        private void TradeGoods()
        {
            foreach (var settlement in Settlements)
            {
                var consumption = settlement.GetResourceConsumption();
                foreach (var partner in Settlements)
                {
                    if (partner != settlement)
                    {
                        TradeGoodsFrom(settlement, partner, consumption);
                    }
                }
            }
        }

        private void TradeGoodsFrom(Settlement source, Settlement destination, Dictionary<ResourceType, double> consumption)
        {
            var moneyResource = _conf.ResourceTypes[_conf.MoneyResource];
            if (destination.Resources[moneyResource] == 0)
            {
                return;
            }
            foreach (var resource in source.Resources)
            {
                if (resource.Key != moneyResource)
                {
                    double tradeAmount = resource.Value - consumption[resource.Key] * _conf.DesiredReserveTurns;
                    if ((tradeAmount > 0) && (source.Prices[resource.Key] > destination.Prices[resource.Key]))
                    {
                        double tradePrice = tradeAmount * destination.Prices[resource.Key];
                        if (tradePrice > destination.Resources[moneyResource])
                        {
                            tradePrice = destination.Resources[moneyResource];
                            tradeAmount = tradePrice / destination.Prices[resource.Key];
                        }
                        destination.Resources[resource.Key] += tradeAmount;
                        destination.Resources[moneyResource] -= tradePrice;
                        source.Resources[resource.Key] -= tradeAmount;
                        source.Resources[moneyResource] += tradePrice;
                    }
                }
            }
        }
    }
}
