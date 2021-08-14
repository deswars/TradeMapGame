using System.Collections.Generic;

namespace TradeMap.Core.Map.ReadOnly
{
    public interface IMap
    {
        int Width { get; }
        int Height { get; }
        ICell this[int column, int row] { get; }
        ICell this[Point position] { get; }
        IReadOnlyList<ISettlement> Settlements { get; }
        IReadOnlyList<ICell> GetNeigborCells(Point position, double distance);
        IReadOnlyList<ISettlement> GetNeigborSettlements(Point position, double distance);
    }
}
