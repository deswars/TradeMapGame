using TradeMap.Interfaces.Map.ReadOnly;

namespace TradeMap.Interfaces.Map.Mutable
{
    public interface IBuildingMutable : IBuilding
    {
        public ISettlementMutable OwnerMut { get; }
        public void ChangeLevel(int level, double power);
    }
}
