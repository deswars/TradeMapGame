using System.Collections.Generic;
using TradeMapGame.Configuration;
using TradeMapGame.Log;
using TradeMapGame.Map;

namespace TradeMapGame
{
    public class Engine
    {
        public delegate void SettlementAction(int turn, Settlement settlement);
        public delegate void GlobalAction(int turn, IMap map, IEnumerable<Settlement> settlementList);

        public event SettlementAction OnPreTurn;
        public event GlobalAction OnTurn;
        public event SettlementAction OnPostTurn;

        public SquareDiagonalMap Map { get; }
        public List<Settlement> Settlements { get; }
        public int Turn { get; private set; }
        public TurnLog? Log { get; }
        public Constants Consts { get; }
        public TypeRepository Lists { get; }

        public Engine(SquareDiagonalMap map, ConfigurationLoader configuration, TurnLog? log)
        {
            Map = map;
            Settlements = new();
            Consts = configuration.Const;
            Lists = configuration.Lists;

            Turn = 0;
            Log = log;
            OnPreTurn = PreTurnLog;
            OnPostTurn = PostTurnLog;
            OnTurn = TurnLog;
        }

        public void NextTurn()
        {
            foreach (var sett in Settlements)
            {
                OnPreTurn(Turn, sett);
            }
            OnTurn(Turn, Map, Settlements);
            foreach (var sett in Settlements)
            {
                OnPostTurn(Turn, sett);
            }
            Turn++;
        }

        private void PreTurnLog(int turn, Settlement settlement)
        {
            if (Log != null)
            {
                Log.AddEntry(new LogEntryPreTurn(turn, settlement));
            }
        }

        private void PostTurnLog(int turn, Settlement settlement)
        {
            if (Log != null)
            {
                Log.AddEntry(new LogEntryPostTurn(turn, settlement));
            }
        }

        private void TurnLog(int turn, IMap map, IEnumerable<Settlement> settlementList)
        {
            if (Log != null)
            {
                Log.AddEntry(new LogEntryTurn(turn));
            }
        }

        //private void ExpandSettlement(Settlement settlement)
        //{
        //    if (settlement.GetFreePopulation() > 0)
        //    {
        //        //if (rnd.NextDouble() < _conf.ChanceToCreateCollector)
        //        //{
        //        //    BuildCollector(settlement);
        //        //}
        //        //else
        //        //{
        //        //    BuildBuilding(settlement);
        //        //}
        //    }
        //}

        //private void BuildCollector(Settlement settlement)
        //{
        //    var position = settlement.Position;
        //    var possibleLocations = Map.GetNeigborCells(position.X, position.Y).Where(c => c.BuiltCollector == null);
        //    if (!possibleLocations.Any())
        //    {
        //        return;
        //    }
        //    int locationIndex = rnd.Next(possibleLocations.Count());
        //    var location = possibleLocations.ElementAt(locationIndex);

        //    var possibleCollectors = _conf.Lists.CollectorTypes
        //        .Select(c => c.Value)
        //        .Where(
        //            c => c.RequiredTerrain.Any(t => t == location.Terrain)
        //            && (!c.RequiredFeautre.Any() || c.RequiredFeautre.Any(f => location.MapFeautres.Contains(f))));
        //    if (!possibleCollectors.Any())
        //    {
        //        return;
        //    }
        //    var possibleCollectorsNumber = possibleCollectors.Count();
        //    var collectorIndex = rnd.Next(possibleCollectorsNumber);
        //    var collectorType = possibleCollectors.ElementAt(collectorIndex);

        //    var collector = new Collector(location, collectorType, settlement);
        //    location.BuiltCollector = collector;
        //    settlement.Collectors.Add(collector);

        //}

        //private void BuildBuilding(Settlement settlement)
        //{
        //    var consumption = settlement.GetResourceConsumption();
        //    var excessResources = settlement.Resources.Where(res => res.Value - consumption[res.Key] > 0).Select(res => res.Key);
        //    var possibleBuildings = _conf.Lists.BuildingTypes.Where(b => b.Value.Input.All(res => excessResources.Contains(res.Key)));
        //    if (!possibleBuildings.Any())
        //    {
        //        return;
        //    }
        //    var buildingIndex = rnd.Next(possibleBuildings.Count());
        //    var buildingType = possibleBuildings.ElementAt(buildingIndex);

        //    var building = new Building(settlement, buildingType.Value);
        //    settlement.Buildings.Add(building);

        //}

        //private void GrowPopulation(Settlement settlement)
        //{
        //    //if (_conf.GrowthDemand.All(res => res.Value * settlement.Population < settlement.Resources[res.Key]))
        //    //{
        //    //    settlement.Population++;
        //    //    foreach (var resource in _conf.GrowthDemand)
        //    //    {
        //    //        settlement.Resources[resource.Key] -= resource.Value;
        //    //    }
        //    //}
        //}

        //private void UpdatePrices(Settlement settlement)
        //{
        //    //Log.StartEntry("Settlement {" + settlement.Position.X + ";" + settlement.Position.Y + "} updated prices { ");
        //    //var consumption = settlement.GetResourceConsumption();
        //    //foreach (var resource in settlement.Resources)
        //    //{
        //    //    if (resource.Key != _conf.MoneyResource)
        //    //    {
        //    //        var resourceDesiredBalance = resource.Value - consumption[resource.Key] * _conf.DesiredReserveTurns;
        //    //        double resourceRelativeBalance;
        //    //        if (consumption[resource.Key] != 0)
        //    //        {
        //    //            resourceRelativeBalance = Math.Abs(resourceDesiredBalance / consumption[resource.Key] / _conf.DesiredReserveTurns);
        //    //        }
        //    //        else
        //    //        {
        //    //            resourceRelativeBalance = _conf.UndesiredEffectiveExcess;
        //    //        }

        //    //        if (resourceDesiredBalance > 0)
        //    //        {
        //    //            Log.ContinueEntry(resource.Key.Id + " from " + settlement.Prices[resource.Key]);
        //    //            var priceDivide = resourceRelativeBalance * _conf.ExcessPriceDivider;
        //    //            if (priceDivide > _conf.MaxExcessPriceDivider)
        //    //            {
        //    //                priceDivide = _conf.MaxExcessPriceDivider;
        //    //            }
        //    //            settlement.Prices[resource.Key] /= 1 + priceDivide;
        //    //            if (settlement.Prices[resource.Key] < _conf.MinPrice)
        //    //            {
        //    //                settlement.Prices[resource.Key] = _conf.MinPrice;
        //    //            }
        //    //            if (settlement.Prices[resource.Key] > _conf.MaxPrice)
        //    //            {
        //    //                settlement.Prices[resource.Key] = _conf.MaxPrice;
        //    //            }
        //    //            Log.ContinueEntry(" to " + settlement.Prices[resource.Key] + "; ");
        //    //        }
        //    //        else
        //    //        {
        //    //            Log.ContinueEntry(resource.Key.Id + " from " + settlement.Prices[resource.Key]);
        //    //            settlement.Prices[resource.Key] *= 1 + resourceRelativeBalance * _conf.StarvePriceMultiply;
        //    //            if (settlement.Prices[resource.Key] < _conf.MinPrice)
        //    //            {
        //    //                settlement.Prices[resource.Key] = _conf.MinPrice;
        //    //            }
        //    //            if (settlement.Prices[resource.Key] > _conf.MaxPrice)
        //    //            {
        //    //                settlement.Prices[resource.Key] = _conf.MaxPrice;
        //    //            }
        //    //            Log.ContinueEntry(" to " + settlement.Prices[resource.Key] + "; ");
        //    //        }
        //    //    }
        //    //}
        //    //Log.FinishEntry("}");
        //}

        //private void TradeGoods()
        //{
        //    foreach (var settlement in Settlements)
        //    {
        //        var consumption = settlement.GetResourceConsumption();
        //        foreach (var partner in Settlements)
        //        {
        //            if (partner != settlement)
        //            {
        //                TradeGoodsFrom(settlement, partner, consumption);
        //            }
        //        }
        //    }
        //}

        //private void TradeGoodsFrom(Settlement source, Settlement destination, Dictionary<ResourceType, double> consumption)
        //{
        //    var moneyResource = _conf.Const.MoneyResource;
        //    if (destination.Resources[moneyResource] == 0)
        //    {
        //        return;
        //    }
        //    foreach (var resource in source.Resources)
        //    {
        //        if (resource.Key != moneyResource)
        //        {
        //            double tradeAmount = resource.Value - consumption[resource.Key];// * _conf.DesiredReserveTurns;
        //            if ((tradeAmount > 0) && (source.Prices[resource.Key] < destination.Prices[resource.Key]))
        //            {
        //                double tradePrice = tradeAmount * destination.Prices[resource.Key];
        //                if (tradePrice > destination.Resources[moneyResource])
        //                {
        //                    tradePrice = destination.Resources[moneyResource];
        //                    tradeAmount = tradePrice / destination.Prices[resource.Key];
        //                }
        //                destination.Resources[resource.Key] += tradeAmount;
        //                destination.Resources[moneyResource] -= tradePrice;
        //                source.Resources[resource.Key] -= tradeAmount;
        //                source.Resources[moneyResource] += tradePrice;
        //            }
        //        }
        //    }
        //}
    }
}
