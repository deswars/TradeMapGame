using MapGame.Core.EntityTypes;
using System.Collections.Generic;

namespace MapGame.Core
{
    public struct Route
    {
        public Point Start { get; }

        public Settlement StartSettlement { get; }

        public Point End { get; }

        public Settlement EndSettlement { get; }

        public IReadOnlyList<Point> Path { get; }


        public Route(Point start, Settlement startSettlement, Point end, Settlement endSettlement, IReadOnlyList<Point> path)
        {
            Start = start;
            StartSettlement = startSettlement;
            End = end;
            EndSettlement = endSettlement;
            Path = path;
        }
    }
}
