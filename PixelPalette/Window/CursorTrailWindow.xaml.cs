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

namespace PixelPalette.Window;

/// <summary>
/// Interaction logic for CursorTrailWindow.xaml
/// </summary>
public partial class CursorTrailWindow
{
    // Must have odd number of columns to have a center column.
    private const int NumColumns = 9;
    private const int ColumnWidth = 13;
    private const int GridWidth = ColumnWidth * NumColumns + (NumColumns - 1);
    private const int SegmentWidth = ColumnWidth / 2;

    private DispatcherTimer? _timer;
    private byte[]? _outputBuffer;
    
    private int _lastCompensatedMouseX;
    private int _lastCompensatedMouseY;

    private bool _windowInitialized;

    public CursorTrailWindow()
    {
        InitializeComponent();
        
        // Timer will attach window to cursor then make window visible.
        Hide();

        var vm = new CursorTrailWindowViewModel();
        DataContext = vm;

        RenderOptions.SetBitmapScalingMode(PreviewImage, BitmapScalingMode.NearestNeighbor);
        const int winBorderWidth = 2;
        Width = winBorderWidth * 2 + GridWidth;
        PreviewContainer.Width = GridWidth;
        PreviewContainer.Height = GridWidth;

        DrawGridLines();
        DrawCrosshair();
        InitializeBuffer();
        SetupTimer(vm);
    }

    private void DrawGridLines()
    {
        var gridLineStyle = PreviewContainer.Resources["GridLine"] as Style;
        for (var x = 1; x < NumColumns; x += 1)
        {
            // vertical line
            PreviewContainer.Children.Add(new Line
            {
                Style = gridLineStyle,
                X1 = x * ColumnWidth + x,
                X2 = x * ColumnWidth + x,
                Y1 = 0,
                Y2 = GridWidth
            });

            // horizontal line
            PreviewContainer.Children.Add(new Line
            {
                Style = gridLineStyle,
                X1 = 0,
                X2 = GridWidth,
                Y1 = x * ColumnWidth + x,
                Y2 = x * ColumnWidth + x
            });
        }
    }

    private void DrawCrosshair()
    {
        var crosshairStyle = PreviewContainer.Resources["Crosshair"] as Style;
        const double pxCenter = GridWidth / 2.0;
        
        // vertical line crosshair
        PreviewContainer.Children.Add(new Line
        {
            Style = crosshairStyle,
            X1 = pxCenter,
            X2 = pxCenter,
            Y1 = pxCenter - SegmentWidth + 2,
            Y2 = pxCenter + SegmentWidth - 2
        });

        // horizontal line crosshair
        PreviewContainer.Children.Add(new Line
        {
            Style = crosshairStyle,
            X1 = pxCenter - SegmentWidth + 2,
            X2 = pxCenter + SegmentWidth - 2,
            Y1 = pxCenter,
            Y2 = pxCenter
        });
    }

    private void InitializeBuffer()
    {
        if (FreezeFrame.Instance.BitmapSource == null) return;
        var bytesPerPixel = FreezeFrame.Instance.BitmapSource.Format.BitsPerPixel / 8;
        var outputStride = NumColumns * bytesPerPixel;
        _outputBuffer = new byte[outputStride * NumColumns];
    }

    private void SetupTimer(CursorTrailWindowViewModel vm)
    {
        _lastCompensatedMouseX = -1;
        _lastCompensatedMouseY = -1;

        _timer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromMilliseconds(12) };
        _timer.Tick += (_, _) =>
        {
            var mouse = Mouse.GetMousePosition();
            const int pxFromCursor = 30;
            var winX = mouse.X + pxFromCursor;
            var winY = mouse.Y;
            var screenBounds = Screen.FromPoint(new Point(mouse.X, mouse.Y)).Bounds;

            if (winX + Width > screenBounds.Right) winX = mouse.X - pxFromCursor - (int) Width;
            if (winY + Height > screenBounds.Bottom) winY -= winY + (int) Height - screenBounds.Bottom;

            Left = winX;
            Top = winY;

            if (!_windowInitialized)
            {
                _windowInitialized = true;
                Show();
                Focus();
            }

            UpdatePreview(vm, mouse.X, mouse.Y);
        };
        _timer.Start();
    }

    private void UpdatePreview(CursorTrailWindowViewModel vm, int mouseX, int mouseY)
    {
        var sourceX = mouseX - (NumColumns - 1) / 2; // Make cursor the center
        var sourceY = mouseY - (NumColumns - 1) / 2; // Make cursor the center
        var compensatedX = sourceX - SystemInformation.VirtualScreen.Left; // Compensate for potential negative position on multi-monitor  
        var compensatedY = sourceY - SystemInformation.VirtualScreen.Top; // Compensate for potential negative position on multi-monitor

        if (compensatedX == _lastCompensatedMouseX && compensatedY == _lastCompensatedMouseY) return;
        _lastCompensatedMouseX = compensatedX;
        _lastCompensatedMouseY = compensatedY;

        if (FreezeFrame.Instance.BitmapSource == null) return;

        var previewImageSource = BitmapUtil.CropBitmapSource(
            FreezeFrame.Instance.BitmapSource,
            compensatedX, compensatedY,
            NumColumns, NumColumns,
            _outputBuffer
        );
        PreviewImage.Source = previewImageSource;

        var rgb = BitmapUtil.PixelToRgb(
            previewImageSource,
            (NumColumns - 1) / 2,
            (NumColumns - 1) / 2
        );

        if (vm.Rgb != null && vm.Rgb == rgb) return;
        vm.Rgb = rgb;
        vm.Hex = rgb.ToHex().ToString();
        vm.HexTextColor = rgb.ContrastingTextColor().ToHex().ToString();

        var averageRgb = BitmapUtil.AverageColor(previewImageSource);
        vm.GridLineColor = averageRgb.ContrastingTextColor().ToHex().ToString();
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