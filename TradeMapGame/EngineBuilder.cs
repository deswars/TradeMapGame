using System;
using TradeMapGame.Configuration;
using TradeMapGame.Log;
using TradeMapGame.Map;
using TradeMapGame.Services;

namespace TradeMapGame
{
    public static class EngineBuilder
    {
        public static Engine Build(SquareDiagonalMap map, ConfigurationLoader conf, TurnLog log)
        {
            Random rnd = new();
            Engine eng = new(map, conf, log);



            ServiceGatherResource gather = new(log);
            eng.OnPreTurn += gather.CollectorsGatherResources;
            eng.OnPreTurn += gather.BuildingsProduceResources;

            return eng;

            //Log.SetTurn(Turn);
            //foreach (var settlement in Settlements)
            //{
            //    //    //if (Turn % _conf.ExpansionDelay == 0)
            //    {
            //        ExpandSettlement(settlement);
            //    }
            //    GrowPopulation(settlement);
            //}
            //TradeGoods();
            //foreach (var settlement in Settlements)
            //{
            //    UpdatePrices(settlement);
            //}
        }
    }
}
