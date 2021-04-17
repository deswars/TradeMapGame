using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MapGame.Core;
using MapGame.SquareMap;

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
        private SquareMap.Map _map; 

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RenderOptions.SetBitmapScalingMode(iMap, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(iMap, EdgeMode.Aliased);

            Matrix m = iMap.LayoutTransform.Value;
            m.ScaleAt(2, 2, 0, 0);
            iMap.LayoutTransform = new MatrixTransform(m);


            int width = 100;
            int height = 100;
            _writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
            iMap.Source = _writeableBitmap;

            List<TerraitType> terrains = new();

            List<Modifier> terrain1Mod = new()
            {
                new Modifier(ModifierTypes.Speed, 1)
            };
            TerraitType terrain1 = new(0, MoveClasses.FreeMovement, terrain1Mod);
            terrains.Add(terrain1);

            List<Modifier> terrain2Mod = new()
            {
                new Modifier(ModifierTypes.Speed, 2)
            };
            TerraitType terrain2 = new(1, MoveClasses.FreeMovement | MoveClasses.GroundBlocked, terrain2Mod);
            terrains.Add(terrain2);

            _map = new SquareMap.Map(100, 100, terrain1);
            _map[1, 1].ChangeTerrain(terrain2);
            _map[1, 2].ChangeTerrain(terrain2);
            _map[1, 3].ChangeTerrain(terrain2);
            _map[2, 1].ChangeTerrain(terrain2);
            _map[2, 2].ChangeTerrain(terrain2);
            _map[3, 1].ChangeTerrain(terrain2);

            Color[] terrainColors = new Color[2] { Colors.Green, Colors.Brown};

            _writeableBitmap.Lock();
            for( int i = 0; i < _map.Width; i++)
            {
                for (int k = 0; k < _map.Height; k++)
                {
                    var color = terrainColors[_map[i, k].Terrain.Id];
                    DrawRectangle(_writeableBitmap, i, k, 1, 1, color);
                }
            }
            _writeableBitmap.Unlock();
        }

         private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Matrix m = iMap.LayoutTransform.Value;
            if (e.Key == System.Windows.Input.Key.Q)
            {
                m.ScaleAt(
                    1.5,
                    1.5,
                    0,
                    0);
            }
            else if (e.Key == System.Windows.Input.Key.W)
                    {
                m.ScaleAt(
                    1.0 / 1.5,
                    1.0 / 1.5,
                    0,
                    0);
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
    }
}
