using TradeMap.Interfaces.Action;
using TradeMap.Interfaces.Map.ReadOnly;

namespace TradeMap.Interfaces
{
    public delegate void StartUpAction(IMap map);

    public delegate ISettlementAction SettlementAction(int turn, ISettlement settlement);
    public delegate IGlobalAction GlobalAction(int turn, IMap map);
}
