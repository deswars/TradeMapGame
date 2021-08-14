using System;
using System.Collections.Generic;
using System.Globalization;

namespace TradeMap.Localization
{
    public class BasicTextLocalizer : ITextLocalizer
    {
        private readonly CultureInfo _culture;


        public BasicTextLocalizer(CultureInfo culture)
        {
            _culture = culture;
        }

        public string Expand(string text, Dictionary<string, object>? variables)
        {
            if (variables == null)
            {
                return Expand(text);
            }
            string res = text;
            string separator = _culture.TextInfo.ListSeparator;
            foreach (var pair in variables)
            {
                res += separator + pair.Key + "=" + Convert.ToString(pair.Value, _culture);
            }
            return res;
        }

        public string Expand(string text)
        {
            return text;
        }
    }
}
