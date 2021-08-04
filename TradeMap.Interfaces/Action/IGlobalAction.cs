using TradeMap.Interfaces.Map.Mutable;

namespace TradeMap.Interfaces.Action
{
    public interface IGlobalAction : IMapAction
    {
        void Execute(IMapMutable map);
    }
}
