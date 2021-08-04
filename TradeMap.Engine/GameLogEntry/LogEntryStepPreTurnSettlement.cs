using System.Collections.Generic;
using TradeMap.Core.Map;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMapGame.TurnLog
{
    public class LogEntryStepPreTurnSettlement : BaseLogEntry
    {
        public LogEntryStepPreTurnSettlement(int turn, Settlement settlement)
        {
            _turn = turn;
            _settlement = settlement;
        }

        public override string ToText(ITextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["turn"] = _turn,
                ["settlement"] = _settlement
            };
            return localizer.Expand("[StepPreTurnSettlement]", param);
        }

        private readonly Settlement _settlement;
        private readonly int _turn;
    }
}
