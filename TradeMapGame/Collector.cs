namespace TradeMapGame
{
    public class Collector
    {
        public Settlement Owner { get; }
        public Point Position { get; }
        public Cell Location { get; }
        public CollectorType Type { get; }


        public Collector(Point position, Cell location, CollectorType type, Settlement owner)
        {
            Owner = owner;
            Position = position;
            Location = location;
            Type = type;
        }
    }
}
