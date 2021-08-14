using System.Collections.Generic;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Engine.Map
{
    public class ResourceType : IResourceType
    {
        public string Id { get; }
        public double BasePrice { get; }
        public IReadOnlySet<string> Tags { get; }


        public ResourceType(string id, double basePrice, IReadOnlySet<string> tags)
        {
            Id = id;
            BasePrice = basePrice;
            Tags = tags;
        }


        public override string ToString()
        {
            var result = $"({Id} {BasePrice} [ ";
            foreach (var tag in Tags)
            {
                result += tag + " ";
            }
            result += "])";
            return result;
        }
    }
}
