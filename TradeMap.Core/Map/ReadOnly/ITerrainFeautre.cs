using System.Collections.Generic;

namespace TradeMap.Core.Map.ReadOnly
{
    public interface ITerrainFeautre
    {
        public string Id { get; }
        public IReadOnlyList<ITerrainType> RequiredTerrain { get; }
        public IReadOnlyList<IResourceDeposit> Deposits { get; }
    }
}
