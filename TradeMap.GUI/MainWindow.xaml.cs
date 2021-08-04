using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TradeMap.Configuration;
using TradeMap.Core;
using TradeMap.Core.Map;
using TradeMap.Di;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMap.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
#pragma warning disable CS8618
        public MainWindow()
        {
            InitializeComponent();
        }
#pragma warning restore CS8618

        private GuiConfig _guiConfig;
        private Manager<Engine, TypeRepository> _diManager;
        private ConfigurationLoader _conf;
        private ITextLocalizer _localizer;
        private IReadOnlyDictionary<string, Service> _availableServices;
        private TypeRepository _types;
        private SquareDiagonalMap _map;
        private Engine _eng;

        private readonly IGameLog _systemLog = new GameLogImpl();
        private readonly IGameLog _gameLog = new GameLogImpl();
        private readonly SpriteRepository _sprites = new();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string configText = File.ReadAllText("Gfx/Config.json");
            _guiConfig = JsonConvert.DeserializeObject<GuiConfig>(configText)!;
            _sprites.EmptySprite.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Gfx\\" + _guiConfig.EmptySprite, UriKind.Absolute));
            _sprites.EmptyIcon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Gfx\\" + _guiConfig.EmptyIcon, UriKind.Absolute));
            _sprites.EmptyIcon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Gfx\\Settlement\\" + _guiConfig.DefaultSettlementSprite, UriKind.Absolute));

            _localizer = new BasicTextLocalizer(CultureInfo.InvariantCulture);
            _systemLog.SetInfoLevel(InfoLevels.Error | InfoLevels.Warning);
            _gameLog.SetInfoLevel(InfoLevels.All);

            _conf = new ConfigurationLoader(_systemLog);
            _conf.AddConfRoot(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Data\\"));

            UpdateLocalization();
        }

        private void UpdateLocalization()
        {
            mLoadConf.Header = _localizer.Expand("[MenuLoadConfig]");
            mOutput.Header = _localizer.Expand("[MenuOutput]");
            mGenerateMap.Header = _localizer.Expand("[MenuGenerateMap]");
        }

        private void MLoadConf_Click(object sender, RoutedEventArgs e)
        {
            _types = _conf.LoadTypes();
            LoadGfx(_types);
            _diManager = new Manager<Engine, TypeRepository>(_systemLog, _gameLog, _types);

            _availableServices = _diManager.CollectAllAvailableServices();
            var steps = ConfigurationLoader.LoadOrderedTurnActions(AppDomain.CurrentDomain.BaseDirectory + "Data\\EngineSteps.json");
            foreach (var step in steps)
            {
                _diManager.TryRegisterTurnAction(step.EventName, step.ServiceName, step.ActionName);
            }

            mOutput.IsEnabled = true;
            mLoadConf.IsEnabled = false;
            mGenerateMap.IsEnabled = true;
        }

        private void MOutput_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDlg = new();
            saveDlg.Filter = _localizer.Expand("[SaveServiceInfo]|*.json");
            if (saveDlg.ShowDialog() == true)
            {
                ConfigurationLoader.WriteAvailableServiceJson(saveDlg.FileName, _availableServices);
            }
        }

        private void MGenerateMap_Click(object sender, RoutedEventArgs e)
        {
            _map = BasicMapGenerator.Generate(100, 100, _types);
            _eng = new Engine(_map, _gameLog);
            var constants = _diManager.CollectConstantDemands();
            constants = _conf.LoadConstants(constants);
            _diManager.CreateServicesAndSubscribeActions(_eng, constants);

            FullDrawMap();
        }

        private void LoadGfx(TypeRepository types)
        {
            _sprites.Terrain = LoadTypeGfx(types.TerrainTypes.Keys, AppDomain.CurrentDomain.BaseDirectory + "Gfx\\Terrain\\", _sprites.EmptySprite);
            _sprites.MapFeautre = LoadTypeGfx(types.TerrainTypes.Keys, AppDomain.CurrentDomain.BaseDirectory + "Gfx\\MapFeautres\\", _sprites.EmptySprite);
            _sprites.Collector = LoadTypeGfx(types.TerrainTypes.Keys, AppDomain.CurrentDomain.BaseDirectory + "Gfx\\Collectors\\", _sprites.EmptySprite);
            _sprites.Building = LoadTypeGfx(types.TerrainTypes.Keys, AppDomain.CurrentDomain.BaseDirectory + "Gfx\\Buildings\\", _sprites.EmptySprite);
        }

        private static IReadOnlyDictionary<string, Image> LoadTypeGfx(IEnumerable<string> idList, string root, Image emptySprite)
        {
            Dictionary<string, Image> result = new();
            foreach (var id in idList)
            {
                FileInfo file = new FileInfo(root + id + ".png");
                Image img;
                if (file.Exists)
                {
                    img = new();
                    img.Source = new BitmapImage(new Uri(file.FullName, UriKind.Absolute));
                }
                else
                {
                    img = emptySprite;
                }
                result.Add(id, img);
            }
            return result;
        }

        private void FullDrawMap()
        {
            for (int i = 0; i < _map.Width; i++)
            {
                for (int k = 0; k < _map.Height; k++)
                {
                    //TODO GUI
                }
            }
        }
    }
}
