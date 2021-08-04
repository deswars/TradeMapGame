namespace TradeMap.Interfaces.Map.ReadOnly
{
    public interface IResourceType
    {
        public string Id { get; }
        public double BasePrice { get; }
        public double DecayRate { get; }
    }
}
