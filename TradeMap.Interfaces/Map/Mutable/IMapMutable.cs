using System.Collections.Generic;
using TradeMap.Core;
using TradeMap.Interfaces.Map.ReadOnly;

namespace TradeMap.Interfaces.Map.Mutable
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
