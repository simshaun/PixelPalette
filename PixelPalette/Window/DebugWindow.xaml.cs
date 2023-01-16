namespace PixelPalette.Window;

/// <summary>
/// Interaction logic for DebugWindow.xaml
/// </summary>
public partial class DebugWindow
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