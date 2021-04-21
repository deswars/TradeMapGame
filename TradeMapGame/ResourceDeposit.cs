namespace TradeMapGame
{
    public class ResourceDeposit
    {
        public ResourceType Type { get; }
        public double Difficulty { get; }
        public double Richness { get; }


        public ResourceDeposit(ResourceType type, double difficulty, double richness)
        {
            Type = type;
            Difficulty = difficulty;
            Richness = richness;
        }
    }
}
