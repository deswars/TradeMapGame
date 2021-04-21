using System.Collections.Generic;

namespace TradeMapGame
{
    public class Cell
    {
        public TerrainType Terrain { get; set; }
        public List<TerrainFeautre> MapFeautres { get; }

        public Cell(TerrainType terrain)
        {
            Terrain = terrain;
            MapFeautres = new();
        }
    }
}
