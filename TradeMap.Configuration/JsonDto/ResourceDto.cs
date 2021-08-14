using System.Collections.Generic;

namespace TradeMap.Configuration.JsonDto
{
    class ResourceDto
    {
        public string Id { get; set; } = "";
        public double BasePrice { get; set; } = 0;
        public HashSet<string> Tags { get; set; } = new();
    }
}
