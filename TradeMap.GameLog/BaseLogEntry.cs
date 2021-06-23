using System.Collections.Generic;
using TradeMap.Localization;

namespace TradeMap.GameLog
{
    public class BaseLogEntry : ILogEntry
    {
        public InfoLevels InfoLevel { get; set; }

        public virtual string ToText(ITextLocalizer localizer)
        {
            return localizer.Expand("[LogEntry]", new Dictionary<string, object>());
        }
    }
}
