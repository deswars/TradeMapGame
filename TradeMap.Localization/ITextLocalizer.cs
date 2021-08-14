using System.Collections.Generic;

namespace TradeMap.Localization
{
    public interface ITextLocalizer
    {
        string Expand(string text, Dictionary<string, object>? variables);
        string Expand(string text);
    }
}
