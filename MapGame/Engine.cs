using MapGame.Core;
using MapGame.Core.EntityTypes;
using System;
using System.Collections.Generic;

namespace MapGame
{
    public class Engine
    {
        public Engine(List<TerrainType> terrains, IMap map, IPathFinder pathFinder)
        {
            _terrains = terrains;
            _map = map;
            _pathFinder = pathFinder;
        }

        public bool AddSettlement(Settlement settlement)
        {
            if (!_map.AddSettlement(settlement))
            {
                return false;
            }
            return true;
        }

        public void RemoveSettlement(Settlement settlement)
        {
            _map.RemoveSettlement(settlement);
            _routes.RemoveAll(x => (x.StartSettlement == settlement) || (x.EndSettlement == settlement));
        }

        public void NextStep()
        {
            throw new NotImplementedException();
        }

        private readonly List<TerrainType> _terrains;
        private readonly IMap _map;
        private readonly IPathFinder _pathFinder;
        private readonly List<Route> _routes = new();
    }
}
