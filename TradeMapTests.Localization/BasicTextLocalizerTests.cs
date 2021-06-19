using Xunit;
using TradeMap.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace TradeMapTests.Localization
{
    public class BasicTextLocalizerTests
    {
        [Fact()]
        public void ExpandTest()
        {
            BasicTextLocalizer loc = new(CultureInfo.InvariantCulture);
            string text = "[a]";
            Dictionary<string, object> variables = new();
            variables["v1"] = "abc";
            variables["v2"] = 10;
            variables["v3"] = 11.1;

            string expected = "[a],v1=abc,v2=10,v3=11.1";
            var result = loc.Expand(text, variables);

            Assert.Equal(expected, result);
        }
    }
}