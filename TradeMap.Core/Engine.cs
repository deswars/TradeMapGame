using System.Collections.Generic;
using TradeMap.Core.Map;
using TradeMap.GameLog;
using TradeMapGame.TurnLog;

namespace TradeMap.Core
{
    public class Engine
    {
        public delegate void SettlementAction(int turn, Settlement settlement);
        public delegate void GlobalAction(int turn, IMap map);

        public event SettlementAction OnPreTurn;
        public event GlobalAction OnTurn;
        public event SettlementAction OnPostTurn;

        public SquareDiagonalMap Map { get; }
        public int Turn { get; private set; }
        public IGameLog Log { get; }

        public Engine(SquareDiagonalMap map, IGameLog log)
        {
            Map = map;

            Turn = 0;
            Log = log;
            OnPreTurn = PreTurnLog;
            OnPostTurn = PostTurnLog;
            OnTurn = TurnLog;
        }

        public void NextTurn()
        {
            foreach (var sett in Map.Settlements)
            {
                OnPreTurn(Turn, sett);
            }
            OnTurn(Turn, Map);
            foreach (var sett in Map.Settlements)
            {
                OnPostTurn(Turn, sett);
            }
            Turn++;
        }

        private void PreTurnLog(int turn, Settlement settlement)
        {
            Log.AddEntry(InfoLevels.Info, () => new LogEntryStepPreTurnSettlement(turn, settlement));
        }

        private void PostTurnLog(int turn, Settlement settlement)
        {
            Log.AddEntry(InfoLevels.Info, () => new LogEntryStepPostTurnSettlement(turn, settlement));
        }

        private void TurnLog(int turn, IMap map)
        {
            Log.AddEntry(InfoLevels.Info, () => new LogEntryStepGlobal(turn));
        }
    }
}
