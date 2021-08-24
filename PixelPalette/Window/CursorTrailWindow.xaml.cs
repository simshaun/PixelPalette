using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using PixelPalette.Bitmap;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Mouse = PixelPalette.Util.Mouse;
using Point = System.Drawing.Point;

namespace PixelPalette.Window
{
    /// <summary>
    /// Interaction logic for CursorTrailWindow.xaml
    /// </summary>
    public partial class CursorTrailWindow
    {
        private DispatcherTimer? _timer;

        public CursorTrailWindow()
        {
            InitializeComponent();

            var vm = new CursorTrailWindowViewModel();
            DataContext = vm;

            RenderOptions.SetBitmapScalingMode(PreviewImage, BitmapScalingMode.NearestNeighbor);
            const int winBorderWidth = 2;

            // Must have odd number of columns to have a center column.
            const int numColumns = 9;
            const int columnWidth = 13;
            const double segmentWidth = (double) columnWidth / 2;
            const int gridWidth = columnWidth * numColumns + (numColumns - 1);

            const int winWidth = winBorderWidth * 2 + gridWidth;
            Width = winWidth;
            PreviewContainer.Width = gridWidth;
            PreviewContainer.Height = gridWidth;

#region Draw grid lines

            for (var x = 1; x < numColumns; x += 1)
            {
                var vLine = new Line
                {
                    Style = PreviewContainer.Resources["GridLine"] as Style,
                    X1 = x * columnWidth + x,
                    X2 = x * columnWidth + x,
                    Y1 = 0,
                    Y2 = gridWidth
                };
                PreviewContainer.Children.Add(vLine);

                var hLine = new Line
                {
                    Style = PreviewContainer.Resources["GridLine"] as Style,
                    X1 = 0,
                    X2 = gridWidth,
                    Y1 = x * columnWidth + x,
                    Y2 = x * columnWidth + x
                };
                PreviewContainer.Children.Add(hLine);
            }

            const double pxCenter = (double) gridWidth / 2;
            var vLineCrosshair = new Line
            {
                Style = PreviewContainer.Resources["Crosshair"] as Style,
                X1 = pxCenter,
                X2 = pxCenter,
                Y1 = pxCenter - segmentWidth + 2,
                Y2 = pxCenter + segmentWidth - 2
            };
            PreviewContainer.Children.Add(vLineCrosshair);

            var hLineCrosshair = new Line
            {
                Style = PreviewContainer.Resources["Crosshair"] as Style,
                X1 = pxCenter - segmentWidth + 2,
                X2 = pxCenter + segmentWidth - 2,
                Y1 = pxCenter,
                Y2 = pxCenter
            };
            PreviewContainer.Children.Add(hLineCrosshair);

#endregion


#region Render the color preview

            var winHeight = (int) Height;

            _timer = new DispatcherTimer(DispatcherPriority.Render) { Interval = TimeSpan.FromMilliseconds(12) };
            _timer.Tick += (_, _) =>
            {
                var mouse = Mouse.GetMousePosition();
                const int pxFromCursor = 30;
                var winX = mouse.X + pxFromCursor;
                var winY = mouse.Y;
                var screenBounds = Screen.FromPoint(new Point(mouse.X, mouse.Y)).Bounds;

                if (winX + winWidth > screenBounds.Right)
                {
                    winX = mouse.X - pxFromCursor - winWidth;
                }

                if (winY + winHeight > screenBounds.Bottom)
                {
                    winY -= winY + winHeight - screenBounds.Bottom;
                }

                Left = winX;
                Top = winY;

                //
                // Generate preview
                // 

                var sourceX = mouse.X - (numColumns - 1) / 2; // Make cursor the center
                var sourceY = mouse.Y - (numColumns - 1) / 2; // Make cursor the center
                var compensatedX = sourceX - SystemInformation.VirtualScreen.Left; // Compensate for potential negative position on multi-monitor  
                var compensatedY = sourceY - SystemInformation.VirtualScreen.Top; // Compensate for potential negative position on multi-monitor

                if (FreezeFrame.Instance.BitmapSource == null) return;

                var previewImageSource = BitmapUtil.CropBitmapSource(
                    FreezeFrame.Instance.BitmapSource,
                    compensatedX, compensatedY,
                    numColumns, numColumns,
                    FreezeFrame.Instance.PixelBuffer
                );
                PreviewImage.Source = previewImageSource;

                var rgb = BitmapUtil.PixelToRgb(
                    previewImageSource,
                    (numColumns - 1) / 2,
                    (numColumns - 1) / 2
                );
                vm.Hex = rgb.ToHex().ToString();
                vm.HexTextColor = rgb.ContrastingTextColor().ToHex().ToString();

                var averageRgb = BitmapUtil.AverageColor(previewImageSource);
                vm.GridLineColor = averageRgb.ContrastingTextColor().ToHex().ToString();
            };
            _timer.Start();

#endregion
        }

        private void Window_Closing(object sender, CancelEventArgs cancelEventArgs)
        {
            _timer?.Stop();
            _timer = null;
            PreviewImage.Source = null;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape) return;
            Close();
        }
    }
}
