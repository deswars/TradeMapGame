using MapGame.Core;
using MapGame.Core.EntityTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGame.SquareMap
{
    public class LinePathFinder : IPathFinder
    {
        public LinePathFinder(Map map)
        {
            _map = map;
        }

        public IEnumerable<Point> GetPath(Point start, Point end)
        {
            var path = FindPath(start, end);
            if (double.IsInfinity(GetPathLength(path)))
            {
                return null;
            }

            return path.ToList();
        }

        public double GetLength(Point start, Point end)
        {
            var path = FindPath(start, end);
            return GetPathLength(path);
        }

        public IEnumerable<Route> FindRoutes(Settlement selected, double maxDistance)
        {
            foreach(var destination in _map.Settlements)
            {

            }
        }

        private readonly Map _map;

        private static Point[] FindPath(Point start, Point end)
        {
            int pathLength = Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y) + 1;
            Point[] result = new Point[pathLength];
            result[0] = start;

            int step = 1;
            Point current;
            current.X = start.X;
            current.Y = start.Y;

            int dx = 1;
            if (start.X > end.X)
            {
                dx = -1;
            }
            int dy = 1;
            if (start.Y > end.Y)
            {
                dy = -1;
            }

            //strait line between start and end: A*x + B*y + C == 0
            int A = start.Y - end.Y;
            int B = end.X - start.X;
            int C = start.X * end.Y - end.X * start.Y;
            while ((current.X != end.X) && (current.Y != end.Y))
            {
                int CurrentDx = A * (current.X + dx) + B * current.Y + C;
                int CurrentDy = A * current.X + B * (current.Y + dy) + C;
                if (CurrentDx <= CurrentDy)
                {
                    current.X += dx;
                }
                else
                {
                    current.Y += dy;
                }
                result[step] = current;
                step++;
            }
            return result;
        }

        private double GetPathLength(Point[] path)
        {
            double length = 0;
            for (int i = 1; i < path.Length; i++)
            {
                var cell = _map[path[i].X, path[i].Y];
                length += cell.MoveModifier;
            }
            return length;
        }
    }
}
