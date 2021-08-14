using System.Collections.Generic;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Core.Map.Mutable
{
    public interface ICellMutable : ICell
    {
        public ICollectorMutable? CollectorMut { get; set; }
        public ISettlementMutable? SettlementMut { get; set; }
        public FlagRepository FlagsMut { get; }
        public void Terraform(ITerrainType terrain, IReadOnlyList<ITerrainFeautre> feautres);
    }
}
