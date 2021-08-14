using System.Collections.Generic;

namespace TradeMap.Core.Map.ReadOnly
{
    public interface ICell
    {
        public ITerrainType Terrain { get; }
        public IReadOnlyList<ITerrainFeautre> Feautres { get; }
        public Point Position { get; }
        public ICollector? Collector { get; }
        public ISettlement? Settlement { get; }
        public IReadOnlyFlagRepository Flags { get; }
    }
}
