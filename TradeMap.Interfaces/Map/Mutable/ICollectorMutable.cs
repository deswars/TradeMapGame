using TradeMap.Interfaces.Map.ReadOnly;

namespace TradeMap.Interfaces.Map.Mutable
{
    public interface ICollectorMutable : ICollector
    {
        public ISettlementMutable OwnerMut { get; }
        public ICellMutable LocationMut { get; }
        public void ChangeLevel(int level, double power);
    }
}
