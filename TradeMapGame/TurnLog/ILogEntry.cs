using TradeMapGame.Localization;

namespace TradeMapGame.TurnLog
{
    public interface ILogEntry
    {
        public string ToText(TextLocalizer localizer);
    }
}
