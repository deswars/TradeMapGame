using TradeMap.Core.Map.Mutable;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Engine.Map
{
    public class Collector : ICollectorMutable
    {
        public ISettlement Owner { get => OwnerMut; }
        public ICell Location { get => LocationMut; }
        public ICollectorType Type { get; }
        public double Power { get; private set; }
        public int Level { get; private set; }

        public ISettlementMutable OwnerMut { get; set; }
        public ICellMutable LocationMut { get; set; }


        public Collector(ISettlementMutable owner, ICellMutable location, ICollectorType type, double power, int level)
        {
            OwnerMut = owner;
            LocationMut = location;
            Type = type;
            Power = power;
            Level = level;
        }


        public void ChangeLevel(int level, double power)
        {
            Level = level;
            Power = power;
        }
    }
}
