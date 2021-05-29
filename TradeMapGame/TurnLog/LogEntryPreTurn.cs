using System.Collections.Generic;
using TradeMapGame.Localization;
using TradeMapGame.Map;

namespace TradeMapGame.TurnLog
{
    public class LogEntryPreTurn : ILogEntry
    {
        public LogEntryPreTurn(int turn, Settlement settlement)
        {
            _turn = turn;
            _settlement = settlement;
        }

        public string ToText(TextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["turn"] = _turn.ToString(),
                ["settlement"] = _settlement
            };
            return localizer.Expand("[PreTurn]", param);
        }

        private readonly Settlement _settlement;
        private readonly int _turn;
    }
}
