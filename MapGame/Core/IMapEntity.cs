using System.Collections.Generic;

namespace MapGame.Core
{
    public interface IMapEntity
    {
        MovementBlockType MovementBlock { get; }

        IReadOnlyList<Modifier> Modifiers { get; }

        double GetMovementDifficulty();
    }
}
