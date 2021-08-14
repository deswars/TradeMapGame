using System.Collections.Generic;
using System.IO;
using System.Linq;
using TradeMap.Configuration;
using TradeMap.Core;
using TradeMap.Di;
using TradeMap.Engine.Map;
using TradeMap.GameLog;

namespace TradeMap.Gui
{
    public class SimulationMapBuilder
    {
        private readonly IGameLog _log;
        private int _height = 100;
        private int _width = 100;
        private int _substeps = 1;
        private readonly List<DirectoryInfo> _packages = new();

        private ConfigurationLoader? _conf;
        private SquareMap? _map;
        private MapEngine? _eng;
        private Manager? _di;


        public SimulationMapBuilder(IGameLog log)
        {
            _log = log;
        }


        public void SetMapSize(int width, int height)
        {
            _height = height;
            _width = width;
        }

        public void SetSubstepCount(int substepCount)
        {
            _substeps = substepCount;
        }

        public void AddPackage(DirectoryInfo root)
        {
            _packages.Add(root);
        }

        public void Initialize()
        {
            _conf = new ConfigurationLoader(_log);
            _map = new SquareMap(_width, _height, _conf.TypeRepository.TerrainTypes.First().Value);
            _eng = new MapEngine(_map, _log, _substeps);
            _di = new Manager(_log, _conf.TypeRepository, _eng);
        }

        public IReadOnlyDictionary<string, Service> FindAllAvailableServices()
        {
            if (_di == null)
            {
                return new Dictionary<string, Service>();
            }
            return _di.FindAllAvailableServices();
        }

        public MapEngine BuildEngine(FileInfo feautreFile)
        {
            if (_conf == null)
            {
                _conf = new ConfigurationLoader(_log);
            }
            if (_map == null)
            {
                _map = new SquareMap(_width, _height, _conf.TypeRepository.TerrainTypes.First().Value);
            }
            if (_eng == null)
            {
                _eng = new MapEngine(_map, _log, _substeps);
            }
            if (_di == null)
            {
                _di = new Manager(_log, _conf.TypeRepository, _eng);
            }

            var feautres = _conf.LoadFeautres(feautreFile);
            foreach (var x in feautres)
            {
                foreach (var y in x.Value)
                {
                    _ = _di.TryRegisterTurnAction(y.SubturnType, y.ServiceName, y.FeautreName, x.Key);
                }
            }

            foreach (var pack in _packages)
            {
                _conf.AddConfRoot(pack);
            }
            _conf.LoadPackages();

            var constants = _di.CollectConstantDemands();
            _conf.SetupConstants(constants);
            _di.CreateServicesAndSubscribeActions(constants);

            return _eng;
        }
    }
}
