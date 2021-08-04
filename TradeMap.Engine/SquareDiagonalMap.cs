using System.Collections.Generic;
using System.Linq;

namespace TradeMap.Core.Map
{
    public class SquareDiagonalMap : IMap
    {
        public int Width { get; }
        public int Height { get; }

        public ICell this[int column, int row]
        {
            get { return _map[column, row]; }
        }

        public ICell this[Point position]
        {
            get { return _map[position.X, position.Y]; }
        }

        public IEnumerable<Settlement> Settlements {
            get { return _settlements; }
        }


        public SquareDiagonalMap(int width, int height, TerrainType defaultTerrain)
        {
            Width = width;
            Height = height;
            _map = new Cell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    _map[i, k] = new Cell(defaultTerrain, new Point(i, k));
                }
            }
        }

        public IEnumerable<ICell> GetNeigborCells(Point position)
        {
            int column = position.X;
            int row = position.Y;
            var neighborList = new List<ICell>();
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
            if (column > 0 && row > 0)
            {
                neighborList.Add(_map[column - 1, row - 1]);
            }
            if (column > 0 && row < Height - 1)
            {
                neighborList.Add(_map[column - 1, row + 1]);
            }
            if (column < Width - 1 && row > 0)
            {
                neighborList.Add(_map[column + 1, row - 1]);
            }
            if (column < Width - 1 && row < Height - 1)
            {
                neighborList.Add(_map[column + 1, row + 1]);
            }
            return neighborList;
        }

        public bool AddSettlement(Settlement settlement)
        {
            if (_map[settlement.Position.X, settlement.Position.Y].Settlement == null)
            {
                _map[settlement.Position.X, settlement.Position.Y].Settlement = settlement;
                _settlements.Add(settlement);
                return true;
            }
            return false;
        }

        public bool RemoveSettlement(Settlement settlement)
        {
            if (_settlements.Contains(settlement))
            {
                _map[settlement.Position.X, settlement.Position.Y].Settlement = null;
                _settlements.Remove(settlement);
                return true;
            }
            return false;
        }

        private readonly Cell[,] _map;
        private readonly List<Settlement> _settlements = new();
    }

}
