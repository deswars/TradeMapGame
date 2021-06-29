using System.Collections.Generic;

namespace TradeMap.Core.Map
{
    public class Cell : ICell
    {
        public TerrainType Terrain { get; set; }
        public List<TerrainFeautre> MapFeautres { get; }
        public Point Position { get; }
        public Collector? BuiltCollector { get; set; }
        public Settlement? Settlement { get; set; }
        public Cell(TerrainType terrain, Point position)
        {
            Terrain = terrain;
            Position = position;
            MapFeautres = new();
        }
    }
}
