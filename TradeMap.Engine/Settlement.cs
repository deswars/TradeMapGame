using System.Collections.Generic;

namespace TradeMap.Core.Map
{
    public class Settlement
    {
        public string Name { get; }
        public Point Position { get; }
        public KeyedVectorFull<ResourceType> Resources { get; }
        public KeyedVectorFull<ResourceType> Prices { get; }
        public int Population { get; set; }
        public List<Collector> Collectors { get; }
        public List<Building> Buildings { get; }
        public int Tier { get; set; }
        public double TierProgress { get; set; }

        public Settlement(string name, Point position, int population, KeyedVectorFull<ResourceType> resources)
        {
            Name = name;
            Position = position;
            Resources = resources;
            Population = population;

            Collectors = new();
            Buildings = new();
            Prices = new();
            foreach (var resource in Resources)
            {
                Prices[resource.Key] = resource.Key.BasePrice;
            }
        }
    }
}
