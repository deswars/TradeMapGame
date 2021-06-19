using System;
using System.Collections.Generic;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMap.Di.GameLog
{
    public class LogEntryServiceOverride : ILogEntry
    {
        public LogEntryServiceOverride(string serviceName, Type oldType, Type newType)
        {
            _serviceName = serviceName;
            _newType = newType;
            _oldType = oldType;
        }

        public string ToText(ITextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["serviceName"] = _serviceName,
                ["newType"] = _newType,
                ["oldType"] = _oldType
            };
            return localizer.Expand("[ServiceOverride]", param);
        }

        private readonly string _serviceName;
        private readonly Type _newType;
        private readonly Type _oldType;
    }
}
