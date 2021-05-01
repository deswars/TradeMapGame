using System;
using System.Collections.Generic;
using System.Linq;

namespace TradeMapGame
{
    public class Engine
    {
        public Map Map { get; }
        public List<Settlement> Settlements { get; }
        public int Turn { get; private set; }
        public TurnLog Log { get; }

        public Engine(TradeMapGame.Map map, Configuration configuration)
        {
            Map = map;
            _conf = configuration;
            Settlements = new();

            rnd = new Random();
            Turn = 0;
            Log = new TurnLog();
        }

        public void NextTurn()
        {
            Log.SetTurn(Turn);
            foreach (var settlement in Settlements)
            {
                DecayResources(settlement);
                if (Turn % _conf.ExpansionDelay == 0)
                {
                    ExpandSettlement(settlement);
                }
                GrowPopulation(settlement);
            }
            TradeGoods();
            foreach (var settlement in Settlements)
            {
                GatherResources(settlement);
                UpdatePrices(settlement);
            }
            Turn++;
        }


        private readonly Configuration _conf;
        private readonly Random rnd;

        private void DecayResources(Settlement settlement)
        {
            Log.StartEntry("Settlement{" + settlement.Position.X + ";" + settlement.Position.Y + "} resources decayed:");

            foreach (var resource in _conf.ResourceTypes)
            {
                Log.ContinueEntry(resource.Value.Id + "=" + (settlement.Resources[resource.Value] * resource.Value.DecayRate) + ";");
                settlement.Resources[resource.Value] *= 1 - resource.Value.DecayRate;
            }
            Log.FinishEntry("");
        }

        private void ExpandSettlement(Settlement settlement)
        {
            if (settlement.GetFreePopulation() > 0)
            {
                Log.StartEntry("Settlement {" + settlement.Position.X + ";" + settlement.Position.Y + "}:");
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
            var possibleLocations = Map.GetNeigborCells(position.X, position.Y).Where(c => c.BuiltCollector == null);
            if (!possibleLocations.Any())
            {
                Log.FinishEntry("every neighbour cell occupied");
                return;
            }
            int locationIndex = rnd.Next(possibleLocations.Count());
            var location = possibleLocations.ElementAt(locationIndex);

            var possibleCollectors = _conf.CollectorTypes
                .Select(c => c.Value)
                .Where(
                    c => c.RequiredTerrain.Any(t => t == location.Terrain)
                    && (!c.RequiredFeautre.Any() || c.RequiredFeautre.Any(f => location.MapFeautres.Contains(f))));
            if (!possibleCollectors.Any())
            {
                Log.FinishEntry("there in no any suitable collector for selected cell");
                return;
            }
            var possibleCollectorsNumber = possibleCollectors.Count();
            var collectorIndex = rnd.Next(possibleCollectorsNumber);
            var collectorType = possibleCollectors.ElementAt(collectorIndex);

            var collector = new Collector(location, collectorType, settlement);
            location.BuiltCollector = collector;
            settlement.Collectors.Add(collector);
            
            Log.FinishEntry("built " + collectorType.Id);
        }

        private void BuildBuilding(Settlement settlement)
        {
            var consumption = settlement.GetResourceConsumption();
            var excessResources = settlement.Resources.Where(res => res.Value - consumption[res.Key] > 0).Select(res => res.Key);
            var possibleBuildings = _conf.BuildingTypes.Where(b => b.Value.Input.All(res => excessResources.Contains(res.Key)));
            if (!possibleBuildings.Any())
            {
                Log.FinishEntry("not enought stored resources for new building");
                return;
            }
            var buildingIndex = rnd.Next(possibleBuildings.Count());
            var buildingType = possibleBuildings.ElementAt(buildingIndex);

            var building = new Building(settlement, buildingType.Value);
            settlement.Buildings.Add(building);

            Log.FinishEntry("built " + buildingType.Value.Id);
        }

        private void GatherResources(Settlement settlement)
        {
            Log.Write("Settlement {" + settlement.Position.X + ";" + settlement.Position.Y + "} gathered resources");
            settlement.GatherResource();
            settlement.Resources[_conf.MoneyResource] += _conf.TaxPerPop * settlement.Population;
        }

        private void GrowPopulation(Settlement settlement)
        {
            if (_conf.GrowthDemand.All(res => res.Value * settlement.Population < settlement.Resources[res.Key]))
            {
                settlement.Population++;
                foreach (var resource in _conf.GrowthDemand)
                {
                    settlement.Resources[resource.Key] -= resource.Value;
                }
            }
        }

        private void UpdatePrices(Settlement settlement)
        {
            Log.StartEntry("Settlement {" + settlement.Position.X + ";" + settlement.Position.Y + "} updated prices { ");
            var consumption = settlement.GetResourceConsumption();
            foreach (var resource in settlement.Resources)
            {
                if (resource.Key != _conf.MoneyResource)
                {
                    var resourceDesiredBalance = resource.Value - consumption[resource.Key] * _conf.DesiredReserveTurns;
                    double resourceRelativeBalance;
                    if (consumption[resource.Key] != 0)
                    {
                        resourceRelativeBalance = Math.Abs(resourceDesiredBalance / consumption[resource.Key] / _conf.DesiredReserveTurns);
                    }
                    else
                    {
                        resourceRelativeBalance = _conf.UndesiredEffectiveExcess;
                    }

                    if (resourceDesiredBalance > 0)
                    {
                        Log.ContinueEntry(resource.Key.Id + " from " + settlement.Prices[resource.Key]);
                        var priceDivide = resourceRelativeBalance * _conf.ExcessPriceDivider;
                        if (priceDivide > _conf.MaxExcessPriceDivider)
                        {
                            priceDivide = _conf.MaxExcessPriceDivider;
                        }
                        settlement.Prices[resource.Key] /= 1 + priceDivide;
                        if (settlement.Prices[resource.Key] < _conf.MinPrice)
                        {
                            settlement.Prices[resource.Key] = _conf.MinPrice;
                        }
                        if (settlement.Prices[resource.Key] > _conf.MaxPrice)
                        {
                            settlement.Prices[resource.Key] = _conf.MaxPrice;
                        }
                        Log.ContinueEntry(" to " + settlement.Prices[resource.Key] + "; ");
                    }
                    else
                    {
                        Log.ContinueEntry(resource.Key.Id + " from " + settlement.Prices[resource.Key]);
                        settlement.Prices[resource.Key] *= 1 + resourceRelativeBalance * _conf.StarvePriceMultiply;
                        if (settlement.Prices[resource.Key] < _conf.MinPrice)
                        {
                            settlement.Prices[resource.Key] = _conf.MinPrice;
                        }
                        if (settlement.Prices[resource.Key] > _conf.MaxPrice)
                        {
                            settlement.Prices[resource.Key] = _conf.MaxPrice;
                        }
                        Log.ContinueEntry(" to " + settlement.Prices[resource.Key] + "; ");
                    }
                }
            }
            Log.FinishEntry("}");
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
            var moneyResource = _conf.MoneyResource;
            if (destination.Resources[moneyResource] == 0)
            {
                return;
            }
            foreach (var resource in source.Resources)
            {
                if (resource.Key != moneyResource)
                {
                    double tradeAmount = resource.Value - consumption[resource.Key] * _conf.DesiredReserveTurns;
                    if ((tradeAmount > 0) && (source.Prices[resource.Key] < destination.Prices[resource.Key]))
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
                        Log.Write("Trade from {" + source.Position.X + ";" + source.Position.Y + "} to {" + destination.Position.X + ";" + destination.Position.Y + "} " + resource.Key.Id + " price:" + destination.Prices[resource.Key] + " amount:" + tradeAmount);
                    }
                }
            }
        }
    }
}
