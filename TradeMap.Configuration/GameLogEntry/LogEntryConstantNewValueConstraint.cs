using System.Collections.Generic;
using TradeMap.Di.Constraints;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMap.Configuration.GameLogEntry
{
    public class LogEntryConstantNewValueConstraint : BaseLogEntry
    {
        public LogEntryConstantNewValueConstraint(string file, string service, string constant, string newValue, IConstraint constraint)
        {
            _file = file;
            _service = service;
            _constant = constant;
            _newValue = newValue;
            _constraint = constraint;
        }

        public override string ToText(ITextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["file"] = _file,
                ["service"] = _service,
                ["constant"] = _constant,
                ["newValue"] = _newValue,
                ["constraint"] = _constraint
            };
            return localizer.Expand("[ConstantNewValueConstraint]", param);
        }

        private readonly string _file;
        private readonly string _service;
        private readonly string _constant;
        private readonly string _newValue;
        private readonly IConstraint _constraint;
    }
}
