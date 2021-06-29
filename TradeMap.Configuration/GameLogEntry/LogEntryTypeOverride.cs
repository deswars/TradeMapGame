using System.Collections.Generic;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMap.Configuration.GameLogEntry
{
    public class LogEntryTypeOverride : BaseLogEntry
    {
        public LogEntryTypeOverride(string id)
        {
            _id = id;
        }

        public override string ToText(ITextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["id"] = _id
            };
            return localizer.Expand("[TypeOverride]", param);
        }

        private readonly string _id;
    }
}
