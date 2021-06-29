using System.Collections.Generic;

namespace TradeMap.Core.Map
{
    public interface IMap
    {
        ICell this[int column, int row] { get; }
        ICell this[Point position] { get; }
        IEnumerable<Settlement> Settlements { get; }
        IEnumerable<ICell> GetNeigborCells(Point position);
        bool AddSettlement(Settlement settlement);
        bool RemoveSettlement(Settlement settlement);
    }
}
