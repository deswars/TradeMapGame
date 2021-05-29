using System.Collections.Generic;
using System.Linq;

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

        public double GetRichness(ResourceType resource)
        {
            var result = Terrain.Resources.Where(x => x.Type == resource).Select(x => x.Richness).Append(0).Sum();
            return MapFeautres.SelectMany(
                feautre => feautre.Resources.Where(x => x.Type == resource).Select(x => x.Richness)
                ).Append(result).Sum();
        }
    }
}
