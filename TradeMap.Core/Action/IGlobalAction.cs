using TradeMap.Core.Map.Mutable;

namespace TradeMap.Core.Action
{
    public interface IGlobalAction : IMapAction
    {
        void Execute(IMapMutable map);
    }
}
