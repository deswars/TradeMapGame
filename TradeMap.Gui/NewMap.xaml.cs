using System.Windows;
using TradeMap.Localization;

namespace TradeMap.Gui
{
    /// <summary>
    /// Interaction logic for NewMap.xaml
    /// </summary>
    public partial class NewMapWindow : Window
    {
        private static string _windowTitle = "[NewMapWindow_Title]";
        private static string _lbWidthText = "[NewMapWindow_Width]";
        private static string _lbHeightText = "[NewMapWindow_Height]";
        private static string _btnCreateText = "[NewMapWindow_Create]";

        private static string _incorrectWidth = "[IncorrectWidth]";
        private static string _incorrectHeight = "[IncorrectHeight]";


        public int MapHeight { get; set; }
        public int MapWidth { get; set; }
        public bool Success { get; set; }

        private ITextLocalizer _localizer;


        public NewMapWindow(ITextLocalizer textLocalizer)
        {
            _localizer = textLocalizer;
            InitializeComponent();
        }

        private void BtmCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TbWidth.Text, out int mapWidth))
            {
                MessageBox.Show(_localizer.Expand(_incorrectWidth));
            }
            else if (!int.TryParse(TbWidth.Text, out int mapHeight))
            {
                MessageBox.Show(_localizer.Expand(_incorrectHeight));
            }
            else
            {
                MapWidth = mapWidth;
                MapHeight = mapHeight;
                Success = true;
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = _localizer.Expand(_windowTitle);
            BtnCreate.Content = _localizer.Expand(_btnCreateText);
            LbWidth.Content = _localizer.Expand(_lbWidthText);
            LbHeight.Content = _localizer.Expand(_lbHeightText);
        }
    }
}
