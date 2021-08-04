using System.Collections.Generic;

namespace TradeMap.Interfaces.Map.ReadOnly
{
    public interface ITerrainFeautre
    {
        public string Id { get; }
        public IReadOnlyList<IResourceDeposit> Resources { get; }
    }
}
