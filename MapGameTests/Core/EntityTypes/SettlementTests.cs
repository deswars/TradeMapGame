using Xunit;
using MapGame.Core.EntityTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGame.Core.EntityTypes.Tests
{
    public class SettlementTests
    {
        [Fact()]
        public void SettlementTest()
        {
            Point position;
            position.X = 1;
            position.Y = 2;
            Modifier mod1 = new Modifier(ModifierTypes.MovementDifficulty, 1);
            Modifier mod2 = new Modifier(ModifierTypes.MovementDifficulty, 2);
            List<Modifier> modifiers = new List<Modifier>() { mod1, mod2 };
            Settlement settlement = new Settlement(position, MovementBlockType.None, modifiers);

            Assert.Equal(position, settlement.Position);
            Assert.Equal(MovementBlockType.None, settlement.MovementBlock);
            Assert.Equal(2, settlement.Modifiers.Count);
            modifiers.Remove(mod1);
            Assert.Equal(2, settlement.Modifiers.Count);
        }

        [Fact()]
        public void GetMovementDifficultyTest()
        {
            Point position;
            position.X = 1;
            position.Y = 2;
            Modifier mod1 = new Modifier(ModifierTypes.MovementDifficulty, 1);
            Modifier mod2 = new Modifier(ModifierTypes.MovementDifficulty, 2);
            List<Modifier> modifiers = new List<Modifier>() { mod1, mod2 };
            Settlement settlement = new Settlement(position, MovementBlockType.None, modifiers);

            Assert.Equal(3, settlement.GetMovementDifficulty());

            Settlement settlement2 = new Settlement(position, MovementBlockType.Ground, new List<Modifier>());
            Assert.True(double.IsInfinity(settlement2.GetMovementDifficulty()));
        }
    }
}