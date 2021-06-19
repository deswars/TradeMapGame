using TradeMap.Localization;

namespace TradeMap.GameLog
{
    public interface ILogEntry
    {
        public string ToText(ITextLocalizer localizer);
    }
}
