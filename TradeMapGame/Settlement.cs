using System.Collections.Generic;

namespace TradeMapGame
{
    public class Settlement
    {
        public Point Position { get; }
        public Dictionary<ResourceType, double> Resources { get; }
        public Dictionary<ResourceType, double> Prices { get; }
        public int Population { get; set; }
        public List<Collector> Collectors { get; }
        public List<Building> Buildings { get; }

        public Settlement(Configuration configuration, Point position, int population, Dictionary<ResourceType, double> resources)
        {
            Position = position;
            Resources = resources;
            Population = population;
            _conf = configuration;

            Collectors = new();
            Buildings = new();
            Prices = new();
            foreach (var resource in Resources)
            {
                Prices.Add(resource.Key, resource.Key.BasePrice);
            }
        }

        public void GatherResource()
        {
            foreach(var collector in Collectors)
            {
                collector.Collect();
            }
            foreach(var building in Buildings)
            {
                building.Produce();
            }
        }

        public int GetFreePopulation()
        {
            return Population - Collectors.Count - Buildings.Count;
        }

        public Dictionary<ResourceType, double> GetResourceConsumption()
        {
            var popDemands = _conf.PopulationDemands;
            Dictionary<ResourceType, double> result = new();
            foreach (var demand in popDemands)
            {
                result.Add(demand.Key, demand.Value * Population);
            }
            foreach (var build in Buildings)
            {
                foreach (var res in build.Type.Input)
                {
                    result[res.Key] += res.Value;
                }
            }
            return result;
        }

        private readonly Configuration _conf;
    }
}
