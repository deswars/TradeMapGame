using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace TradeMapGame
{
    public class Configuration
    {
        public double MinMovementDifficultyModifier { get; }
        public string MoneyResource { get; }
        public string DefaultTerrain { get; }
        public Dictionary<string, TerrainType> TerrainTypes { get; }
        public Dictionary<string, ResourceType> ResourceTypes { get; }
        public Dictionary<string, TerrainFeautre> MapFeautreTypes { get; }


        public Configuration(string configurationFile)
        {
            TerrainTypes = new();
            ResourceTypes = new();
            MapFeautreTypes = new();

            string configurationJson = File.ReadAllText(configurationFile);
            var inputJson = JObject.Parse(configurationJson);
            MinMovementDifficultyModifier = inputJson["Constants"].Value<double>("MinMovementDifficultyModifier");
            MoneyResource = inputJson["Constants"].Value<string>("MoneyResource");
            DefaultTerrain = inputJson["Constants"].Value<string>("DefaultTerrain");

            JArray resourceListJson = (JArray)inputJson["ResourceTypes"];
            BuildResources(resourceListJson);

            JArray terrainListJson = (JArray)inputJson["TerrainTypes"];
            BuildTerrainType(terrainListJson);

            JArray feautreListJson = (JArray)inputJson["MapFeautreTypes"];
            BuildMapFeautre(feautreListJson);
        }


        private void BuildResources(JToken resourceListJson)
        {
            foreach (var resourceJson in resourceListJson)
            {
                string id = resourceJson.Value<string>("Id");
                var resourceType = new ResourceType(id);
                ResourceTypes.Add(id, resourceType);
            }
        }

        private void BuildTerrainType(JToken terrainListJson)
        {
            foreach (var terrainJson in terrainListJson)
            {
                string id = terrainJson.Value<string>("Id");

                List<ResourceDeposit> resourceList = new();
                var resourceListJson = terrainJson["Resources"];
                foreach (var resourceJson in resourceListJson)
                {
                    var resource = BuildResourceDeposit(resourceJson);
                    resourceList.Add(resource);
                }

                TerrainType terrain = new TerrainType(id, resourceList);
                TerrainTypes.Add(id, terrain);
            }
        }

        private ResourceDeposit BuildResourceDeposit(JToken resourceJson)
        {
            string resourceId = resourceJson.Value<string>("Resource");
            ResourceType type = ResourceTypes[resourceId];
            double difficulty = resourceJson.Value<double>("Difficulty");
            double richness = resourceJson.Value<double>("Richness");
            return new ResourceDeposit(type, difficulty, richness);
        }

        private void BuildMapFeautre(JToken feautreListJson)
        {
            foreach (var feautreJson in feautreListJson)
            {
                string id = feautreJson.Value<string>("Id");

                List<ResourceDeposit> resourceList = new();
                var resourceListJson = feautreJson["Resources"];
                foreach (var resourceJson in resourceListJson)
                {
                    var resource = BuildResourceDeposit(resourceJson);
                    resourceList.Add(resource);
                }

                TerrainFeautre feautre = new TerrainFeautre(id, resourceList);
                MapFeautreTypes.Add(id, feautre);
            }
        }
    }
}
