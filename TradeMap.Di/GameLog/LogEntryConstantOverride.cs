using System.Collections.Generic;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMap.Di.GameLog
{
    public class LogEntryConstantOverride : ILogEntry
    {
        public LogEntryConstantOverride(string constantName, bool isDifferentTypes)
        {
            _constantName = constantName;
            _isDifferentTypes = isDifferentTypes;
        }

        public string ToText(ITextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["constantName"] = _constantName,
                ["isDifferentTypes"] = _isDifferentTypes
            };
            return localizer.Expand("[ConstantOverride]", param);
        }

        private readonly string _constantName;
        private readonly bool _isDifferentTypes;
    }
}
