using System.Collections.Generic;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMap.Configuration.GameLogEntry
{
    public class LogEntryIncorrectFileFormat : BaseLogEntry
    {
        public LogEntryIncorrectFileFormat(string file)
        {
            _file = file;
        }

        public override string ToText(ITextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["file"] = _file
            };
            return localizer.Expand("[IncorrectFileFormat]", param);
        }

        private readonly string _file;
    }
}
