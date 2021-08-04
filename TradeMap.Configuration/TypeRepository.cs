using System.Collections.Generic;
using TradeMap.Core;
using TradeMap.Core.Map;
using TradeMap.Di.Attributes;

namespace TradeMap.Configuration
{
    public class TypeRepository
    {
        [Type("Resources")]
        public IReadOnlyDictionary<string, ResourceType> ResourceTypes { get; }

        [Type("Terrain")]
        public IReadOnlyDictionary<string, TerrainType> TerrainTypes { get; }

        [Type("MapFeautres")]
        public IReadOnlyDictionary<string, TerrainFeautre> MapFeautreTypes { get; }

        [Type("Collectors")]
        public IReadOnlyDictionary<string, CollectorType> CollectorTypes { get; }

        [Type("Buildings")]
        public IReadOnlyDictionary<string, BuildingType> BuildingTypes { get; }

        [Type("Population")]
        public IReadOnlyDictionary<int, KeyedVectorFull<ResourceType>> PopulationDemands { get; }

        public TypeRepository(
            IReadOnlyDictionary<string, ResourceType> resourceTypes,
            IReadOnlyDictionary<string, TerrainType> terrainTypes, 
            IReadOnlyDictionary<string, TerrainFeautre> mapFeautreTypes, 
            IReadOnlyDictionary<string, CollectorType> collectorTypes, 
            IReadOnlyDictionary<string, BuildingType> buildingTypes, 
            IReadOnlyDictionary<int, KeyedVectorFull<ResourceType>> populationDemands)
        {
            ResourceTypes = resourceTypes;
            TerrainTypes = terrainTypes;
            MapFeautreTypes = mapFeautreTypes;
            CollectorTypes = collectorTypes;
            BuildingTypes = buildingTypes;
            PopulationDemands = populationDemands;
        }
    }
}
