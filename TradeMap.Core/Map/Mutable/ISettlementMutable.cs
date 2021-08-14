using System.Collections.Generic;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Core.Map.Mutable
{
    public interface ISettlementMutable : ISettlement
    {
        public KeyedVectorFull<IResourceType> ResourcesMut { get; set; }
        public KeyedVectorFull<IResourceType> PricesMut { get; set; }
        public int PopulationMut { get; set; }
        public List<ICollectorMutable> CollectorsMut { get; }
        public List<IBuildingMutable> BuildingsMut { get; }
        public FlagRepository FlagsMut { get; }
        public void ChangeLevel(int level, double levelProgress);
    }
}
