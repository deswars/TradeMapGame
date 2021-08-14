using System.Collections.Generic;
using TradeMap.Core;
using TradeMap.Core.Map.ReadOnly;
using TradeMap.Di.Attributes;
using TradeMap.Engine.Map;

namespace TradeMap.Configuration
{
    class TypeRepository : ITypeRepository
    {
        public const string NullResourceId = "r_null";
        public const string NullTerrainId = "t_null";


        IReadOnlyDictionary<string, IResourceType> ITypeRepository.ResourceTypes { get => ResourceTypes; }
        IReadOnlyDictionary<string, ITerrainType> ITypeRepository.TerrainTypes { get => TerrainTypes; }
        IReadOnlyDictionary<string, ITerrainFeautre> ITypeRepository.TerrainFeautreTypes { get => TerrainFeautreTypes; }
        IReadOnlyDictionary<string, ICollectorType> ITypeRepository.CollectorTypes { get => CollectorTypes; }
        IReadOnlyDictionary<string, IBuildingType> ITypeRepository.BuildingTypes { get => BuildingTypes; }
        IReadOnlyDictionary<int, IReadOnlyKeyedVectorPartial<IResourceType>> ITypeRepository.PopulationDemands { get => PopulationDemands; }


        [TypeCategory("Resources")]
        public Dictionary<string, IResourceType> ResourceTypes { get; }

        [TypeCategory("Terrains")]
        public Dictionary<string, ITerrainType> TerrainTypes { get; }

        [TypeCategory("MapFeautres")]
        public Dictionary<string, ITerrainFeautre> TerrainFeautreTypes { get; }

        [TypeCategory("Collectors")]
        public Dictionary<string, ICollectorType> CollectorTypes { get; }

        [TypeCategory("Buildings")]
        public Dictionary<string, IBuildingType> BuildingTypes { get; }

        [TypeCategory("Population")]
        public Dictionary<int, IReadOnlyKeyedVectorPartial<IResourceType>> PopulationDemands { get; }


        public TypeRepository()
        {
            ResourceTypes = new() { { NullResourceId, new ResourceType(NullResourceId, 0, new HashSet<string>() { "null" }) } };
            TerrainTypes = new() { { NullTerrainId, new TerrainType(NullTerrainId, new List<IResourceDeposit>()) } };
            TerrainFeautreTypes = new();
            CollectorTypes = new();
            BuildingTypes = new();
            PopulationDemands = new();
        }


        public bool Contains(string id)
        {
            return
                ResourceTypes.ContainsKey(id) ||
                TerrainTypes.ContainsKey(id) ||
                TerrainFeautreTypes.ContainsKey(id) ||
                CollectorTypes.ContainsKey(id) ||
                BuildingTypes.ContainsKey(id);
        }
    }
}
