using TradeMap.Core;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Engine.Map
{
    public class BuildingType : IBuildingType
    {
        public string Id { get; }
        public int BaseLevel { get; }
        public IReadOnlyKeyedVectorPartial<IResourceType> Input { get; }
        public IReadOnlyKeyedVectorPartial<IResourceType> Output { get; }


        public BuildingType(string id, int baseLevel, IReadOnlyKeyedVectorPartial<IResourceType> input, IReadOnlyKeyedVectorPartial<IResourceType> output)
        {
            Id = id;
            BaseLevel = baseLevel;
            Input = input;
            Output = output;
        }


        public override string ToString()
        {
            return $"({Id} {BaseLevel} {Input} {Output})";
        }
    }
}
