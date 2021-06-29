using System.Collections.Generic;

namespace TradeMap.Core.Map
{
    public interface ICell
    {
        Collector? BuiltCollector { get; set; }
        List<TerrainFeautre> MapFeautres { get; }
        Point Position { get; }
        Settlement? Settlement { get; }
        TerrainType Terrain { get; set; }
    }
}