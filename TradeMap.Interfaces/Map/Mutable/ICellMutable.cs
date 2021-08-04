using System.Collections.Generic;
using TradeMap.Core;
using TradeMap.Interfaces.Map.ReadOnly;

namespace TradeMap.Interfaces.Map.Mutable
{
    public interface ICellMutable : ICell
    {
        public ICollectorMutable CollectorMutable { get; set; }
        public ISettlementMutable SettlementMutable { get; set; }
        public FlagRepository FlagsMut { get; }
        public void Terraform(ITerrainType terrain, IReadOnlyList<ITerrainFeautre> feautres);
        public void SetFlag(string name, string value);
        public void SetFlagInt(string name, int value);
        public void SetFlagDouble(string name, double value);
        public void ClearFlags();
    }
}
