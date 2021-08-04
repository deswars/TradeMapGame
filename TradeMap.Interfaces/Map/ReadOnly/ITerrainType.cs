using System.Collections.Generic;

namespace TradeMap.Interfaces.Map.ReadOnly
{
    public interface ITerrainType
    {
        public string Id { get; }
        public IReadOnlyList<IResourceDeposit> Resources { get; }
    }
}
