using Newtonsoft.Json;

namespace TradeMap.Configuration.JsonDto
{
    class ResourceDeltaDto
    {
        [JsonProperty(Required = Required.Always)]
        public string Resource { get; set; } = "";

        [JsonProperty(Required = Required.Always)]
        public double Amount { get; set; } = 0;
    }
}
