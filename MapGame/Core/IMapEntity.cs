using System.Collections.Generic;

namespace MapGame.Core
{
    public interface IMapEntity
    {
        MoveClasses MoveClass { get; }
        IReadOnlyList<Modifier> Modifiers { get; }
        double MoveSpeed { get; }
    }
}
