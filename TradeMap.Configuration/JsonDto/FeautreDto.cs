using Newtonsoft.Json;
using TradeMap.Core;

namespace TradeMap.Configuration.JsonDto
{
    class FeautreDto
    {
        [JsonProperty(Required = Required.Always)]
        public SubturnTypes SubturnType { get; set; } = SubturnTypes.Settlement;

        [JsonProperty(Required = Required.Always)]
        public string ServiceName { get; set; } = "";

        [JsonProperty(Required = Required.Always)]
        public string FeautreName { get; set; } = "";

        [JsonProperty(Required = Required.Always)]
        public int Substep { get; set; } = 0;


        public FeautreInfo ConvertToFeatreInfo()
        {
            return new FeautreInfo(SubturnType, ServiceName, FeautreName);
        }
    }
}
