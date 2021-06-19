using System.Collections.Generic;

namespace TradeMap.Localization
{
    public interface ITextLocalizer
    {
        public string Expand(string text, Dictionary<string, object> variables);
    }
}
