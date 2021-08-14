using System.Collections.Generic;

namespace TradeMap.Core.Map.ReadOnly
{
    public interface IResourceType
    {
        public string Id { get; }
        public double BasePrice { get; }
        public IReadOnlySet<string> Tags { get; }
    }
}
