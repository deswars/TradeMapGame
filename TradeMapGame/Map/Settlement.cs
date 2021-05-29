using System.Collections.Generic;

namespace TradeMapGame.Map
{
    public class Settlement
    {
        public string Name { get; }
        public Point Position { get; }
        public KeyedVector<ResourceType> Resources { get; }
        public KeyedVector<ResourceType> Prices { get; }
        public int Population { get; set; }
        public List<Collector> Collectors { get; }
        public List<Building> Buildings { get; }
        public int Tier { get; set; }
        public double TierProgress { get; set; }

        public Settlement(string name, Point position, int population, KeyedVector<ResourceType> resources)
        {
            Name = name;
            Position = position;
            Resources = resources;
            Population = population;

            Collectors = new();
            Buildings = new();
            Prices = Resources.Zeroed();
            foreach (var resource in Resources)
            {
                Prices[resource.Key] = resource.Key.BasePrice;
            }
        }
    }
}
