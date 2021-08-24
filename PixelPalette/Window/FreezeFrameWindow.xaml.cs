using System;
using System.Windows.Forms;
using System.Windows.Input;
using PixelPalette.Bitmap;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Mouse = PixelPalette.Util.Mouse;

namespace PixelPalette.Window
{
    /// <summary>
    /// Interaction logic for FreezeFrameWindow.xaml
    /// </summary>
    public partial class FreezeFrameWindow
    {
        public event EventHandler? ColorPicked;

        private readonly MainWindowViewModel _vm;

        public FreezeFrameWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            FrozenImage.Source = FreezeFrame.Instance.BitmapSource;
            Top = SystemInformation.VirtualScreen.Top;
            Left = SystemInformation.VirtualScreen.Left;
            Width = SystemInformation.VirtualScreen.Width;
            Height = SystemInformation.VirtualScreen.Height;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouse = Mouse.GetMousePosition();
            var compensatedX = mouse.X - SystemInformation.VirtualScreen.Left; // Compensate for potential negative position on multi-monitor  
            var compensatedY = mouse.Y - SystemInformation.VirtualScreen.Top; // Compensate for potential negative position on multi-monitor 
            if (FreezeFrame.Instance.BitmapSource != null)
            {
                var rgb = BitmapUtil.PixelToRgb(FreezeFrame.Instance.BitmapSource, compensatedX, compensatedY);
                _vm.GlobalState.RefreshFromRgb(rgb);
            }

            ColorPicked?.Invoke(this, EventArgs.Empty);
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