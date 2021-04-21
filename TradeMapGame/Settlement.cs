using System.Collections.Generic;

namespace TradeMapGame
{
    public class Settlement
    {
        public Point Position { get; }
        public Dictionary<ResourceType, double> Resources { get; }
        public double GatherPower { get; }


        public Settlement(Point position, double gatherPower, Dictionary<ResourceType, double> resources)
        {
            Position = position;
            GatherPower = gatherPower;
            Resources = resources;
        }

        public void GatherResource(Cell cell)
        {
            foreach (var resource in cell.Terrain.Resources)
            {
                Resources[resource.Type] += GatherPower * resource.Richness / resource.Difficulty;
            }
            foreach (var feautre in cell.MapFeautres)
            {
                foreach (var resource in feautre.Resources)
                {
                    Resources[resource.Type] += GatherPower * resource.Richness / resource.Difficulty;
                }
            }
        }
    }
}
