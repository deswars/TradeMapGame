using System.Collections.Generic;
using TradeMap.Core;
using TradeMap.Core.Map.Mutable;
using TradeMap.Core.Map.ReadOnly;

namespace TradeMap.Engine.Map
{
    public class SquareMap : IMapMutable
    {
        public int Width { get; }
        public int Height { get; }
        ICell IMap.this[Point position] { get => this[position]; }
        ICell IMap.this[int column, int row] { get => this[column, row]; }
        public IReadOnlyList<ISettlement> Settlements { get => SettlementsMutable; }

        public ICellMutable this[Point position] { get { return _map[position.X, position.Y]; } }
        public ICellMutable this[int column, int row] { get { return _map[column, row]; } }
        public IReadOnlyList<ISettlementMutable> SettlementsMutable { get { return _settlements; } }


        private readonly Cell[,] _map;
        private readonly List<ISettlementMutable> _settlements = new();


        public SquareMap(int width, int height, ITerrainType defaultTerrain)
        {
            Width = width;
            Height = height;
            _map = new Cell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    _map[i, k] = new Cell(defaultTerrain, new List<TerrainFeautre>(), new Point(i, k));
                }
            }
        }


        public bool AddSettlement(ISettlementMutable settlement)
        {
            if (_map[settlement.Position.X, settlement.Position.Y].Settlement == null)
            {
                _map[settlement.Position.X, settlement.Position.Y].SettlementMut = settlement;
                _settlements.Add(settlement);
                return true;
            }
            return false;
        }

        public bool RemoveSettlement(ISettlementMutable settlement)
        {
            if (_settlements.Contains(settlement))
            {
                _map[settlement.Position.X, settlement.Position.Y].SettlementMut = null;
                _settlements.Remove(settlement);
                return true;
            }
            return false;
        }

        public IReadOnlyList<ICell> GetNeigborCells(Point position, double distance)
        {
            double square = distance * distance;
            int x = position.X;
            int y = position.Y;
            List<ICell> result = new();
            if (distance < 0)
            {
                return result;
            }
            result.Add(_map[x, y]);

            //left
            x -= 1;
            int dx = x - position.X;
            while ((x >= 0) && (dx * dx <= square))
            {
                x -= 1;
                dx = x - position.X;
            }

            //quadrant left-up
            x += 1;
            y -= 1;
            int dy = y - position.Y;
            while ((y >= 0) && (dy * dy <= square))
            {
                while (DistanceSquared(x, y, position.X, position.Y) > square)
                {
                    x += 1;
                }
                for (int i = x; i <= position.X; i++)
                {
                    result.Add(_map[i, y]);
                }
                y -= 1;
                dy = y - position.Y;
            }

            //quadrant right-up
            x = position.X + 1;
            y += 1;
            dx = x - position.X;
            while ((x < Width) && (dx * dx <= square))
            {
                while (DistanceSquared(x, y, position.X, position.Y) > square)
                {
                    y += 1;
                }
                for (int i = y; i <= position.Y; i++)
                {
                    result.Add(_map[x, i]);
                }
                x += 1;
                dx = x - position.X;
            }

            //quadrant right-down
            x -= 1;
            y = position.Y + 1;
            dy = y - position.Y;
            while ((y < Height) && (dy * dy <= square))
            {
                while (DistanceSquared(x, y, position.X, position.Y) > square)
                {
                    x -= 1;
                }
                for (int i = position.X; i <= x; i++)
                {
                    result.Add(_map[i, y]);
                }
                y += 1;
                dy = y - position.Y;
            }

            //quadrant left-down
            x = position.X - 1;
            y -= 1;
            dx = x - position.X;
            while ((x >= 0) && (dx * dx <= square))
            {
                while (DistanceSquared(x, y, position.X, position.Y) > square)
                {
                    y -= 1;
                }
                for (int i = position.Y; i <= y; i++)
                {
                    result.Add(_map[x, i]);
                }
                x -= 1;
                dx = x - position.X;
            }

            return result;
        }

        public IReadOnlyList<ISettlement> GetNeigborSettlements(Point position, double distance)
        {
            List<ISettlement> result = new();
            double square = distance * distance;
            foreach (var set in _settlements)
            {
                if (DistanceSquared(set.Position, position) <= square)
                {
                    result.Add(set);
                }
            }
            return result;
        }

        private static double DistanceSquared(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }

        private static double DistanceSquared(int x1, int y1, int x2, int y2)
        {
            int dx = x1 - x2;
            int dy = y1 - y2;
            return dx * dx + dy * dy;
        }
    }
}
