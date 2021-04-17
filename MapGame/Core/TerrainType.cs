using System.Collections.Generic;

namespace MapGame.Core
{
    public class TerrainType
    {
        public int Id { get; }

        public MovementBlockType MovementBlock { get; }

        public IReadOnlyList<Modifier> Modifiers
        {
            get
            {
                return _modifiers.AsReadOnly();
            }
        }


        public TerrainType(int id, MovementBlockType movementBlock, IEnumerable<Modifier> modifiers)
        {
            Id = id;
            MovementBlock = movementBlock;

            _modifiers = new List<Modifier>();
            foreach (var modifier in modifiers)
            {
                _modifiers.Add(modifier);
            }
        }


        public double GetMovementDifficulty()
        {
            double result = 0;
            if (MovementBlock != MovementBlockType.None)
            {
                return double.PositiveInfinity;
            }

            foreach (var modifier in Modifiers)
            {
                if (modifier.Type == ModifierTypes.MovementDifficulty)
                {
                    result += modifier.Value;
                }
            }
            return result;
        }


        private readonly List<Modifier> _modifiers;
    }
}
