using System.Collections.Generic;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMapGame.TurnLog
{
    public class LogEntryStepGlobal : BaseLogEntry
    {
        public LogEntryStepGlobal(int turn)
        {
            _turn = turn;
        }

        public override string ToText(ITextLocalizer localizer)
        {
            Dictionary<string, object> param = new()
            {
                ["turn"] = _turn
            };
            return localizer.Expand("[StepGlobal]", param);
        }

        private readonly int _turn;
    }
}
