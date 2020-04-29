using System.Windows.Forms;
using System.Windows.Input;
using PixelPalette.Bitmap;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace PixelPalette.Window
{
    /// <summary>
    /// Interaction logic for FreezeFrameWindow.xaml
    /// </summary>
    public partial class FreezeFrameWindow : System.Windows.Window
    {
        private readonly MainWindowViewModel _vm;

        public FreezeFrameWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            FrozenImage.Source = FreezeFrame.Instance.BitmapSource;
            Top = 0;
            Left = 0;
            Width = SystemInformation.VirtualScreen.Width;
            Height = SystemInformation.VirtualScreen.Height;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouse = MouseUtil.GetMousePosition();
            var rgb = BitmapUtil.PixelToRgb(FreezeFrame.Instance.BitmapSource, mouse.X, mouse.Y);
            _vm.RefreshFromRgb(rgb);
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FrozenImage.Source = null;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape) return;
            Close();
        }
    }
}