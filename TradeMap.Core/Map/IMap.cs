using System.Collections.Generic;

namespace TradeMap.Core.Map
{
    public interface IMap
    {
        public Cell this[int column, int row] { get; }
        IEnumerable<Cell> GetNeigborCells(int column, int row);
    }
}
