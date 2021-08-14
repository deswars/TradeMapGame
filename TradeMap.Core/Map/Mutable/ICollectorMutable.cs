using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Core.Map.Mutable
{
    public interface ICollectorMutable : ICollector
    {
        public ISettlementMutable OwnerMut { get; }
        public ICellMutable LocationMut { get; }
        public void ChangeLevel(int level, double power);
    }
}
