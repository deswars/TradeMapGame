using Newtonsoft.Json.Linq;
using TradeMap.Core;
using TradeMap.Di.Attributes;

namespace TradeMap.Di.Parsers
{
    [SupportedType(typeof(string))]
    public class ParserString : IParserJson
    {
        public bool TryParse(JToken token, ITypeRepository repo, out object? result)
        {
            if (!SupportedType(token.Type))
            {
                result = null;
                return false;
            }

            result = token.Value<string>();
            return true;
        }

        public override string ToString()
        {
            return nameof(ParserString);
        }

        private static bool SupportedType(JTokenType tokenType)
        {
            return tokenType == JTokenType.String;
        }
    }
}
