using TradeMap.Localization;

namespace TradeMap.GameLog
{
    public interface ILogEntry
    {
        string ToText(ITextLocalizer localizer);
        InfoLevels InfoLevel { get; set; }
    }
}
