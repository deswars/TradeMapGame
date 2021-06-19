using System.Collections.Generic;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMap.Di.GameLog
{
    public class LogEntryConstantTypeError : ILogEntry
    {
        public LogEntryConstantTypeError(string typeName, string serviceName, string constantName)
        {
            _typeName = typeName;
            _serviceName = serviceName;
            _constantName = constantName;
        }

        public string ToText(ITextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["typeName"] = _typeName,
                ["serviceName"] = _serviceName,
                ["constantName"] = _constantName
            };
            return localizer.Expand("[ConstantTypeError]", param);
        }

        private readonly string _typeName;
        private readonly string _serviceName;
        private readonly string _constantName;
    }
}
