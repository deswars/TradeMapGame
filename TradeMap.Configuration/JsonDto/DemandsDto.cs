using Newtonsoft.Json;
using System.Collections.Generic;

namespace TradeMap.Configuration.JsonDto
{
    class DemandsDto
    {
        [JsonProperty(Required = Required.Always)]
        public int Level { get; set; } = 0;

        [JsonProperty(Required = Required.Always)]
        public List<ResourceDeltaDto> Demands { get; set; } = new();
    }
}
