using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace TradeMapGame
{
    public class Configuration
    {
        public string MoneyResourceId { get; private set; }
        public ResourceType MoneyResource { get; private set; }
        public string DefaultTerrain { get; private set; }
        public double DesiredReserveTurns { get; private set; }
        public double ChanceToCreateCollector { get; private set; }
        public double ExpansionDelay { get; private set; }
        public double StarvePriceMultiply { get; private set; }
        public double ExcessPriceDivider { get; private set; }
        public double MaxExcessPriceDivider { get; private set; }
        public double UndesiredEffectiveExcess { get; private set; }
        public double MinPrice { get; private set; }
        public double MaxPrice { get; private set; }
        public double TaxPerPop { get; private set; }
        public Dictionary<string, TerrainType> TerrainTypes { get; }
        public Dictionary<string, ResourceType> ResourceTypes { get; }
        public Dictionary<string, TerrainFeautre> MapFeautreTypes { get; }
        public Dictionary<string, CollectorType> CollectorTypes { get; }
        public Dictionary<string, BuildingType> BuildingTypes { get; }
        public Dictionary<ResourceType, double> PopulationDemands { get; }
        public Dictionary<ResourceType, double> GrowthDemand { get; }

        public Configuration(string configurationFile)
        {
            TerrainTypes = new();
            ResourceTypes = new();
            MapFeautreTypes = new();
            CollectorTypes = new();
            BuildingTypes = new();
            PopulationDemands = new();
            GrowthDemand = new();

            string configurationJson = File.ReadAllText(configurationFile);
            var inputJson = JObject.Parse(configurationJson);
            JToken constatsJson = inputJson["Constants"];
            BuildConstants(constatsJson);

#pragma warning disable CA1507 // Use nameof to express symbol names
            JArray resourceListJson = (JArray)inputJson["ResourceTypes"];
            BuildResources(resourceListJson);
            MoneyResource = ResourceTypes[MoneyResourceId];

            JArray terrainListJson = (JArray)inputJson["TerrainTypes"];
            BuildTerrainType(terrainListJson);

            JArray feautreListJson = (JArray)inputJson["MapFeautreTypes"];
            BuildMapFeautre(feautreListJson);

            JArray collectorListJson = (JArray)inputJson["CollectorTypes"];
            BuildCollectorTypes(collectorListJson);

            JArray buildingListJson = (JArray)inputJson["BuildingTypes"];
            BuildBuildingTypes(buildingListJson);

            JArray demandsListJson = (JArray)inputJson["Population"]["Demands"];
            BuildPopDemands(demandsListJson);
#pragma warning restore CA1507 // Use nameof to express symbol names
        }

        private void BuildConstants(JToken constantsJson)
        {
            MoneyResourceId = constantsJson.Value<string>("MoneyResource");
            DefaultTerrain = constantsJson.Value<string>("DefaultTerrain");
            DesiredReserveTurns = constantsJson.Value<double>("DesiredReserveTurns");
            ChanceToCreateCollector = constantsJson.Value<double>("ChanceToCreateCollector");
            ExpansionDelay = constantsJson.Value<int>("ExpansionDelay");
            StarvePriceMultiply = constantsJson.Value<double>("StarvePriceMultiply");
            ExcessPriceDivider = constantsJson.Value<double>("ExcessPriceDivider");
            UndesiredEffectiveExcess = constantsJson.Value<double>("UndesiredEffectiveExcess");
            MaxExcessPriceDivider = constantsJson.Value<double>("MaxExcessPriceDivider");
            MinPrice = constantsJson.Value<double>("MinPrice");
            MaxPrice = constantsJson.Value<double>("MaxPrice");
            TaxPerPop = constantsJson.Value<double>("TaxPerPop");
        }

        private void BuildResources(JToken resourceListJson)
        {
            foreach (var resourceJson in resourceListJson)
            {
                string id = resourceJson.Value<string>("Id");
                double basePrice = resourceJson.Value<double>("BasePrice");
                double decayRate = resourceJson.Value<double>("DecayRate");
                var resourceType = new ResourceType(id, basePrice, decayRate);
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

                TerrainType terrain = new(id, resourceList);
                TerrainTypes.Add(id, terrain);
            }
        }

        private ResourceDeposit BuildResourceDeposit(JToken resourceJson)
        {
            string resourceId = resourceJson.Value<string>("Resource");
            ResourceType type = ResourceTypes[resourceId];
            double richness = resourceJson.Value<double>("Richness");
            return new ResourceDeposit(type, richness);
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

                TerrainFeautre feautre = new(id, resourceList);
                MapFeautreTypes.Add(id, feautre);
            }
        }

        private void BuildCollectorTypes(JToken collectorListJson)
        {
            foreach (var collectorJson in collectorListJson)
            {
                string id = collectorJson.Value<string>("Id");

                List<TerrainType> requiredTerrain = new();
                var terrainListJson = collectorJson["RequiredTerrain"];
                foreach (var terrainJson in terrainListJson)
                {
                    TerrainType terrain = TerrainTypes[terrainJson.ToString()];
                    requiredTerrain.Add(terrain);
                }

                List<TerrainFeautre> requiredFeautre = new();
                var feautreListJson = collectorJson["RequiredFeautre"];
                foreach (var feautreJson in feautreListJson)
                {
                    TerrainFeautre feautre = MapFeautreTypes[feautreJson.ToString()];
                    requiredFeautre.Add(feautre);
                }

                List<ResourceType> collectedResources = new();
                var collectedListJson = collectorJson["CollectedResources"];
                foreach (var collectedJson in collectedListJson)
                {
                    ResourceType resource = ResourceTypes[collectedJson.ToString()];
                    collectedResources.Add(resource);
                }
                CollectorType collector = new(id, requiredTerrain, requiredFeautre, collectedResources);
                CollectorTypes.Add(id, collector);
            }
        }

        private void BuildBuildingTypes(JToken buildingListJson)
        {
            foreach (var buildingJson in buildingListJson)
            {
                string id = buildingJson.Value<string>("Id");

                Dictionary<ResourceType, double> input = new();
                var inputListJson = buildingJson["Input"];
                foreach (var resourceJson in inputListJson)
                {
                    ResourceType resource = ResourceTypes[resourceJson.Value<string>("Resource")];
                    double amount = resourceJson.Value<double>("Amount");
                    input.Add(resource, amount);
                }

                Dictionary<ResourceType, double> output = new();
                var outputListJson = buildingJson["Output"];
                foreach (var resourceJson in outputListJson)
                {
                    ResourceType resource = ResourceTypes[resourceJson.Value<string>("Resource")];
                    double amount = resourceJson.Value<double>("Amount");
                    output.Add(resource, amount);
                }

                BuildingType building = new(id, input, output);
                BuildingTypes.Add(id, building);
            }
        }

        private void BuildPopDemands(JToken demandsListJson)
        {
            foreach(var resource in ResourceTypes)
            {
                PopulationDemands.Add(resource.Value, 0);
            }

            foreach (var demandJson in demandsListJson)
            {
                ResourceType resource = ResourceTypes[demandJson.Value<string>("Resource")];
                double amount = demandJson.Value<double>("Amount");
                PopulationDemands[resource] = amount;
                if (demandJson["Growth"] != null)
                {
                    GrowthDemand.Add(resource, demandJson.Value<double>("Growth"));
                }
            }
        }
    }
}
