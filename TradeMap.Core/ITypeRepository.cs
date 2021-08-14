using System.Collections.Generic;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Core
{
    public interface ITypeRepository
    {
        IReadOnlyDictionary<string, IResourceType> ResourceTypes { get; }
        IReadOnlyDictionary<string, ITerrainType> TerrainTypes { get; }
        IReadOnlyDictionary<string, ITerrainFeautre> TerrainFeautreTypes { get; }
        IReadOnlyDictionary<string, ICollectorType> CollectorTypes { get; }
        IReadOnlyDictionary<string, IBuildingType> BuildingTypes { get; }
        IReadOnlyDictionary<int, IReadOnlyKeyedVectorPartial<IResourceType>> PopulationDemands { get; }


        bool Contains(string Id);
    }
}
