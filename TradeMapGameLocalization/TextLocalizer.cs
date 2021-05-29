using System.Collections.Generic;

namespace TradeMapGame.Localization
{
    public class TextLocalizer
    {
        public string Expand(string text, Dictionary<string, object> variables)
        {
            //TODO implement
            string res = text;
            foreach (var pair in variables)
            {
                res += "," + pair.Key + "=" + pair.Value.ToString();
            }
            return res;
        }
    }
}
