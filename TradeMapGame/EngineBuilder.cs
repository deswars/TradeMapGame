using System;
using TradeMapGame.Configuration;
using TradeMapGame.Map;
using TradeMapGame.Services;
using TradeMapGame.TurnLog;

namespace TradeMapGame
{
    public static class EngineBuilder
    {
        public static Engine Build(SquareDiagonalMap map, ConfigurationLoader conf, TurnLogImpl log)
        {
            Random rnd = new();
            Engine eng = new(map, conf, log);



            ServiceGatherResource gather = new(log);
            eng.OnPreTurn += gather.CollectorsGatherResources;
            eng.OnPreTurn += gather.BuildingsProduceResources;

            return eng;
            //TODO finish all steps

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
