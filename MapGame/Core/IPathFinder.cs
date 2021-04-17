using System.Collections.Generic;

namespace MapGame.Core
{
    public interface IPathFinder
    {
        IEnumerable<Point> GetPath(Point start, Point end);

        double GetLength(Point start, Point end);
    }
}
