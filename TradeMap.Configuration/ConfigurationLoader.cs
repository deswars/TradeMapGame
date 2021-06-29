using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TradeMap.Configuration.GameLogEntry;
using TradeMap.Core;
using TradeMap.Core.Map;
using TradeMap.Di;
using TradeMap.Di.Attributes;
using TradeMap.GameLog;

namespace TradeMap.Configuration
{
    public class ConfigurationLoader
    {
        public const string NameDivisor = "-";

        public ConfigurationLoader(IGameLog log)
        {
            _log = log;
        }

        public void AddConfRoot(DirectoryInfo root)
        {
            _rootList.Add(root);
        }

        public TypeRepository LoadTypes()
        {
            //resource
            string resourceDataName = typeof(TypeRepository).GetProperty(nameof(TypeRepository.ResourceTypes))!.GetCustomAttribute<TypeAttribute>()!.Name;
            var resources = LoadResources(resourceDataName);

            //terrain
            string terrainDataName = typeof(TypeRepository).GetProperty(nameof(TypeRepository.TerrainTypes))!.GetCustomAttribute<TypeAttribute>()!.Name;
            var terrains = LoadTerrains(terrainDataName, resources);

            //feautre
            string feautreDataName = typeof(TypeRepository).GetProperty(nameof(TypeRepository.MapFeautreTypes))!.GetCustomAttribute<TypeAttribute>()!.Name;
            var feautres = LoadFeautres(feautreDataName, resources);

            //collector
            string collectorDataName = typeof(TypeRepository).GetProperty(nameof(TypeRepository.CollectorTypes))!.GetCustomAttribute<TypeAttribute>()!.Name;
            var collectors = LoadCollectors(collectorDataName, resources, terrains, feautres);

            //building
            string buildingDataName = typeof(TypeRepository).GetProperty(nameof(TypeRepository.BuildingTypes))!.GetCustomAttribute<TypeAttribute>()!.Name;
            var buildings = LoadBuildings(buildingDataName, resources);

            //pop demand
            string demandDataName = typeof(TypeRepository).GetProperty(nameof(TypeRepository.PopulationDemands))!.GetCustomAttribute<TypeAttribute>()!.Name;
            var demands = LoadPopDemands(demandDataName, resources);

            return new TypeRepository(resources, terrains, feautres, collectors, buildings, demands);
        }

        public static void WriteAvailableServiceJson(string file, IReadOnlyDictionary<string, Service> serviceList)
        {
            JArray servicesJson = new();
            foreach (var serviceKV in serviceList)
            {
                servicesJson.Add(GetServiceJson(serviceKV.Value));
            }
            File.WriteAllText(file, servicesJson.ToString());
        }

        public static Queue<string> LoadOrderedTurnActions(string file)
        {
            Queue<string> result = new();
            string actionOrderText = File.ReadAllText(file);
            JArray actionOrderJson = JArray.Parse(actionOrderText);
            foreach (var actionJson in actionOrderJson)
            {
                result.Enqueue(actionJson.ToString());
            }
            return result;
        }

        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, Constant>> LoadConstants(IReadOnlyDictionary<string, IReadOnlyDictionary<string, Constant>> constants)
        {
            var files = _rootList.SelectMany(dir => dir.GetDirectories("Constants")).SelectMany(resDir => resDir.GetFiles());
            foreach(var file in files)
            {
                try
                {
                    string constantTextText = File.ReadAllText(file.FullName);
                    JObject constantListJson = JObject.Parse(constantTextText);
                    foreach (var serviceKV in constants)
                    {
                        if (constantListJson.TryGetValue(serviceKV.Key, out var tokenJson))
                        {
                            JObject serviceJson = (JObject)tokenJson;
                            foreach (var constantKV in serviceKV.Value)
                            {
                                if (serviceJson.TryGetValue(constantKV.Key, out var constantJson))
                                {
                                    Constant constant = constantKV.Value;
                                    string newValue = constantJson.ToString();
                                    if (constant.Constraint.Check(newValue))
                                    {
                                        constant.Value = newValue;
                                    }
                                    else
                                    {
                                        _log.AddEntry(InfoLevels.Warning, () => new LogEntryConstantNewValueConstraint(file.FullName, serviceKV.Key, constantKV.Key, newValue, constant.Constraint));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    _log.AddEntry(InfoLevels.Warning, () => new LogEntryIncorrectFileFormat(file.FullName));
                }
            }
            return constants;
        }

        private readonly IGameLog _log;
        private readonly List<DirectoryInfo> _rootList = new();

        private IReadOnlyDictionary<string, ResourceType> LoadResources(string dataName)
        {
            var files = _rootList.SelectMany(dir => dir.GetDirectories(dataName)).SelectMany(resDir => resDir.GetFiles());
            Dictionary<string, ResourceType> resources = new();
            foreach (var file in files)
            {
                try
                {
                    string fileText = File.ReadAllText(file.FullName);
                    JArray json = JArray.Parse(fileText);

                    foreach (var resourceJson in json)
                    {
                        string id = resourceJson.Value<string>("Id")!;
                        double basePrice = resourceJson.Value<double>("BasePrice");
                        double decayRate = resourceJson.Value<double>("DecayRate");
                        var resourceType = new ResourceType(id, basePrice, decayRate);
                        if (resources.ContainsKey(id))
                        {
                            _log.AddEntry(InfoLevels.Warning, () => new LogEntryTypeOverride(id));
                        }
                        resources[id] = resourceType;
                    }
                }
                catch (Exception)
                {
                    _log.AddEntry(InfoLevels.Warning, () => new LogEntryIncorrectFileFormat(file.FullName));
                }
            }
            return resources;
        }

        private IReadOnlyDictionary<string, TerrainType> LoadTerrains(string dataName, IReadOnlyDictionary<string, ResourceType> resources)
        {
            var files = _rootList.SelectMany(dir => dir.GetDirectories(dataName)).SelectMany(resDir => resDir.GetFiles());
            Dictionary<string, TerrainType> terrains = new();
            foreach (var file in files)
            {
                try
                {
                    string fileText = File.ReadAllText(file.FullName);
                    JArray json = JArray.Parse(fileText);

                    foreach (var terrainJson in json)
                    {
                        string id = terrainJson.Value<string>("Id")!;

                        List<ResourceDeposit> resourceList = new();
                        var resourceListJson = terrainJson["Resources"]!;
                        foreach (var resourceJson in resourceListJson)
                        {
                            var resource = BuildResourceDeposit(resourceJson, resources);
                            resourceList.Add(resource);
                        }

                        TerrainType terrain = new(id, resourceList);
                        if (terrains.ContainsKey(id))
                        {
                            _log.AddEntry(InfoLevels.Warning, () => new LogEntryTypeOverride(id));
                        }
                        terrains[id] = terrain;
                    }
                }
                catch (Exception)
                {
                    _log.AddEntry(InfoLevels.Warning, () => new LogEntryIncorrectFileFormat(file.FullName));
                }
            }
            return terrains;
        }

        private IReadOnlyDictionary<string, TerrainFeautre> LoadFeautres(string dataName, IReadOnlyDictionary<string, ResourceType> resources)
        {
            var files = _rootList.SelectMany(dir => dir.GetDirectories(dataName)).SelectMany(resDir => resDir.GetFiles());
            Dictionary<string, TerrainFeautre> feautres = new();
            foreach (var resourceDataFile in files)
            {
                try
                {
                    string fileText = File.ReadAllText(resourceDataFile.FullName);
                    JArray json = JArray.Parse(fileText);

                    foreach (var feautreJson in json)
                    {
                        string id = feautreJson.Value<string>("Id")!;

                        List<ResourceDeposit> resourceList = new();
                        var resourceListJson = feautreJson["Resources"]!;
                        foreach (var resourceJson in resourceListJson)
                        {
                            var resource = BuildResourceDeposit(resourceJson, resources);
                            resourceList.Add(resource);
                        }

                        TerrainFeautre feautre = new(id, resourceList);
                        if (feautres.ContainsKey(id))
                        {
                            _log.AddEntry(InfoLevels.Warning, () => new LogEntryTypeOverride(id));
                        }
                        feautres[id] = feautre;
                    }
                }
                catch (Exception)
                {
                    _log.AddEntry(InfoLevels.Warning, () => new LogEntryIncorrectFileFormat(resourceDataFile.FullName));
                }
            }
            return feautres;
        }

        private static ResourceDeposit BuildResourceDeposit(JToken json, IReadOnlyDictionary<string, ResourceType> resources)
        {
            string resourceId = json.Value<string>("Resource")!;
            ResourceType type = resources[resourceId];
            double richness = json.Value<double>("Richness");
            return new ResourceDeposit(type, richness);
        }

        private IReadOnlyDictionary<string, CollectorType> LoadCollectors(
            string dataName,
            IReadOnlyDictionary<string, ResourceType> resources,
            IReadOnlyDictionary<string, TerrainType> terrains,
            IReadOnlyDictionary<string, TerrainFeautre> feautres)
        {
            var files = _rootList.SelectMany(dir => dir.GetDirectories(dataName)).SelectMany(resDir => resDir.GetFiles());
            Dictionary<string, CollectorType> collectors = new();
            foreach (var file in files)
            {
                try
                {
                    string fileText = File.ReadAllText(file.FullName);
                    JArray json = JArray.Parse(fileText);

                    foreach (var collectorJson in json)
                    {
                        string id = collectorJson.Value<string>("Id")!;

                        List<TerrainType> requiredTerrain = new();
                        var terrainListJson = collectorJson["RequiredTerrain"]!;
                        foreach (var terrainJson in terrainListJson)
                        {
                            TerrainType terrain = terrains[terrainJson.ToString()];
                            requiredTerrain.Add(terrain);
                        }

                        List<TerrainFeautre> requiredFeautre = new();
                        var feautreListJson = collectorJson["RequiredFeautre"]!;
                        foreach (var feautreJson in feautreListJson)
                        {
                            TerrainFeautre feautre = feautres[feautreJson.ToString()];
                            requiredFeautre.Add(feautre);
                        }

                        List<ResourceType> collectedResources = new();
                        var collectedListJson = collectorJson["CollectedResources"]!;
                        foreach (var collectedJson in collectedListJson)
                        {
                            ResourceType resource = resources[collectedJson.ToString()];
                            collectedResources.Add(resource);
                        }
                        CollectorType collector = new(id, requiredTerrain, requiredFeautre, collectedResources);
                        if (collectors.ContainsKey(id))
                        {
                            _log.AddEntry(InfoLevels.Warning, () => new LogEntryTypeOverride(id));
                        }
                        collectors[id] = collector;
                    }
                }
                catch (Exception)
                {
                    _log.AddEntry(InfoLevels.Warning, () => new LogEntryIncorrectFileFormat(file.FullName));
                }
            }
            return collectors;
        }

        private IReadOnlyDictionary<string, BuildingType> LoadBuildings(string dataName, IReadOnlyDictionary<string, ResourceType> resources)
        {
            var files = _rootList.SelectMany(dir => dir.GetDirectories(dataName)).SelectMany(resDir => resDir.GetFiles());
            Dictionary<string, BuildingType> buildings = new();
            foreach (var file in files)
            {
                try
                {
                    string fileText = File.ReadAllText(file.FullName);
                    JArray json = JArray.Parse(fileText);

                    foreach (var buildingJson in json)
                    {
                        string id = buildingJson.Value<string>("Id")!;

                        List<ResourceType> inputResources = new();
                        KeyedVector<ResourceType> input = new(resources.Values);
                        var inputListJson = buildingJson["Input"]!;
                        foreach (var resourceJson in inputListJson)
                        {
                            ResourceType resource = resources[resourceJson.Value<string>("Resource")!];
                            inputResources.Add(resource);
                            double amount = resourceJson.Value<double>("Amount");
                            input[resource] = amount;
                        }
                        input = input.FilterSmaller(inputResources);

                        List<ResourceType> outputResources = new();
                        KeyedVector<ResourceType> output = new(resources.Values);
                        var outputListJson = buildingJson["Output"]!;
                        foreach (var resourceJson in outputListJson)
                        {
                            ResourceType resource = resources[resourceJson.Value<string>("Resource")!];
                            outputResources.Add(resource);
                            double amount = resourceJson.Value<double>("Amount");
                            output[resource] = amount;
                        }
                        output = output.FilterSmaller(outputResources);

                        BuildingType building = new(id, input, output);
                        if (buildings.ContainsKey(id))
                        {
                            _log.AddEntry(InfoLevels.Warning, () => new LogEntryTypeOverride(id));
                        }
                        buildings[id] = building;
                    }
                }
                catch (Exception)
                {
                    _log.AddEntry(InfoLevels.Warning, () => new LogEntryIncorrectFileFormat(file.FullName));
                }
            }
            return buildings;
        }

        private IReadOnlyDictionary<int, KeyedVector<ResourceType>> LoadPopDemands(string dataName, IReadOnlyDictionary<string, ResourceType> resources)
        {
            var files = _rootList.SelectMany(dir => dir.GetDirectories(dataName)).SelectMany(resDir => resDir.GetFiles());
            Dictionary<int, KeyedVector<ResourceType>> demands = new();
            foreach (var resourceDataFile in files)
            {
                try
                {
                    string fileText = File.ReadAllText(resourceDataFile.FullName);
                    JArray json = JArray.Parse(fileText);

                    foreach (var tierJson in json)
                    {
                        var tier = tierJson.Value<int>("Tier");
                        var demandsListJson = tierJson["Demands"]!;
                        KeyedVector<ResourceType> tierDemands = new(new List<ResourceType>());
                        foreach (var demandJson in demandsListJson)
                        {
                            ResourceType resource = resources[demandJson.Value<string>("Resource")!];
                            double amount = demandJson.Value<double>("Amount");
                            tierDemands[resource] = amount;
                        }
                        demands[tier] = tierDemands;
                    }
                }
                catch (Exception)
                {
                    _log.AddEntry(InfoLevels.Warning, () => new LogEntryIncorrectFileFormat(resourceDataFile.FullName));
                }
            }
            if (!demands.ContainsKey(0))
            {
                demands[0] = new KeyedVector<ResourceType>(new List<ResourceType>());
            }
            int i = 1;
            while (i < demands.Count)
            {
                if (!demands.ContainsKey(i))
                {
                    demands[i] = demands[i - 1];
                }
                i++;
            }
            return demands;
        }

        private static JToken GetServiceJson(Service service)
        {
            JObject serviceJson = new();
            serviceJson.Add(nameof(service.Name), service.Name);
            serviceJson.Add(nameof(service.ServiceType), service.ServiceType.FullName);
            JArray constantArrayJson = new();
            foreach (var constant in service.Constants)
            {
                constantArrayJson.Add(GetConstantJson(constant.Value));
            }
            serviceJson.Add(nameof(service.Constants), constantArrayJson);
            JArray actionArrayJson = new();
            foreach (var action in service.Actions)
            {
                actionArrayJson.Add(GetActionJson(action.Value));
            }
            serviceJson.Add(nameof(service.Actions), actionArrayJson);
            return serviceJson;
        }

        private static JToken GetConstantJson(Constant constant)
        {
            JObject constantJson = new();
            constantJson.Add(nameof(constant.Name), constant.Name);
            constantJson.Add(nameof(constant.Value), constant.Value);
            constantJson.Add(nameof(constant.Constraint), constant.Constraint.ToString());
            return constantJson;
        }

        private static JToken GetActionJson(TurnAction action)
        {
            JObject actionJson = new();
            actionJson.Add(nameof(action.Name), action.Name);
            actionJson.Add(nameof(action.Action), action.Action.Name);
            return actionJson;
        }
    }
}
