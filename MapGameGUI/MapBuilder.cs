using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapGame.SquareMap;
using MapGame.Core;
using MapGame.Core.EntityTypes;

namespace MapGame.GUI
{
    class MapBuilder
    {
        public Map NewMap;
        public int Width = 10;
        public int Height = 10;

        public MapBuilder()
        {
            Modifier MovementDifficulty1 = new Modifier(ModifierTypes.MovementDifficulty, 1);
            Modifier MovementDifficulty2 = new Modifier(ModifierTypes.MovementDifficulty, 1);

            List<Modifier> Modifiers1 = new() { MovementDifficulty1 };
            TerrainType terrain1 = new TerrainType(0, MovementBlockType.None, Modifiers1);

            List<Modifier> Modifiers2 = new() { MovementDifficulty2 };
            TerrainType terrain2 = new TerrainType(1, MovementBlockType.None, Modifiers2);

            TerrainType terrain3 = new TerrainType(2, MovementBlockType.Ground, Modifiers1);

            NewMap = new Map(Width, Height, terrain1);
            for (int i = 5; i < 7; i++)
            {
                for (int k = 3; k < 5; k++)
                {
                    NewMap[i, k].ChangeTerrain(terrain2);
                }
            }
            for (int i = 1; i < 3; i++)
            {
                for (int k = 1; k < 3; k++)
                {
                    NewMap[i, k].ChangeTerrain(terrain2);
                }
            }
            for (int i = 3; i < 6; i++)
            {
                for (int k = 1; k < 4; k++)
                {
                    NewMap[i, k].ChangeTerrain(terrain3);
                }
            }

            Point position1 = new Point();
            position1.X = 1;
            position1.Y = 1;
            Settlement settlement1 = new Settlement(position1, MovementBlockType.None, new List<Modifier>());

            Point position2 = new Point();
            position2.X = 4;
            position2.Y = 4;
            Settlement settlement2 = new Settlement(position2, MovementBlockType.None, new List<Modifier>());

            Point position3 = new Point();
            position3.X = 0;
            position3.Y = 6;
            Settlement settlement3 = new Settlement(position3, MovementBlockType.None, new List<Modifier>());

            NewMap.AddSettlement(settlement1);
            NewMap.AddSettlement(settlement2);
            NewMap.AddSettlement(settlement3);
        }
    }
}
