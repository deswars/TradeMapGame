using Newtonsoft.Json.Linq;
using TradeMap.Core;
using TradeMap.Di.Attributes;

namespace TradeMap.Di.Parsers
{
    [SupportedType(typeof(double))]
    public class ParserDouble : IParserJson
    {
        public bool TryParse(JToken token, ITypeRepository repo, out object? result)
        {
            if (!SupportedType(token.Type))
            {
                result = null;
                return false;
            }

            result = token.Value<double>();
            return true;
        }

        public override string ToString()
        {
            return nameof(ParserDouble);
        }

        private static bool SupportedType(JTokenType tokenType)
        {
            return tokenType == JTokenType.Integer || tokenType == JTokenType.Float;
        }
    }
}
