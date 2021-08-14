using TradeMap.Core.Map.Mutable;

namespace TradeMap.Core.Action
{
    public interface ISettlementAction : IMapAction
    {
        void Execute(ISettlementMutable settlement);
    }
}
