using Newtonsoft.Json.Linq;
using TradeMap.Core;
using TradeMap.Di.Attributes;

namespace TradeMap.Di.Parsers
{
    [SupportedType(typeof(int))]
    public class ParserInt : IParserJson
    {
        public bool TryParse(JToken token, ITypeRepository repo, out object? result)
        {
            if (!SupportedType(token.Type))
            {
                result = null;
                return false;
            }

            result = token.Value<int>();
            return true;
        }

        public override string ToString()
        {
            return nameof(ParserInt);
        }

        private static bool SupportedType(JTokenType tokenType)
        {
            return tokenType == JTokenType.Integer;
        }
    }
}
