using TradeMap.Core.Action;
using TradeMap.Core.Map.Mutable;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Core
{
    public delegate void StartUpSubstep(IMapMutable map);

    public delegate ISettlementAction? SettlementSubstep(int turn, ISettlement settlement);
    public delegate IGlobalAction? GlobalSubstep(int turn, IMap map);
}
