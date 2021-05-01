using System.Collections.Generic;

namespace TradeMapGame
{
    public class Map
    {
        public int Width { get; }
        public int Height { get; }

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
            if (column > 0 && row > 0)
            {
                neighborList.Add(_map[column - 1, row - 1]);
            }
            if (column > 0 && (row < Height - 1))
            {
                neighborList.Add(_map[column - 1, row + 1]);
            }
            if ((column < Width - 1) && row > 0)
            {
                neighborList.Add(_map[column + 1, row - 1]);
            }
            if ((column < Width - 1) && (row < Height - 1))
            {
                neighborList.Add(_map[column + 1, row + 1]);
            }
            return neighborList;
        }

        private readonly Cell[,] _map;
    }

}
