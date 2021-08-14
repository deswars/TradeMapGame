using TradeMap.Core.Map.Mutable;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Engine.Map
{
    public class Building : IBuildingMutable
    {
        public ISettlement Owner { get => OwnerMut; }
        public IBuildingType Type { get; }
        public double Power { get; private set; }
        public int Level { get; private set; }

        public ISettlementMutable OwnerMut { get; }


        public Building(ISettlementMutable owner, IBuildingType type, double power, int level)
        {
            OwnerMut = owner;
            Type = type;
            Power = power;
            Level = level;
        }


        public void ChangeLevel(int level, double power)
        {
            Power = power;
            Level = level;
        }
    }
}
