using System.Collections.Generic;
using TradeMap.Core;

namespace TradeMap.Interfaces.Map.ReadOnly
{
    public interface IMap
    {
        ICell this[int column, int row] { get; }
        ICell this[Point position] { get; }
        IReadOnlyList<ISettlement> Settlements { get; }
        IEnumerable<ICell> GetNeigborCells(Point position);
    }
}
