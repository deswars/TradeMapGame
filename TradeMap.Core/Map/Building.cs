namespace TradeMap.Core.Map
{
    public class Building
    {
        public Settlement Owner { get; }
        public BuildingType Type { get; }

        public Building(Settlement owner, BuildingType type)
        {
            Owner = owner;
            Type = type;
        }
    }
}
