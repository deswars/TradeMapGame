using System.Collections.Generic;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMap.Di.GameLogEntry
{
    public class LogEntryReadOnlyProperty : BaseLogEntry
    {
        public LogEntryReadOnlyProperty(string typeName, string serviceName, string constantName)
        {
            _typeName = typeName;
            _serviceName = serviceName;
            _constantName = constantName;
        }

        public override string ToText(ITextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["typeName"] = _typeName,
                ["serviceName"] = _serviceName,
                ["constantName"] = _constantName
            };
            return localizer.Expand("[ReadOnlyProperty]", param);
        }

        private readonly string _typeName;
        private readonly string _serviceName;
        private readonly string _constantName;
    }
}
