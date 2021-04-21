using System.Collections.Generic;

namespace TradeMapGame
{
    public class Engine
    {
        public Map Map { get; }
        public List<Settlement> Settlements { get; }

        public Engine(TradeMapGame.Map map, Configuration configuration)
        {
            Map = map;
            _configuration = configuration;
            Settlements = new();
        }

        public void NextTick()
        {
            foreach (var settlement in Settlements)
            {
                var position = settlement.Position;
                var cell = Map[position.X, position.Y];
                settlement.GatherResource(cell);
            }
        }


        private readonly Configuration _configuration;
    }
}
