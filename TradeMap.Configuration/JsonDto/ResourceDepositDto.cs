using Newtonsoft.Json;

namespace TradeMap.Configuration.JsonDto
{
    class ResourceDepositDto
    {
        [JsonProperty(Required = Required.Always)]
        public string Resource { get; set; } = "";

        [JsonProperty(Required = Required.Always)]
        public double Richness { get; set; } = 0;

        [JsonProperty(Required = Required.Always)]
        public double Hardness { get; set; } = 0;
    }
}
