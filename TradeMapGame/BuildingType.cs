using System.Collections.Generic;

namespace TradeMapGame
{
    public class BuildingType
    {
        public string Id { get; }
        public Dictionary<ResourceType, double> Input { get; }
        public Dictionary<ResourceType, double> Output { get; }


        public BuildingType(string id, Dictionary<ResourceType, double> input, Dictionary<ResourceType, double> output)
        {
            Id = id;
            Input = input;
            Output = output;
        }
    }
}
