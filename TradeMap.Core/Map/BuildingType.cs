namespace TradeMap.Core.Map
{
    public class BuildingType
    {
        public string Id { get; }
        public KeyedVectorPartial<ResourceType> Input { get; }
        public KeyedVectorPartial<ResourceType> Output { get; }

        public BuildingType(string id, KeyedVectorPartial<ResourceType> input, KeyedVectorPartial<ResourceType> output)
        {
            Id = id;
            Input = input;
            Output = output;
        }
    }
}
