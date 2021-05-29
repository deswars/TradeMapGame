using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TradeMapGame.Map;

namespace TradeMapGame.Configuration
{
    public class ConfigurationLoader
    {
        public Constants Const { get; }
        public TypeRepository Lists { get; } = new();
        public List<string> LoadingErrors { get; } = new();

        public ConfigurationLoader()
        {
            TerrainType nullTerrain = new("t_null", new List<ResourceDeposit>());
            Lists.TerrainTypes.Add(nullTerrain.Id, nullTerrain);

            ResourceType nullResource = new("r_null", 1, 1);
            Lists.ResourceTypes.Add(nullResource.Id, nullResource);

            Lists.PopulationDemands.Add(0, new Dictionary<ResourceType, double>());

            Const = new Constants(nullResource, nullTerrain);
        }

        public void Load(DirectoryInfo root)
        {
            var subRoot = root.GetDirectories();

            //resources
            var resourceDir = subRoot.SingleOrDefault(x => x.Name == "Resources");
            if (resourceDir != null)
            {
                var files = resourceDir.GetFiles();
                foreach (var file in files)
                {
                    try
                    {
                        string fileText = File.ReadAllText(file.FullName);
                        JArray json = JArray.Parse(fileText);
                        BuildResources(json);
                    }
                    catch (Exception e)
                    {
                        LoadingErrors.Add("\"" + file.FullName + "\":" + e.Message);
                    }
                }
            }

            //terrain
            var terrainDir = subRoot.SingleOrDefault(x => x.Name == "Terrain");
            if (terrainDir != null)
            {
                var files = terrainDir.GetFiles();
                foreach (var file in files)
                {
                    try
                    {
                        string fileText = File.ReadAllText(file.FullName);
                        JArray json = JArray.Parse(fileText);
                        BuildTerrain(json);
                    }
                    catch (Exception e)
                    {
                        LoadingErrors.Add("\"" + file.FullName + "\":" + e.Message);
                    }
                }
            }

            //feautres
            var feautreDir = subRoot.SingleOrDefault(x => x.Name == "MapFeautres");
            if (feautreDir != null)
            {
                var files = feautreDir.GetFiles();
                foreach (var file in files)
                {
                    try
                    {
                        string fileText = File.ReadAllText(file.FullName);
                        JArray json = JArray.Parse(fileText);
                        BuildMapFeautre(json);
                    }
                    catch (Exception e)
                    {
                        LoadingErrors.Add("\"" + file.FullName + "\":" + e.Message);
                    }
                }
            }

            //collectors
            var collectorDir = subRoot.SingleOrDefault(x => x.Name == "Collectors");
            if (collectorDir != null)
            {
                var files = collectorDir.GetFiles();
                foreach (var file in files)
                {
                    try
                    {
                        string fileText = File.ReadAllText(file.FullName);
                        JArray json = JArray.Parse(fileText);
                        BuildCollector(json);
                    }
                    catch (Exception e)
                    {
                        LoadingErrors.Add("\"" + file.FullName + "\":" + e.Message);
                    }
                }
            }

            //buildings
            var buildingDir = subRoot.SingleOrDefault(x => x.Name == "Buildings");
            if (buildingDir != null)
            {
                var files = buildingDir.GetFiles();
                foreach (var file in files)
                {
                    try
                    {
                        string fileText = File.ReadAllText(file.FullName);
                        JArray json = JArray.Parse(fileText);
                        BuildBuilding(json);
                    }
                    catch (Exception e)
                    {
                        LoadingErrors.Add("\"" + file.FullName + "\":" + e.Message);
                    }
                }
            }

            //constants
            var constantDir = subRoot.SingleOrDefault(x => x.Name == "Constants");
            if (constantDir != null)
            {
                var files = constantDir.GetFiles();
                foreach (var file in files)
                {
                    try
                    {
                        string fileText = File.ReadAllText(file.FullName);
                        JObject json = JObject.Parse(fileText);
                        BuildConstants(json);
                    }
                    catch (Exception e)
                    {
                        LoadingErrors.Add("\"" + file.FullName + "\":" + e.Message);
                    }
                }
            }

            //population
            var demands = Lists.PopulationDemands;
            for (int i = 1; i <= Const.MaxPopTier; i++)
            {
                Lists.PopulationDemands.Add(i, new Dictionary<ResourceType, double>());
                foreach (var resource in Lists.ResourceTypes)
                {
                    Lists.PopulationDemands[i].Add(resource.Value, 0);
                }
            }

            var populationDir = subRoot.SingleOrDefault(x => x.Name == "Population");
            if (populationDir != null)
            {
                var files = populationDir.GetFiles();
                foreach (var file in files)
                {
                    try
                    {
                        string fileText = File.ReadAllText(file.FullName);
                        JArray json = JArray.Parse(fileText);
                        BuildPopulation(json);
                    }
                    catch (Exception e)
                    {
                        LoadingErrors.Add("\"" + file.FullName + "\":" + e.Message);
                    }
                }
            }
        }

        public void BuildResources(JArray json)
        {
            foreach (var resourceJson in json)
            {
                string id = resourceJson.Value<string>("Id")!;
                double basePrice = resourceJson.Value<double>("BasePrice");
                double decayRate = resourceJson.Value<double>("DecayRate");
                var resourceType = new ResourceType(id, basePrice, decayRate);
                Lists.ResourceTypes.Add(id, resourceType);
            }
        }

        public void BuildTerrain(JArray json)
        {
            foreach (var terrainJson in json)
            {
                string id = terrainJson.Value<string>("Id")!;

                List<ResourceDeposit> resourceList = new();
                var resourceListJson = terrainJson["Resources"]!;
                foreach (var resourceJson in resourceListJson)
                {
                    var resource = BuildResourceDeposit(resourceJson);
                    resourceList.Add(resource);
                }

                TerrainType terrain = new(id, resourceList);
                Lists.TerrainTypes.Add(id, terrain);
            }
        }

        private ResourceDeposit BuildResourceDeposit(JToken json)
        {
            string resourceId = json.Value<string>("Resource")!;
            ResourceType type = Lists.ResourceTypes[resourceId];
            double richness = json.Value<double>("Richness");
            return new ResourceDeposit(type, richness);
        }

        private void BuildMapFeautre(JArray json)
        {
            foreach (var feautreJson in json)
            {
                string id = feautreJson.Value<string>("Id")!;

                List<ResourceDeposit> resourceList = new();
                var resourceListJson = feautreJson["Resources"]!;
                foreach (var resourceJson in resourceListJson)
                {
                    var resource = BuildResourceDeposit(resourceJson);
                    resourceList.Add(resource);
                }

                TerrainFeautre feautre = new(id, resourceList);
                Lists.MapFeautreTypes.Add(id, feautre);
            }
        }

        private void BuildCollector(JArray json)
        {
            foreach (var collectorJson in json)
            {
                string id = collectorJson.Value<string>("Id")!;

                List<TerrainType> requiredTerrain = new();
                var terrainListJson = collectorJson["RequiredTerrain"]!;
                foreach (var terrainJson in terrainListJson)
                {
                    TerrainType terrain = Lists.TerrainTypes[terrainJson.ToString()];
                    requiredTerrain.Add(terrain);
                }

                List<TerrainFeautre> requiredFeautre = new();
                var feautreListJson = collectorJson["RequiredFeautre"]!;
                foreach (var feautreJson in feautreListJson)
                {
                    TerrainFeautre feautre = Lists.MapFeautreTypes[feautreJson.ToString()];
                    requiredFeautre.Add(feautre);
                }

                List<ResourceType> collectedResources = new();
                var collectedListJson = collectorJson["CollectedResources"]!;
                foreach (var collectedJson in collectedListJson)
                {
                    ResourceType resource = Lists.ResourceTypes[collectedJson.ToString()];
                    collectedResources.Add(resource);
                }
                CollectorType collector = new(id, requiredTerrain, requiredFeautre, collectedResources);
                Lists.CollectorTypes.Add(id, collector);
            }
        }

        private void BuildBuilding(JArray buildingListJson)
        {
            foreach (var buildingJson in buildingListJson)
            {
                string id = buildingJson.Value<string>("Id")!;

                KeyedVector<ResourceType> input = new(Lists.ResourceTypes.Values);
                var inputListJson = buildingJson["Input"]!;
                foreach (var resourceJson in inputListJson)
                {
                    ResourceType resource = Lists.ResourceTypes[resourceJson.Value<string>("Resource")!];
                    double amount = resourceJson.Value<double>("Amount");
                    input[resource] = amount;
                }

                KeyedVector<ResourceType> output = input.Zeroed();
                var outputListJson = buildingJson["Output"]!;
                foreach (var resourceJson in outputListJson)
                {
                    ResourceType resource = Lists.ResourceTypes[resourceJson.Value<string>("Resource")!];
                    double amount = resourceJson.Value<double>("Amount");
                    output[resource] = amount;
                }

                BuildingType building = new(id, input, output);
                Lists.BuildingTypes.Add(id, building);
            }
        }

        private void BuildPopulation(JToken json)
        {
            foreach (var tierJson in json)
            {
                var tier = tierJson.Value<int>("Tier");
                if (tier <= Const.MaxPopTier)
                {
                    var demandsListJson = tierJson["Demands"]!;
                    foreach (var demandJson in demandsListJson)
                    {
                        ResourceType resource = Lists.ResourceTypes[demandJson.Value<string>("Resource")!];
                        double amount = demandJson.Value<double>("Amount");
                        Lists.PopulationDemands[tier][resource] = amount;
                    }
                }
            }
        }

        private void BuildConstants(JObject json)
        {
            if (json.ContainsKey("MoneyResource"))
            {
                Const.MoneyResourceId = json.Value<string>("MoneyResource")!;
                Const.MoneyResource = Lists.ResourceTypes[Const.MoneyResourceId];
            }
            if (json.ContainsKey("MoneyResource"))
            {
                Const.DefaultTerrainId = json.Value<string>("DefaultTerrain")!;
                Const.DefaultTerrain = Lists.TerrainTypes[Const.DefaultTerrainId];
            }
            if (json.ContainsKey("MinPrice"))
            {
                Const.MinPrice = json.Value<double>("MinPrice");
            }
            if (json.ContainsKey("MaxPrice"))
            {
                Const.MaxPrice = json.Value<double>("MaxPrice");
            }
            if (json.ContainsKey("TaxPerPop"))
            {
                Const.TaxPerPop = json.Value<double>("TaxPerPop");
            }
            if (json.ContainsKey("MaxPopTier"))
            {
                Const.MaxPopTier = json.Value<int>("MaxPopTier");
            }
            if (json.ContainsKey("TierLevelLimit"))
            {
                Const.TierLevelLimit = json.Value<double>("TierLevelLimit");
            }
            if (json.ContainsKey("TierLevelUpStep"))
            {
                Const.TierLevelUpStep = json.Value<double>("TierLevelUpStep");
            }
            if (json.ContainsKey("TierLevelUpLimit"))
            {
                Const.TierLevelUpLimit = json.Value<double>("TierLevelUpLimit");
            }
            if (json.ContainsKey("TierLevelDownStep"))
            {
                Const.TierLevelDownStep = json.Value<double>("TierLevelDownStep");
            }
            if (json.ContainsKey("TierLevelDownLimit"))
            {
                Const.TierLevelDownLimit = json.Value<double>("TierLevelDownLimit");
            }
        }
    }
}
