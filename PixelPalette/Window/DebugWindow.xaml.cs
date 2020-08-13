namespace PixelPalette.Window
{
    /// <summary>
    /// Interaction logic for DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow : System.Windows.Window
    {
        public DebugWindow(double initLeft, double initTop)
        {
            InitializeComponent();
            Left = initLeft;
            Top = initTop;

            var vm = new DebugWindowViewModel();
            DataContext = vm;
        }
    }
}