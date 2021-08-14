using System.Collections.Generic;

namespace TradeMap.Core.Map.ReadOnly
{
    public interface ICollectorType
    {
        public string Id { get; }
        public double BasePower { get; }
        public int BaseLevel { get; }
        public IReadOnlyList<ITerrainType> RequiredTerrain { get; }
        public IReadOnlyList<ITerrainFeautre> RequiredFeautre { get; }
        public IReadOnlyList<IResourceType> Collected { get; }
    }
}
