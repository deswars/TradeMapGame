using Newtonsoft.Json;
using System.Collections.Generic;

namespace TradeMap.Configuration.JsonDto
{
    class CollectorDto
    {
        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; } = "";

        [JsonProperty(Required = Required.Always)]
        public double BasePower { get; set; } = 0;

        [JsonProperty(Required = Required.Always)]
        public int BaseLevel { get; set; } = 0;

        [JsonProperty(Required = Required.Always)]
        public List<string> RequiredTerrain { get; set; } = new();

        [JsonProperty(Required = Required.Always)]
        public List<string> RequiredFeautre { get; set; } = new();

        [JsonProperty(Required = Required.Always)]
        public List<string> Collected { get; set; } = new();
    }
}
