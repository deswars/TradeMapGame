using System.Collections.Generic;
using TradeMap.Localization;

namespace TradeMap.GameLog
{
    public class BaseLogEntry : ILogEntry
    {
        public InfoLevels InfoLevel { get; set; }


        private readonly string _name;
        private readonly Dictionary<string, object>? _variables;


        public BaseLogEntry(string name, Dictionary<string, object>? variables)
        {
            _name = name;
            _variables = variables;
        }


        public virtual string ToText(ITextLocalizer localizer)
        {
            return localizer.Expand($"[{_name}]", _variables);
        }
    }
}
