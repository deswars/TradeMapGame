using Newtonsoft.Json.Linq;
using TradeMap.Core;

namespace TradeMap.Di
{
    public interface IParserJson
    {
        public bool TryParse(JToken token, ITypeRepository repo, out object? result);
    }
}
