using MapGame.Core.EntityTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGame.Core
{
    public interface IPathFinder
    {
        IEnumerable<Point> GetPath(Point start, Point end);
        double GetLength(Point start, Point end);
        IEnumerable<Route> FindRoutes(Settlement source, double maxDistance);
    }
}
