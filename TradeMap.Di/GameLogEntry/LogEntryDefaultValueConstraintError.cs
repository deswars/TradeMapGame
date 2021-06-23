using System.Collections.Generic;
using TradeMap.Di.Constraints;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMap.Di.GameLogEntry
{
    public class LogEntryDefaultValueConstraintError : BaseLogEntry
    {
        public LogEntryDefaultValueConstraintError(string typeName, string serviceName, string constantName, string defaultValue, IConstraint constraint)
        {
            _typeName = typeName;
            _serviceName = serviceName;
            _constantName = constantName;
            _defaultValue = defaultValue;
            _constraint = constraint;
        }

        public override string ToText(ITextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["typeName"] = _typeName,
                ["serviceName"] = _serviceName,
                ["constantName"] = _constantName,
                ["defaultValue"] = _defaultValue,
                ["constraint"] = _constraint
            };
            return localizer.Expand("[DefaultValueConstraintError]", param);
        }

        private readonly string _typeName;
        private readonly string _serviceName;
        private readonly string _constantName;
        private readonly string _defaultValue;
        private readonly IConstraint _constraint;
    }
}
