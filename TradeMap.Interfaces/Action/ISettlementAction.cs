using TradeMap.Interfaces.Map.Mutable;

namespace TradeMap.Interfaces.Action
{
    public interface ISettlementAction : IMapAction
    {
        void Execute(ISettlementMutable settlement);
    }
}
