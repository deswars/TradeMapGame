using System.Collections.Generic;
using TradeMapGame.Localization;

namespace TradeMapGame.TurnLog
{
    class LogEntryTurn : ILogEntry
    {
        public LogEntryTurn(int turn)
        {
            _turn = turn;
        }

        public string ToText(TextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["turn"] = _turn
            };
            return localizer.Expand("[Turn]", param);
        }

        private readonly int _turn;

    }
}
