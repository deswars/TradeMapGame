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

        public void Collect()
        {
            var resources = Owner.Resources;
            foreach (var collectable in Type.Collected)
            {
                double richness = Location.GetRichness(collectable);
                if (richness > 0)
                {
                    resources[collectable] += richness;
                }
            }
        }
    }
}
