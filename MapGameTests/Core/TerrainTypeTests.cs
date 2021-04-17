using Xunit;
using MapGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGame.Core.Tests
{
    public class TerrainTypeTests
    {
        [Fact()]
        public void TerrainTypeTest()
        {
            int Id = 1;
            Modifier mod1 = new Modifier(ModifierTypes.MovementDifficulty, 1);
            Modifier mod2 = new Modifier(ModifierTypes.MovementDifficulty, 2);
            List<Modifier> modifiers = new List<Modifier>() { mod1, mod2 };
            TerrainType terrain = new TerrainType(Id, MovementBlockType.Ground, modifiers);

            Assert.Equal(Id, terrain.Id);
            Assert.Equal(MovementBlockType.Ground, terrain.MovementBlock);
            Assert.Equal(2, terrain.Modifiers.Count);
            modifiers.Remove(mod1);
            Assert.Equal(2, terrain.Modifiers.Count);
        }

        [Fact()]
        public void GetMovementDifficultyTest()
        {
            int Id = 1;
            Modifier mod1 = new Modifier(ModifierTypes.MovementDifficulty, 1);
            Modifier mod2 = new Modifier(ModifierTypes.MovementDifficulty, 2);
            List<Modifier> modifiers = new List<Modifier>() { mod1, mod2 };
            TerrainType terrain = new TerrainType(Id, MovementBlockType.None, modifiers);

            Assert.Equal(3, terrain.GetMovementDifficulty());

            TerrainType terrain2 = new TerrainType(Id, MovementBlockType.Ground, new List<Modifier>());
            Assert.True(double.IsInfinity(terrain2.GetMovementDifficulty()));
        }
    }
}