using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TradeMap.Core;
using TradeMap.Core.Map.ReadOnly;
using TradeMap.GameLog;
using TradeMap.Localization;

namespace TradeMap.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _windowTitle = "[MainWindow_Title]";
        private const string _btnNextTurn = "[MainWindow_NextTurn]";
        private const string _lbTurn = "[MainWindow_Turn]";


        private const string _configFile = "config.json";
        private Config? _conf;
        private SimulationMapBuilder? _builder;
        private ITextLocalizer _localizer;
        private MapEngine _eng;
        private IMap _map;
        private int _baseCellSize = 32;
        private GfxConfig? _confGfx;
        private GfxRepository _repo;

        public MainWindow()
        {

            Cursor = Cursors.Wait;
            string appName = Assembly.GetExecutingAssembly().GetName().Name!;
            try
            {
                string configStr = File.ReadAllText(_configFile);
                _conf = JsonSerializer.Deserialize<Config>(configStr);
                string configGfxStr = File.ReadAllText("Gfx/gfx.json");
                _confGfx = JsonSerializer.Deserialize<GfxConfig>(configGfxStr);
            }
            catch
            { }

            if ((_conf == null) || (_confGfx == null))
            {
                MessageBox.Show("Incorrect config file");
                Close();
            }

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _localizer = new TextLocalizer(new string[1] { _conf!.Localization });

            GameLogFile _log = new(appName, _conf.Log, InfoLevels.Warning);
            _builder = new(_log);
            _builder.SetSubstepCount(_conf.SubstepCount);
            NewMapWindow newMap = new NewMapWindow(_localizer);

            _repo = new();
            LoadGfx();

            InitializeComponent();

            newMap.ShowDialog();
            if (!newMap.Success)
            {
                Close();
            }
            _builder.SetMapSize(newMap.MapWidth, newMap.MapHeight);
            foreach(var package in _conf.Packages)
            {
                _builder.AddPackage(new DirectoryInfo(package));
            }
            _builder.Initialize();
            _eng = _builder.BuildEngine(new FileInfo(_conf.Feautres));
            _map = _eng.Map;
            _conf = null;
            _builder = null;
            _confGfx = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = _localizer.Expand(_windowTitle);
            BtnTurn.Content = _localizer.Expand(_btnNextTurn);
            LbTurn.Content = _localizer.Expand(_lbTurn);
            LbCurrentTurn.Content = 0;

            cnsMap.Children.Clear();
            cnsMap.Height = _map.Height * _baseCellSize;
            cnsMap.Width = _map.Width * _baseCellSize;
            DrawMap();
            Cursor = Cursors.Arrow;
        }

        private void DrawMap()
        {
            for (int i = 0; i < _map.Width; i++)
            {
                for (int k = 0; k < _map.Height; k++)
                {
                    CreateCell(_map[i, k]);
                }
            }
        }

        private void CreateCell(ICell cell)
        {
            Image imgCell = new Image();
            if (_repo.Terrains.TryGetValue(cell.Terrain.Id, out Image? img))
            {
                imgCell.Source = img.Source;
            }
            else
            {
                imgCell.Source = _repo.Empty32.Source;
            }
            imgCell.Height = _baseCellSize;
            imgCell.Width = _baseCellSize;
            cnsMap.Children.Add(imgCell);
            Canvas.SetLeft(imgCell, cell.Position.X * _baseCellSize);
            Canvas.SetTop(imgCell, cell.Position.Y * _baseCellSize);
        }

        private void BtnTurn_Click(object sender, RoutedEventArgs e)
        {
            _eng.NextTurn();
        }

        private void LoadGfx()
        {
            var config = _confGfx!;

            try
            {
                Image img = new();
                BitmapImage source = new BitmapImage();
                source.BeginInit();
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                source.UriSource = new Uri(config.Empty16, UriKind.Relative);
                source.EndInit();
                img.Source = source;
                _repo.Empty16 = img;
            }
            catch { }

            try
            {
                Image img = new();
                BitmapImage source = new BitmapImage();
                source.BeginInit();
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                source.UriSource = new Uri(config.Empty32, UriKind.Relative);
                source.EndInit();
                img.Source = source;
                _repo.Empty32 = img;
            }
            catch { }

            foreach(var resource in config.Resources)
            {
                try
                {
                    Image img = new();
                    BitmapImage source = new BitmapImage();
                    source.BeginInit();
                    source.CacheOption = BitmapCacheOption.OnLoad;
                    source.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    source.UriSource = new Uri(resource.Value, UriKind.Relative);
                    source.EndInit();
                    img.Source = source;
                    _repo.Resources[resource.Key] = img;
                }
                catch { }
            }

            foreach (var terrain in config.Terrains)
            {
                try
                {
                    Image img = new();
                    BitmapImage source = new BitmapImage();
                    source.BeginInit();
                    source.CacheOption = BitmapCacheOption.OnLoad;
                    source.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    source.UriSource = new Uri(terrain.Value, UriKind.Relative);
                    source.EndInit();
                    img.Source = source;
                    _repo.Terrains[terrain.Key] = img;
                }
                catch { }
            }
        }
    }
}
