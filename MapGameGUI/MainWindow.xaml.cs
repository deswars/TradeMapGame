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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RenderOptions.SetBitmapScalingMode(iMap, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(iMap, EdgeMode.Aliased);

            Matrix m = iMap.LayoutTransform.Value;
            m.ScaleAt(2, 2, 0, 0);
            iMap.LayoutTransform = new MatrixTransform(m);

            int scale = 7;
            MapBuilder builder = new MapBuilder();
            Map map = builder.NewMap;
            int width = builder.Width * scale;
            int height = builder.Height * scale;
            Color[] terrainColors = new Color[3] { Colors.Green, Colors.Yellow, Colors.Brown };
            Color settlementColor = Colors.Red;

            _writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
            iMap.Source = _writeableBitmap;


            _writeableBitmap.Lock();
            for( int i = 0; i < map.Width; i++)
            {
                for (int k = 0; k < map.Height; k++)
                {
                    var color = terrainColors[map[i, k].Terrain.Id];
                    DrawRectangle(_writeableBitmap, i * scale + 1, k * scale + 1, scale - 2, scale - 2, color);
                }
            }
            foreach ( var settlement in map.Settlements)
            {
                DrawRectangle(_writeableBitmap, settlement.Position.X * scale + 2, settlement.Position.Y * scale + 2, scale - 4, scale - 4, settlementColor);
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
