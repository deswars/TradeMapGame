using System.Collections.Generic;
using TradeMapGame.Map;

namespace TradeMapGame.Configuration
{
    public class TypeRepository
    {
        public Dictionary<string, TerrainType> TerrainTypes { get; } = new();
        public Dictionary<string, ResourceType> ResourceTypes { get; } = new();
        public Dictionary<string, TerrainFeautre> MapFeautreTypes { get; } = new();
        public Dictionary<string, CollectorType> CollectorTypes { get; } = new();
        public Dictionary<string, BuildingType> BuildingTypes { get; } = new();
        public Dictionary<int, Dictionary<ResourceType, double>> PopulationDemands { get; } = new();
    }
}
