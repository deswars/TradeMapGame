using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TradeMapGame
{
    public static class GameBuilder
    {
        public static Engine BuildMap(string confFile, string bmpFile, Configuration conf)
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

            Bitmap mapBmp = new Bitmap(bmpFile);
            int Width = mapBmp.Width;
            int Height = mapBmp.Height;
            Map map = new Map(Width, Height, conf.TerrainTypes[conf.DefaultTerrain]);
            for (int i = 0; i < Width; i++)
            {
                for (int k = 0; k < Height; k++)
                {
                    var color = mapBmp.GetPixel(i, k);
                    string terrainId = terrainColors[color];
                    map[i, k].Terrain = conf.TerrainTypes[terrainId];
                }
            }

            Engine engine = new Engine(map, conf);
            foreach (var settlementJson in mapConfJson["Settlemets"])
            {
                int x = settlementJson["Position"].Value<int>("X");
                int y = settlementJson["Position"].Value<int>("Y");
                Point position = new Point(x, y);
                double gatherPower = settlementJson.Value<double>("GatherPower");
                Dictionary<ResourceType, double> resources = new();
                foreach (var resourse in conf.ResourceTypes.Values)
                {
                    resources.Add(resourse, 0);
                }
                Settlement settlment = new Settlement(position, gatherPower, resources);
                engine.Settlements.Add(settlment);
            }

            return engine;
        }
    }
}
