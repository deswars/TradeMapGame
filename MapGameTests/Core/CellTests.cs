using Xunit;
using MapGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapGame.Core.EntityTypes;

namespace MapGame.Core.Tests
{
    public class CellTests
    {
        [Fact()]
        public void CellTest()
        {
            int Id = 1;
            Modifier mod1 = new Modifier(ModifierTypes.MovementDifficulty, 1);
            Modifier mod2 = new Modifier(ModifierTypes.MovementDifficulty, 2);
            List<Modifier> modifiers = new List<Modifier>() { mod1, mod2 };
            TerrainType terrain = new TerrainType(Id, MovementBlockType.None, modifiers);
            Cell cell = new Cell(terrain);

            Assert.Equal(MovementBlockType.None, cell.MovementBlock);
            Assert.Equal(3, cell.MovementDifficulty);
            Assert.Equal(terrain, cell.Terrain);
            Assert.Empty(cell.Entities);
        }

        [Fact()]
        public void ChangeTerrainTest()
        {
            int Id1 = 1;
            Modifier mod1 = new Modifier(ModifierTypes.MovementDifficulty, 1);
            Modifier mod2 = new Modifier(ModifierTypes.MovementDifficulty, 2);
            List<Modifier> modifiers1 = new List<Modifier>() { mod1, mod2 };
            TerrainType terrain1 = new TerrainType(Id1, MovementBlockType.None, modifiers1);

            int Id2 = 2;
            Modifier mod3 = new Modifier(ModifierTypes.MovementDifficulty, 5);
            Modifier mod4 = new Modifier(ModifierTypes.MovementDifficulty, 2);
            List<Modifier> modifiers2 = new List<Modifier>() { mod3, mod4 };
            TerrainType terrain2 = new TerrainType(Id2, MovementBlockType.Ground, modifiers2);
            TerrainType terrain3 = new TerrainType(Id2, MovementBlockType.None, modifiers2);

            Cell cell = new Cell(terrain1);
            cell.ChangeTerrain(terrain2);

            Assert.Equal(MovementBlockType.Ground, cell.MovementBlock);
            Assert.True(double.IsInfinity(cell.MovementDifficulty));
            Assert.Equal(terrain2, cell.Terrain);
            Assert.Empty(cell.Entities);

            cell.ChangeTerrain(terrain3);
            Assert.Equal(MovementBlockType.None, cell.MovementBlock);
            Assert.Equal(7, cell.MovementDifficulty);
            Assert.Equal(terrain3, cell.Terrain);
            Assert.Empty(cell.Entities);
        }

        [Fact()]
        public void AddEntityTest()
        {
            int Id = 1;
            Modifier mod1 = new Modifier(ModifierTypes.MovementDifficulty, 1);
            Modifier mod2 = new Modifier(ModifierTypes.MovementDifficulty, 2);
            List<Modifier> modifiers = new List<Modifier>() { mod1, mod2 };
            TerrainType terrain = new TerrainType(Id, MovementBlockType.None, modifiers);
            Cell cell = new Cell(terrain);

            Point position1;
            position1.X = 1;
            position1.Y = 2;
            List<Modifier> modifiers1 = new List<Modifier>() { mod1, mod2 };
            Settlement settlement1 = new Settlement(position1, MovementBlockType.None, modifiers1);
            cell.AddEntity(settlement1);

            Assert.Equal(MovementBlockType.None, cell.MovementBlock);
            Assert.Equal(6, cell.MovementDifficulty);
            Assert.Equal(1, cell.Entities.Count);

            Point position2;
            position2.X = 3;
            position2.Y = 4;
            Modifier mod3 = new Modifier(ModifierTypes.MovementDifficulty, 3);
            Modifier mod4 = new Modifier(ModifierTypes.MovementDifficulty, 4);
            List<Modifier> modifiers2 = new List<Modifier>() { mod3, mod4 };
            Settlement settlement2 = new Settlement(position2, MovementBlockType.Ground, modifiers2);
            cell.AddEntity(settlement2);

            Assert.Equal(MovementBlockType.Ground, cell.MovementBlock);
            Assert.True(double.IsInfinity(cell.MovementDifficulty));
            Assert.Equal(2, cell.Entities.Count);
        }

        [Fact()]
        public void RemoveEntityTest()
        {
            int Id = 1;
            Modifier mod1 = new Modifier(ModifierTypes.MovementDifficulty, 1);
            Modifier mod2 = new Modifier(ModifierTypes.MovementDifficulty, 2);
            List<Modifier> modifiers = new List<Modifier>() { mod1, mod2 };
            TerrainType terrain = new TerrainType(Id, MovementBlockType.None, modifiers);
            Cell cell = new Cell(terrain);

            Point position1;
            position1.X = 1;
            position1.Y = 2;
            List<Modifier> modifiers1 = new List<Modifier>() { mod1, mod2 };
            Settlement settlement1 = new Settlement(position1, MovementBlockType.Ground, modifiers1);
            cell.AddEntity(settlement1);

            Point position2;
            position2.X = 3;
            position2.Y = 4;
            Modifier mod3 = new Modifier(ModifierTypes.MovementDifficulty, 3);
            Modifier mod4 = new Modifier(ModifierTypes.MovementDifficulty, 4);
            List<Modifier> modifiers2 = new List<Modifier>() { mod3, mod4 };
            Settlement settlement2 = new Settlement(position2, MovementBlockType.None, modifiers2);
            cell.AddEntity(settlement2);
            cell.RemoveEntity(settlement1);

            Assert.Equal(MovementBlockType.None, cell.MovementBlock);
            Assert.Equal(10, cell.MovementDifficulty);
            Assert.Equal(1, cell.Entities.Count);
        }
    }
}