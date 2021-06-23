using System;
using System.Collections.Generic;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMap.Di.GameLogEntry
{
    public class LogEntryActionlessService : BaseLogEntry
    {
        public LogEntryActionlessService(string serviceName, Type type)
        {
            _serviceName = serviceName;
            _type = type;
        }

        public override string ToText(ITextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["serviceName"] = _serviceName,
                ["type"] = _type
            };
            return localizer.Expand("[ActionlessService]", param);
        }

        private readonly string _serviceName;
        private readonly Type _type;
    }
}
