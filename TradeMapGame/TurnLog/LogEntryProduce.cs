using System.Collections.Generic;
using TradeMapGame.Localization;
using TradeMapGame.Map;

namespace TradeMapGame.TurnLog
{
    class LogEntryProduce : ILogEntry
    {
        public LogEntryProduce(int turn, Settlement settlement, KeyedVector<ResourceType> delta)
        {
            _turn = turn;
            _settlement = settlement;
            _delta = delta;
        }

        public string ToText(TextLocalizer localizer)
        {
            string resources = "";
            foreach (var res in _delta)
            {
                resources += "[" + res.Key + "] " + res.Value;
            }

            Dictionary<string, object> param = new()
            {
                ["turn"] = _turn,
                ["settlement"] = _settlement,
                ["delta"] = _delta
            };
            return localizer.Expand("[Produce]", param);
        }

        private readonly int _turn;
        private readonly Settlement _settlement;
        private readonly KeyedVector<ResourceType> _delta;
    }
}
