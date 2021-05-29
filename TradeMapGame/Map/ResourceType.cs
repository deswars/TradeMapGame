namespace TradeMapGame.Map
{
    public class ResourceType
    {
        public string Id { get; }
        public double BasePrice { get; }
        public double DecayRate { get; }


        public ResourceType(string id, double basePrice, double decayRate)
        {
            Id = id;
            BasePrice = basePrice;
            DecayRate = decayRate;
        }
    }
}
