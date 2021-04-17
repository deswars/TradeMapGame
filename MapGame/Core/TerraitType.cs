using System.Collections.Generic;

namespace MapGame.Core
{
    public class TerraitType
    {
        public int Id { get; }
        public MoveClasses MoveClass { get; }
        public IReadOnlyList<Modifier> Modifiers
        {
            get
            {
                return _modifiers.AsReadOnly();
            }
        }
        public double MoveSpeed
        {
            get
            {
                double result = 0;
                if ((MoveClass & MoveClasses.GroundBlocked) != MoveClasses.FreeMovement)
                {
                    return double.PositiveInfinity;
                }

                foreach (var modifier in Modifiers)
                {
                    if (modifier.Type == ModifierTypes.Speed)
                    {
                        result += modifier.Value;
                    }
                }
                return result;
            }
        }

        public TerraitType(int id, MoveClasses moveClass, IEnumerable<Modifier> modifiers)
        {
            Id = id;
            MoveClass = moveClass;

            _modifiers = new List<Modifier>();
            foreach (var modifier in modifiers)
            {
                _modifiers.Add(modifier);
            }
        }

        private readonly List<Modifier> _modifiers;
    }
}
