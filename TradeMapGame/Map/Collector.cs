namespace TradeMapGame.Map
{
    public class Collector
    {
        public Settlement Owner { get; }
        public Cell Location { get; }
        public CollectorType Type { get; }

        public Collector(Cell location, CollectorType type, Settlement owner)
        {
            Owner = owner;
            Location = location;
            Type = type;
        }
    }
}
