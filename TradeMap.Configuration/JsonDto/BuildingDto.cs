using Newtonsoft.Json;
using System.Collections.Generic;

namespace TradeMap.Configuration.JsonDto
{
    class BuildingDto
    {
        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; } = "";

        [JsonProperty(Required = Required.Always)]
        public int BaseLevel { get; set; } = 0;

        [JsonProperty(Required = Required.Always)]
        public List<ResourceDeltaDto> Input { get; set; } = new();

        [JsonProperty(Required = Required.Always)]
        public List<ResourceDeltaDto> Output { get; set; } = new();
    }
}
