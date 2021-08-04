using System.Collections.Generic;
using TradeMap.Core;

namespace TradeMap.Interfaces.Map.ReadOnly
{
    public interface ICell
    {
        public ITerrainType Terrain { get; }
        public IReadOnlyList<ITerrainFeautre> MapFeautres { get; }
        public Point Position { get; }
        public ICollector Collector { get; }
        public ISettlement Settlement { get; }
        public IReadOnlyFlagRepository Flags { get; }
    }
}
