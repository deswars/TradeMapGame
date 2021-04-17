using MapGame.Core;
using MapGame.Core.EntityTypes;
using System.Collections.Generic;
using System.Linq;

namespace MapGame.SquareMap
{
    public class Map : IMap
    {
        public int Width { get; }

        public int Height { get; }

        public IReadOnlyList<Settlement> Settlements
        {
            get
            {
                return _settlements.AsReadOnly();
            }
        }

        public Cell this[int column, int row]
        {
            get { return _map[column, row]; }
        }


        public Map(int width, int height, TerrainType defaultTerrain)
        {
            Width = width;
            Height = height;
            _map = new Cell[width, height];
            for (int i = 0; i < height; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    _map[i, k] = new Cell(defaultTerrain);
                }
            }
        }

        public IEnumerable<Cell> GetNeigborCells(int column, int row)
        {
            var neighborList = new List<Cell>();
            if (column > 0)
            {
                neighborList.Add(_map[column - 1, row]);
            }
            if (column < Width - 1)
            {
                neighborList.Add(_map[column + 1, row]);
            }
            if (row > 0)
            {
                neighborList.Add(_map[column, row - 1]);
            }
            if (row < Height - 1)
            {
                neighborList.Add(_map[column, row + 1]);
            }
            return neighborList;
        }

        public bool AddSettlement(Settlement settlement)
        {
            var position = settlement.Position;
            if (_settlements.Any(x => x.Position == position))
            {
                return false;
            }

            _map[position.X, position.Y].AddEntity(settlement);
            _settlements.Add(settlement);
            return true;
        }

        public void RemoveSettlement(Settlement settlement)
        {
            var position = settlement.Position;
            _map[position.X, position.Y].RemoveEntity(settlement);
            _settlements.Remove(settlement);
        }


        private readonly Cell[,] _map;

        private readonly List<Settlement> _settlements = new();
    }
}
