using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGame.Core.EntityTypes
{
    public class Settlement : IMapEntity
    {
        public MoveClasses MoveClass { get; } = MoveClasses.FreeMovement;

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

        public Point Position { get; }

        public Settlement(Point position, IEnumerable<Modifier> modifiers)
        {
            Position = position;

            modifiers = new List<Modifier>();
            foreach (var modifier in modifiers)
            {
                _modifiers.Add(modifier);
            }
        }

        private readonly List<Modifier> _modifiers;
    }
}
