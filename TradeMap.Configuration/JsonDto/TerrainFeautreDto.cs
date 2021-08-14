using Newtonsoft.Json;
using System.Collections.Generic;

namespace TradeMap.Configuration.JsonDto
{
    class TerrainFeautreDto
    {
        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; } = "";

        [JsonProperty(Required = Required.Always)]
        public List<string> RequiredTerrain { get; set; } = new();

        [JsonProperty(Required = Required.Always)]
        public List<ResourceDepositDto> Deposits { get; set; } = new();
    }
}
