using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Core.Map.Mutable
{
    public interface IBuildingMutable : IBuilding
    {
        public ISettlementMutable OwnerMut { get; }
        public void ChangeLevel(int level, double power);
    }
}
