using TradeMap.Core;

namespace TradeMap.Configuration
{
    public class FeautreInfo
    {
        public SubturnTypes SubturnType { get; }
        public string ServiceName { get; }
        public string FeautreName { get; }


        public FeautreInfo(SubturnTypes subturnType, string serviceName, string feautreName)
        {
            SubturnType = subturnType;
            ServiceName = serviceName;
            FeautreName = feautreName;
        }

        public override string ToString()
        {
            return $"({SubturnType} {ServiceName} {FeautreName})";
        }
    }
}
