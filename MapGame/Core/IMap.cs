using MapGame.Core.EntityTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGame.Core
{
    public interface IMap
    {
        Cell this[int column, int row] { get; }
        IReadOnlyList<Settlement> Settlements { get; }
        IEnumerable<Cell> GetNeigborCells(int column, int row);
        bool AddSettlement(Settlement newSettlement);
        void RemoveSettlement(Settlement oldSettlement);
    }
}
