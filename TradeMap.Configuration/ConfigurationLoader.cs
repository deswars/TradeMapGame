using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TradeMap.Configuration.JsonDto;
using TradeMap.Core;
using TradeMap.Core.Map.ReadOnly;
using TradeMap.Di;
using TradeMap.Di.Attributes;
using TradeMap.Engine.Map;
using TradeMap.GameLog;

namespace TradeMap.Configuration
{
    public class ConfigurationLoader
    {
        private const string _errorTypeJson = "TypeJsonError";
        private const string _errorFeautreJson = "FeautreJsonError";
        private const string _errorMissingType = "MissingResourceError";
        private const string _errorParseConstant = "ParseConstantError";
        private const string _errorValidateConstant = "ValidateConstantError";


        public ITypeRepository TypeRepository { get { return _typeRepository; } }


        private readonly IGameLog _log;
        private readonly List<DirectoryInfo> _rootList = new();
        private readonly TypeRepository _typeRepository = new();
        private readonly JsonSerializerSettings _serializationSettings = new();
        private readonly Dictionary<string, JToken> _constants = new();


        public ConfigurationLoader(IGameLog log)
        {
            _log = log;
            _serializationSettings.MissingMemberHandling = MissingMemberHandling.Error;
        }


        public void AddConfRoot(DirectoryInfo root)
        {
            _rootList.Add(root);
        }

        public Dictionary<int, Queue<FeautreInfo>> LoadFeautres(FileInfo file)
        {
            Dictionary<int, Queue<FeautreInfo>> result = new();
            string jsonString = File.ReadAllText(file.FullName);
            try
            {
                var feautreDtoList = JsonConvert.DeserializeObject<List<FeautreDto>>(jsonString, _serializationSettings);

                if (feautreDtoList == null)
                {
                    throw new FormatException();
                }

                foreach (var feautreDto in feautreDtoList)
                {
                    int subStep = feautreDto.Substep;
                    if (!result.ContainsKey(subStep))
                    {
                        result[subStep] = new Queue<FeautreInfo>();
                    }
                    result[subStep].Enqueue(feautreDto.ConvertToFeatreInfo());
                }
            }
            catch (Exception e)
            {
                _log.AddEntry(
                    InfoLevels.Error,
                    () => new BaseLogEntry(_errorFeautreJson, new Dictionary<string, object>() { { "file", file }, { "exception", e.Message } })
                    );
            }
            return result;
        }

        public ITypeRepository LoadPackages()
        {
            foreach (var packageRoot in _rootList)
            {
                var newConst = LoadPackage(packageRoot);
                if (newConst != null)
                {
                    foreach (var constKV in newConst)
                    {
                        _constants[constKV.Key] = constKV.Value;
                    }
                }
            }
            FillAllPopulationDemandsLevels();
            return TypeRepository;
        }

        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, Constant>> SetupConstants(IReadOnlyDictionary<string, IReadOnlyDictionary<string, Constant>> constants)
        {
            foreach (var service in constants)
            {
                foreach (var constantKV in service.Value)
                {
                    if (_constants.ContainsKey(constantKV.Key))
                    {
                        var constant = constantKV.Value;
                        var error = constant.TrySetValue(_constants[constantKV.Key], _typeRepository);
                        if (error == TrySetError.ParseError)
                        {
                            _log.AddEntry(
                                InfoLevels.Error,
                                () => new BaseLogEntry(_errorParseConstant, new Dictionary<string, object>() { { "constat", constantKV.Key }, { "parser", constant.Parser } })
                                );
                        }
                        if (error == TrySetError.ValidationError)
                        {
                            _log.AddEntry(
                                InfoLevels.Error,
                                () => new BaseLogEntry(_errorValidateConstant, new Dictionary<string, object>() { { "constat", constantKV.Key }, { "validator", constant.Validator! } })
                                );
                        }
                    }
                }
            }
            return constants;
        }

        private Dictionary<string, JToken>? LoadPackage(DirectoryInfo root)
        {
            //Resources
            var resourceAttribute = typeof(TypeRepository).
                GetProperty(nameof(TypeRepository.ResourceTypes))!.
                GetCustomAttribute<TypeCategoryAttribute>(false)!;
            var resourcesRoot = root.GetDirectories(resourceAttribute.Name, SearchOption.TopDirectoryOnly);
            if (resourcesRoot.Length == 1)
            {
                LoadPackageResource(resourcesRoot[0]);
            }
            KeyedVectorFull<IResourceType>.SetAvailableKeys(TypeRepository.ResourceTypes.Values);

            //Terrain
            var terrainAttribute = typeof(TypeRepository).
                GetProperty(nameof(TypeRepository.TerrainTypes))!.
                GetCustomAttribute<TypeCategoryAttribute>(false)!;
            var terrainRoot = root.GetDirectories(terrainAttribute.Name, SearchOption.TopDirectoryOnly);
            if (terrainRoot.Length == 1)
            {
                LoadPackageTerrain(terrainRoot[0]);
            }

            //MapFeautres
            var feautreAttribute = typeof(TypeRepository).
                GetProperty(nameof(TypeRepository.TerrainFeautreTypes))!.
                GetCustomAttribute<TypeCategoryAttribute>(false)!;
            var feautreRoot = root.GetDirectories(feautreAttribute.Name, SearchOption.TopDirectoryOnly);
            if (feautreRoot.Length == 1)
            {
                LoadPackageFeautre(feautreRoot[0]);
            }

            //Collectors
            var collectorAttribute = typeof(TypeRepository).
                GetProperty(nameof(TypeRepository.CollectorTypes))!.
                GetCustomAttribute<TypeCategoryAttribute>(false)!;
            var collectorRoot = root.GetDirectories(collectorAttribute.Name, SearchOption.TopDirectoryOnly);
            if (collectorRoot.Length == 1)
            {
                LoadPackageCollector(collectorRoot[0]);
            }

            //Buildings
            var buildingAttribute = typeof(TypeRepository).
                GetProperty(nameof(TypeRepository.BuildingTypes))!.
                GetCustomAttribute<TypeCategoryAttribute>(false)!;
            var buildingRoot = root.GetDirectories(buildingAttribute.Name, SearchOption.TopDirectoryOnly);
            if (buildingRoot.Length == 1)
            {
                LoadPackageBuilding(buildingRoot[0]);
            }

            //Population
            var demandsAttribute = typeof(TypeRepository).
                GetProperty(nameof(TypeRepository.PopulationDemands))!.
                GetCustomAttribute<TypeCategoryAttribute>(false)!;
            var demandsRoot = root.GetDirectories(demandsAttribute.Name, SearchOption.TopDirectoryOnly);
            if (demandsRoot.Length == 1)
            {
                LoadPackageDemands(demandsRoot[0]);
            }

            //Constants
            var constantRoot = root.GetDirectories("Constants", SearchOption.TopDirectoryOnly);
            if (constantRoot.Length == 1)
            {
                return LoadPackageConstants(constantRoot[0]);
            }
            return null;
        }

        private void LoadPackageResource(DirectoryInfo root)
        {
            var jsonFiles = root.GetFiles("*.json", SearchOption.AllDirectories);
            foreach (var file in jsonFiles)
            {
                try
                {
                    string json = File.ReadAllText(file.FullName);
                    var groupList = JsonConvert.DeserializeObject<List<GroupDto<ResourceDto>>>(json, _serializationSettings);

                    if (groupList == null)
                    {
                        throw new FormatException();
                    }

                    foreach (var group in groupList)
                    {
                        if (CheckGroupRequirements(group))
                        {
                            foreach (var dataType in group.Include)
                            {
                                SaveTypeResource(dataType);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.AddEntry(
                        InfoLevels.Error,
                        () => new BaseLogEntry(_errorTypeJson, new Dictionary<string, object>() { { "file", file }, { "exception", e.Message } })
                        );
                }
            }
        }

        private void LoadPackageTerrain(DirectoryInfo root)
        {
            var jsonFiles = root.GetFiles("*.json", SearchOption.AllDirectories);
            foreach (var file in jsonFiles)
            {
                try
                {
                    string json = File.ReadAllText(file.FullName);
                    var groupList = JsonConvert.DeserializeObject<List<GroupDto<TerrainDto>>>(json, _serializationSettings);

                    if (groupList == null)
                    {
                        throw new FormatException();
                    }

                    foreach (var group in groupList)
                    {
                        if (CheckGroupRequirements(group))
                        {
                            foreach (var dataType in group.Include)
                            {
                                SaveTypeTerrain(dataType);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.AddEntry(
                        InfoLevels.Error,
                        () => new BaseLogEntry(_errorTypeJson, new Dictionary<string, object>() { { "file", file }, { "exception", e.Message } })
                        );
                }
            }
        }

        private void LoadPackageFeautre(DirectoryInfo root)
        {
            var jsonFiles = root.GetFiles("*.json", SearchOption.AllDirectories);
            foreach (var file in jsonFiles)
            {
                try
                {
                    string json = File.ReadAllText(file.FullName);
                    var groupList = JsonConvert.DeserializeObject<List<GroupDto<TerrainFeautreDto>>>(json, _serializationSettings);

                    if (groupList == null)
                    {
                        throw new FormatException();
                    }

                    foreach (var group in groupList)
                    {
                        if (CheckGroupRequirements(group))
                        {
                            foreach (var dataType in group.Include)
                            {
                                SaveTypeFeautre(dataType);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.AddEntry(
                        InfoLevels.Error,
                        () => new BaseLogEntry(_errorTypeJson, new Dictionary<string, object>() { { "file", file }, { "exception", e.Message } })
                        );
                }
            }
        }

        private void LoadPackageCollector(DirectoryInfo root)
        {
            var jsonFiles = root.GetFiles("*.json", SearchOption.AllDirectories);
            foreach (var file in jsonFiles)
            {
                try
                {
                    string json = File.ReadAllText(file.FullName);
                    var groupList = JsonConvert.DeserializeObject<List<GroupDto<CollectorDto>>>(json, _serializationSettings);

                    if (groupList == null)
                    {
                        throw new FormatException();
                    }

                    foreach (var group in groupList)
                    {
                        if (CheckGroupRequirements(group))
                        {
                            foreach (var dataType in group.Include)
                            {
                                SaveTypeCollector(dataType);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.AddEntry(
                        InfoLevels.Error,
                        () => new BaseLogEntry(_errorTypeJson, new Dictionary<string, object>() { { "file", file }, { "exception", e.Message } })
                        );
                }
            }
        }

        private void LoadPackageBuilding(DirectoryInfo root)
        {
            var jsonFiles = root.GetFiles("*.json", SearchOption.AllDirectories);
            foreach (var file in jsonFiles)
            {
                try
                {
                    string json = File.ReadAllText(file.FullName);
                    var groupList = JsonConvert.DeserializeObject<List<GroupDto<BuildingDto>>>(json, _serializationSettings);

                    if (groupList == null)
                    {
                        throw new FormatException();
                    }

                    foreach (var group in groupList)
                    {
                        if (CheckGroupRequirements(group))
                        {
                            foreach (var dataType in group.Include)
                            {
                                SaveTypeBuilding(dataType);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.AddEntry(
                        InfoLevels.Error,
                        () => new BaseLogEntry(_errorTypeJson, new Dictionary<string, object>() { { "file", file }, { "exception", e.Message } })
                        );
                }
            }
        }

        private void LoadPackageDemands(DirectoryInfo root)
        {
            var jsonFiles = root.GetFiles("*.json", SearchOption.AllDirectories);
            foreach (var file in jsonFiles)
            {
                try
                {
                    string json = File.ReadAllText(file.FullName);
                    var groupList = JsonConvert.DeserializeObject<List<GroupDto<DemandsDto>>>(json, _serializationSettings);

                    if (groupList == null)
                    {
                        throw new FormatException();
                    }

                    foreach (var group in groupList)
                    {
                        if (CheckGroupRequirements(group))
                        {
                            foreach (var dataType in group.Include)
                            {
                                SaveTypeDemands(dataType);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.AddEntry(
                        InfoLevels.Error,
                        () => new BaseLogEntry(_errorTypeJson, new Dictionary<string, object>() { { "file", file }, { "exception", e.Message } })
                        );
                }
            }
        }

        private Dictionary<string, JToken> LoadPackageConstants(DirectoryInfo root)
        {
            Dictionary<string, JToken> result = new();
            var jsonFiles = root.GetFiles("*.json", SearchOption.AllDirectories);
            foreach (var file in jsonFiles)
            {
                try
                {
                    string json = File.ReadAllText(file.FullName);
                    var constantList = JObject.Parse(json);
                    foreach (var constJson in constantList)
                    {
                        result[constJson.Key] = constJson.Value!;
                    }
                }
                catch (Exception e)
                {
                    _log.AddEntry(
                        InfoLevels.Error,
                        () => new BaseLogEntry(_errorTypeJson, new Dictionary<string, object>() { { "file", file }, { "exception", e.Message } })
                        );
                }
            }
            return result;
        }

        private bool CheckGroupRequirements<DataType>(GroupDto<DataType> group)
        {
            if (group.IfDef != null)
            {
                foreach (var id in group.IfDef)
                {
                    if (!TypeRepository.Contains(id))
                    {
                        return false;
                    }
                }
            }
            if (group.IfNotDef != null)
            {
                foreach (var id in group.IfNotDef)
                {
                    if (TypeRepository.Contains(id))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void SaveTypeResource(ResourceDto dataType)
        {
            ResourceType resource = new(dataType.Id, dataType.BasePrice, dataType.Tags);
            _typeRepository.ResourceTypes[dataType.Id] = resource;
        }

        private void SaveTypeTerrain(TerrainDto dataType)
        {
            List<ResourceDeposit> depositList = new();
            foreach (var depositDto in dataType.Deposits)
            {
                if (_typeRepository.ResourceTypes.ContainsKey(depositDto.Resource))
                {
                    ResourceDeposit deposit = new(_typeRepository.ResourceTypes[depositDto.Resource], depositDto.Richness, depositDto.Hardness);
                    depositList.Add(deposit);
                }
                else
                {
                    _log.AddEntry(InfoLevels.Warning, () => new BaseLogEntry(_errorMissingType, new Dictionary<string, object>() { { "container_id", dataType.Id }, { "missing_id", depositDto.Resource } }));
                }
            }
            TerrainType terrain = new(dataType.Id, depositList);
            _typeRepository.TerrainTypes[dataType.Id] = terrain;
        }

        private void SaveTypeFeautre(TerrainFeautreDto dataType)
        {
            List<ITerrainType> terrains = new();
            foreach (var terrainId in dataType.RequiredTerrain)
            {
                if (_typeRepository.TerrainTypes.ContainsKey(terrainId))
                {
                    terrains.Add(_typeRepository.TerrainTypes[terrainId]);
                }
                else
                {
                    _log.AddEntry(InfoLevels.Warning, () => new BaseLogEntry(_errorMissingType, new Dictionary<string, object>() { { "container_id", dataType.Id }, { "missing_id", terrainId } }));
                }
            }

            List<ResourceDeposit> depositList = new();
            foreach (var depositDto in dataType.Deposits)
            {
                if (_typeRepository.ResourceTypes.ContainsKey(depositDto.Resource))
                {
                    ResourceDeposit deposit = new(_typeRepository.ResourceTypes[depositDto.Resource], depositDto.Richness, depositDto.Hardness);
                    depositList.Add(deposit);
                }
                else
                {
                    _log.AddEntry(InfoLevels.Warning, () => new BaseLogEntry(_errorMissingType, new Dictionary<string, object>() { { "container_id", dataType.Id }, { "missing_id", depositDto.Resource } }));
                }
            }
            TerrainFeautre terrain = new(dataType.Id, terrains, depositList);
            _typeRepository.TerrainFeautreTypes[dataType.Id] = terrain;
        }

        private void SaveTypeCollector(CollectorDto dataType)
        {
            List<ITerrainType> terrains = new();
            foreach (var terrainId in dataType.RequiredTerrain)
            {
                if (_typeRepository.TerrainTypes.ContainsKey(terrainId))
                {
                    terrains.Add(_typeRepository.TerrainTypes[terrainId]);
                }
                else
                {
                    _log.AddEntry(InfoLevels.Warning, () => new BaseLogEntry(_errorMissingType, new Dictionary<string, object>() { { "container_id", dataType.Id }, { "missing_id", terrainId } }));
                }
            }

            List<ITerrainFeautre> feautres = new();
            foreach (var feautreId in dataType.RequiredFeautre)
            {
                if (_typeRepository.TerrainFeautreTypes.ContainsKey(feautreId))
                {
                    feautres.Add(_typeRepository.TerrainFeautreTypes[feautreId]);
                }
                else
                {
                    _log.AddEntry(InfoLevels.Warning, () => new BaseLogEntry(_errorMissingType, new Dictionary<string, object>() { { "container_id", dataType.Id }, { "missing_id", feautreId } }));
                }
            }

            List<IResourceType> resources = new();
            foreach (var resourceId in dataType.Collected)
            {
                if (_typeRepository.ResourceTypes.ContainsKey(resourceId))
                {
                    resources.Add(_typeRepository.ResourceTypes[resourceId]);
                }
                else
                {
                    _log.AddEntry(InfoLevels.Warning, () => new BaseLogEntry(_errorMissingType, new Dictionary<string, object>() { { "container_id", dataType.Id }, { "missing_id", resourceId } }));
                }
            }

            CollectorType collector = new(dataType.Id, dataType.BasePower, dataType.BaseLevel, terrains, feautres, resources);
            _typeRepository.CollectorTypes.Add(dataType.Id, collector);
        }

        private void SaveTypeBuilding(BuildingDto dataType)
        {
            Dictionary<IResourceType, double> input = new();
            foreach (var resourceDelta in dataType.Input)
            {
                if (_typeRepository.ResourceTypes.ContainsKey(resourceDelta.Resource))
                {
                    input[_typeRepository.ResourceTypes[resourceDelta.Resource]] = resourceDelta.Amount;
                }
                else
                {
                    _log.AddEntry(InfoLevels.Warning, () => new BaseLogEntry(_errorMissingType, new Dictionary<string, object>() { { "container_id", dataType.Id }, { "missing_id", resourceDelta.Resource } }));
                }
            }
            var zeroVector = new KeyedVectorFull<IResourceType>();
            var partialInput = zeroVector.FilterSmaller(input.Keys);
            foreach (var inputPair in input)
            {
                partialInput[inputPair.Key] = inputPair.Value;
            }

            Dictionary<IResourceType, double> output = new();
            foreach (var resourceDelta in dataType.Output)
            {
                if (_typeRepository.ResourceTypes.ContainsKey(resourceDelta.Resource))
                {
                    output[_typeRepository.ResourceTypes[resourceDelta.Resource]] = resourceDelta.Amount;
                }
                else
                {
                    _log.AddEntry(InfoLevels.Warning, () => new BaseLogEntry(_errorMissingType, new Dictionary<string, object>() { { "container_id", dataType.Id }, { "missing_id", resourceDelta.Resource } }));
                }
            }
            var partialOutput = zeroVector.FilterSmaller(output.Keys);
            foreach (var outputPair in output)
            {
                partialOutput[outputPair.Key] = outputPair.Value;
            }

            BuildingType building = new(dataType.Id, dataType.BaseLevel, partialInput, partialOutput);
            _typeRepository.BuildingTypes.Add(dataType.Id, building);
        }

        private void SaveTypeDemands(DemandsDto dataType)
        {
            Dictionary<IResourceType, double> resources = new();
            foreach (var resourceDelta in dataType.Demands)
            {
                if (_typeRepository.ResourceTypes.ContainsKey(resourceDelta.Resource))
                {
                    resources[_typeRepository.ResourceTypes[resourceDelta.Resource]] = resourceDelta.Amount;
                }
                else
                {
                    _log.AddEntry(InfoLevels.Warning, () => new BaseLogEntry(_errorMissingType, new Dictionary<string, object>() { { "container_id", "population" }, { "missing_id", resourceDelta.Resource } }));
                }
            }
            var zeroVector = new KeyedVectorFull<IResourceType>();
            var partialResources = zeroVector.FilterSmaller(resources.Keys);
            foreach (var inputPair in resources)
            {
                partialResources[inputPair.Key] = inputPair.Value;
            }

            _typeRepository.PopulationDemands[dataType.Level] = partialResources;
        }

        private void FillAllPopulationDemandsLevels()
        {
            int i = 0;
            while (i < _typeRepository.PopulationDemands.Count)
            {
                if (!_typeRepository.PopulationDemands.ContainsKey(i))
                {
                    if (i != 0)
                    {
                        _typeRepository.PopulationDemands[i] = _typeRepository.PopulationDemands[i - 1];
                    }
                    else
                    {
                        var vector = new KeyedVectorFull<IResourceType>();
                        _typeRepository.PopulationDemands[0] = vector.FilterSmaller(new List<ResourceType>());
                    }
                }
                i++;
            }
        }
    }
}
