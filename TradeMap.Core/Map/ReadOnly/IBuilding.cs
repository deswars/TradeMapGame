namespace TradeMap.Core.Map.ReadOnly
{
    public interface IBuilding
    {
        public ISettlement Owner { get; }
        public IBuildingType Type { get; }
        public double Power { get; }
        public int Level { get; }
    }
}
