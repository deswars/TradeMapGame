namespace TradeMapGame.Map
{
    public class ResourceDeposit
    {
        public ResourceType Type { get; }
        public double Richness { get; }


        public ResourceDeposit(ResourceType type, double richness)
        {
            Type = type;
            Richness = richness;
        }
    }
}
