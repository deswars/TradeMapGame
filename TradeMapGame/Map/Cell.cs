using System.Collections.Generic;

namespace TradeMapGame.Map
{
    public class Cell
    {
        public TerrainType Terrain { get; set; }
        public List<TerrainFeautre> MapFeautres { get; }
        public Point Position { get; }
        public Collector? BuiltCollector { get; set; }

        public Cell(TerrainType terrain, Point position)
        {
            Terrain = terrain;
            Position = position;
            MapFeautres = new();
        }
    }
}
