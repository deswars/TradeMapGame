using System.Collections.Generic;

namespace TradeMap.Core.Map.ReadOnly
{
    public interface ITerrainType
    {
        public string Id { get; }
        public IReadOnlyList<IResourceDeposit> Deposits { get; }
    }
}
