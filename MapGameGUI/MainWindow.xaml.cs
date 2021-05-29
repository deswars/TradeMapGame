using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TradeMapGame;
using TradeMapGame.Configuration;
using TradeMapGame.Localization;
using TradeMapGame.Map;

namespace MapGame.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private WriteableBitmap _writeableBitmap;
        private Engine _engine;
        private readonly Dictionary<string, Color> _terrainColors = new();
        private readonly Dictionary<string, Color> _feautreColors = new();
        private readonly int _scale = 7;
        private Color _settlementColor = Colors.Red;
        private Color _settlementBorder = Colors.Black;
        private readonly TextLocalizer textLocalizer = new();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RenderOptions.SetBitmapScalingMode(iMap, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(iMap, EdgeMode.Aliased);

            Matrix m = iMap.LayoutTransform.Value;
            m.ScaleAt(5, 5, 0, 0);
            iMap.LayoutTransform = new MatrixTransform(m);

            ConfigurationLoader conf = new();
            conf.Load(new DirectoryInfo("Data"));

            _engine = GameBuilder.BuildMap("exampleMap.json", "exampleMap.bmp", "exampleMapFeautres.bmp", conf);
            var map = _engine.Map;

            int width = map.Width * _scale;
            int height = map.Height * _scale;
            _terrainColors.Add("t_plains", Colors.Green);
            _terrainColors.Add("t_hills", Colors.Orange);
            _terrainColors.Add("t_mountains", Colors.DarkGray);

            _feautreColors.Add("f_forest", Colors.DarkGreen);
            _feautreColors.Add("f_gold", Colors.Yellow);
            _feautreColors.Add("f_metal", Colors.Brown);

            _writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
            iMap.Source = _writeableBitmap;

            DrawMap();
        }

        private void DrawMap()
        {
            SquareDiagonalMap map = _engine.Map;
            _writeableBitmap.Lock();
            for (int i = 0; i < map.Width; i++)
            {
                for (int k = 0; k < map.Height; k++)
                {
                    var color = _terrainColors[map[i, k].Terrain.Id];
                    DrawRectangle(_writeableBitmap, i * _scale, k * _scale, _scale, _scale, color);

                    if (map[i, k].MapFeautres.Count > 0)
                    {
                        var fcolor = _feautreColors[map[i, k].MapFeautres[0].Id];
                        DrawRectangle(_writeableBitmap, i * _scale + 1, k * _scale + 1, _scale - 2, _scale - 2, fcolor);
                    }
                    if (map[i, k].BuiltCollector != null)
                    {
                        DrawRectangle(_writeableBitmap, i * _scale + 2, k * _scale + 2, _scale - 4, _scale - 4, _settlementBorder);
                    }
                }
            }
            foreach (var settlement in _engine.Settlements)
            {
                DrawRectangle(_writeableBitmap, settlement.Position.X * _scale + 2, settlement.Position.Y * _scale + 2, _scale - 4, _scale - 4, _settlementBorder);
                DrawRectangle(_writeableBitmap, settlement.Position.X * _scale + 3, settlement.Position.Y * _scale + 3, _scale - 6, _scale - 6, _settlementColor);
            }

            _writeableBitmap.Unlock();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Matrix m = iMap.LayoutTransform.Value;
            if (e.Key == System.Windows.Input.Key.Q)
            {
                m.ScaleAt(1.5, 1.5, 0, 0);
            }
            else if (e.Key == System.Windows.Input.Key.W)
            {
                m.ScaleAt(1.0 / 1.5, 1.0 / 1.5, 0, 0);
            }
            iMap.LayoutTransform = new MatrixTransform(m);
        }

        private static void DrawRectangle(WriteableBitmap writeableBitmap, int left, int top, int width, int height, Color color)
        {
            // Compute the pixel's color
            int colorData = color.R << 16; // R
            colorData |= color.G << 8; // G
            colorData |= color.B << 0; // B
            int bpp = writeableBitmap.Format.BitsPerPixel / 8;

            unsafe
            {
                for (int y = 0; y < height; y++)
                {
                    // Get a pointer to the back buffer
                    int pBackBuffer = (int)writeableBitmap.BackBuffer;

                    // Find the address of the pixel to draw
                    pBackBuffer += (top + y) * writeableBitmap.BackBufferStride;
                    pBackBuffer += left * bpp;

                    for (int x = 0; x < width; x++)
                    {
                        // Assign the color data to the pixel
                        *((int*)pBackBuffer) = colorData;

                        // Increment the address of the pixel to draw
                        pBackBuffer += bpp;
                    }
                }
            }

            writeableBitmap.AddDirtyRect(new Int32Rect(left, top, width, height));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tbSettlements.Text = "";
            _engine.NextTurn();
            foreach (var settlement in _engine.Settlements)
            {
                tbSettlements.Text += "Settlement: X=" + settlement.Position.X + ",Y=" + settlement.Position.Y + "\n";
                tbSettlements.Text += "Population = " + settlement.Population + "\n";
                tbSettlements.Text += "Buildings : ";
                foreach (var building in settlement.Buildings)
                {
                    tbSettlements.Text += building.Type.Id + " ";
                }
                tbSettlements.Text += "\nCollectors : ";
                foreach (var collector in settlement.Collectors)
                {
                    tbSettlements.Text += collector.Type.Id + " ";
                }
                tbSettlements.Text += "\nResurces(stored, price) :\n";
                foreach (var resoure in settlement.Resources)
                {
                    tbSettlements.Text += resoure.Key.Id + ": " + resoure.Value.ToString("F") + ", " + settlement.Prices[resoure.Key].ToString("F") + "\n";
                }
                tbSettlements.Text += "\n\n";
            }
            DrawMap();
            WriteLog();
        }

        private void WriteLog()
        {
            tbLog.Text = "";
            var logEntries = _engine.Log.Entries;
            foreach (var entry in logEntries)
            {
                tbLog.Text += entry.ToText(textLocalizer) + "\n";
            }
        }
    }
}
