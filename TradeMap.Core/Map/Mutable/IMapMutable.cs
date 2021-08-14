using System.Collections.Generic;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Core.Map.Mutable
{
    public interface IMapMutable : IMap
    {
        new ICellMutable this[int column, int row] { get; }
        new ICellMutable this[Point position] { get; }
        IReadOnlyList<ISettlementMutable> SettlementsMutable { get; }
        bool AddSettlement(ISettlementMutable settlement);
        bool RemoveSettlement(ISettlementMutable settlement);
    }
}
