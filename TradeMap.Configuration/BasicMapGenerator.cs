using System;
using System.Collections.Generic;
using System.Linq;
using TradeMap.Core.Map;

namespace TradeMap.Configuration
{
    public static class BasicMapGenerator
    {
        public static SquareDiagonalMap Generate(int width, int height, TypeRepository types)
        {
            List<string> terraindIdList = types.TerrainTypes.Keys.ToList();
            List<string> feautreIdList = types.MapFeautreTypes.Keys.ToList();
            Random rnd = new Random();
            var defaultTerrain = types.TerrainTypes[terraindIdList[0]];
            SquareDiagonalMap map = new(width, height, defaultTerrain);

            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    var randomTerrainId = terraindIdList[rnd.Next(terraindIdList.Count)];
                    var randomTerrain = types.TerrainTypes[randomTerrainId];
                    map[i,k].Terrain = randomTerrain;
                    if (rnd.NextDouble() > 0.8)
                    {
                        var randomFeautreId = feautreIdList[rnd.Next(feautreIdList.Count)];
                        var randomFeautre = types.MapFeautreTypes[randomFeautreId];
                        map[i, k].MapFeautres.Add(randomFeautre);
                    }
                }
            }
            return map;
        }
    }
}
