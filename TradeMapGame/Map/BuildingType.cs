namespace TradeMapGame.Map
{
    public class BuildingType
    {
        public string Id { get; }
        public KeyedVector<ResourceType> Input { get; }
        public KeyedVector<ResourceType> Output { get; }

        public BuildingType(string id, KeyedVector<ResourceType> input, KeyedVector<ResourceType> output)
        {
            Id = id;
            Input = input;
            Output = output;
        }
    }
}
