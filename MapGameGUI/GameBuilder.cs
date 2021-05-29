using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using TradeMapGame.Map;
using TradeMapGame.TurnLog;
using Point = TradeMapGame.Map.Point;

namespace TradeMapGame
{
    public static class GameBuilder
    {
        public static Engine BuildMap(string confFile, string bmpMap, string bmpFeautres, Configuration.ConfigurationLoader conf)
        {
            string mapConf = File.ReadAllText(confFile);
            var mapConfJson = JObject.Parse(mapConf);

            Dictionary<Color, string> terrainColors = new();
            foreach (var terrainJson in mapConfJson["MapColors"])
            {
                string terrain = terrainJson.Value<string>("Terrain");
                Color color = Color.FromArgb(terrainJson.Value<int>("Color") | -16777216);
                terrainColors.Add(color, terrain);
            }

            Dictionary<Color, string> feautresColors = new();
            foreach (var feautreJson in mapConfJson["MapFeautreColors"])
            {
                string feautre = feautreJson.Value<string>("Feautre");
                Color color = Color.FromArgb(feautreJson.Value<int>("Color") | -16777216);
                feautresColors.Add(color, feautre);
            }


            Bitmap mapBmp = new(bmpMap);
            Bitmap feautreBmp = new(bmpFeautres);
            int Width = mapBmp.Width;
            int Height = mapBmp.Height;
            SquareDiagonalMap map = new(Width, Height, conf.Const.DefaultTerrain);
            for (int i = 0; i < Width; i++)
            {
                for (int k = 0; k < Height; k++)
                {
                    var color = mapBmp.GetPixel(i, k);
                    string terrainId = terrainColors[color];
                    map[i, k].Terrain = conf.Lists.TerrainTypes[terrainId];

                    var feautreColor = feautreBmp.GetPixel(i, k);
                    string feautreId = feautresColors[feautreColor];
                    if (feautreId != "")
                    {
                        map[i, k].MapFeautres.Add(conf.Lists.MapFeautreTypes[feautreId]);
                    }
                }
            }

            Engine engine = EngineBuilder.Build(map, conf, new TurnLogImpl());
            foreach (var settlementJson in mapConfJson["Settlemets"])
            {
                int x = settlementJson["Position"].Value<int>("X");
                int y = settlementJson["Position"].Value<int>("Y");
                Point position = new(x, y);
                int population = settlementJson.Value<int>("Population");
                KeyedVector<ResourceType> resources = new(conf.Lists.ResourceTypes.Values);
                Settlement settlment = new(position.X + ":" + position.Y, position, population, resources);
                engine.Settlements.Add(settlment);
            }

            return engine;
        }
    }
}
