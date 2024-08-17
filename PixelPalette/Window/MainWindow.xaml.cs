using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelPalette.Bitmap;
using PixelPalette.Color;
using PixelPalette.State;
using PixelPalette.Util;

namespace PixelPalette.Window;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private FreezeFrameWindow? _freezeFrameWin;
    private CursorTrailWindow? _cursorTrailWin;
    private readonly HistoryWindow _historyWin;
    private readonly GlobalState _globalState;
    private readonly MainWindowViewModel _vm;

    public MainWindow()
    {
        InitializeComponent();
        Style = (Style) FindResource(typeof(System.Windows.Window));

        _globalState = GlobalState.Instance;
        _vm = new MainWindowViewModel(_globalState);

        _globalState.LoadFromPersistedData(PersistedState.Data());
        _vm.LoadFromPersistedData(PersistedState.Data(), ColorModelTabs);
        DataContext = _vm;

        if (_globalState.Rgb == Rgb.Empty)
        {
            // Material Blue – #2196F3
            _globalState.RefreshFromRgb(Rgb.FromScaledValues(33, 150, 243));
        }

        // Eyedropper
        EyedropperButton.Click += (_, _) => { StartEyedropper(); };

        // Lighter / Darker Buttons
        LighterButton.Click += (_, _) => { _globalState.RefreshFromHsl(_globalState.Hsl.Lighter(5)); };
        DarkerButton.Click += (_, _) => { _globalState.RefreshFromHsl(_globalState.Hsl.Darker(5)); };

        // History Button
        _historyWin = new HistoryWindow(_globalState, Left + Width, Top);
        _historyWin.HistoryItemSelected += (_, args) => { _globalState.RefreshFromRgb(args.HistoryItem.Color.ToRgb()); };
        HistoryButton.Click += (_, _) =>
        {
            if (_historyWin.IsVisible)
            {
                _historyWin.Hide();
                _globalState.HistoryVisible = false;
            }
            else
            {
                RepositionHistoryWin();
                _historyWin.Show();
                _globalState.HistoryVisible = true;
            }
        };

        // Remember what tab is selected
        _vm.PropertyChanged += (_, ev) =>
        {
            if (ev.PropertyName == "ActiveColorModelTabItem")
            {
                _globalState.SaveToPersistedData(PersistedState.Data(), _vm.ActiveColorModelTabItem?.Name);
            }
        };

        // Hack to bring window to front on start. (I had instances where I would open the tool
        // only for it to come up behind my current window.)
        if (!IsVisible) Show();
        WindowState = WindowState.Normal;
        Activate();
        Topmost = true;
        Topmost = false;
        Focus();
    }

    private void StartEyedropper()
    {
        FreezeFrame.Instance.BitmapSource = new WriteableBitmap(ScreenCapture.GetFullScreen());

        // Debug - Write freeze frame to file
        // using (var fileStream = new FileStream("cap.png", FileMode.Create))
        // {
        //     BitmapEncoder encoder = new PngBitmapEncoder();
        //     encoder.Frames.Add(BitmapFrame.Create(FreezeFrame.Instance.BitmapSource));
        //     encoder.Save(fileStream);
        // }

        _freezeFrameWin = new FreezeFrameWindow(_vm);
        _freezeFrameWin.ColorPicked += (_, _) =>
        {
            if (_vm.ActiveColorModelTabItem == null) return;
            var el = WpfHelper.FindFirstVisualChild<TextBox>(_vm.ActiveColorModelTabItem);
            el?.Focus();
            el?.SelectAll();
        };
        _freezeFrameWin.Show();
        _freezeFrameWin.Focus();
        _freezeFrameWin.Closed += (_, _) =>
        {
            try
            {
                _cursorTrailWin?.Close();
            }
            catch (InvalidOperationException)
            {
                // Already closed.
            }
                
            _cursorTrailWin = null;
            _freezeFrameWin = null;
            FreezeFrame.Instance.Dispose();
        };

        _cursorTrailWin = new CursorTrailWindow();
        _cursorTrailWin.Closed += (_, _) =>
        {
            try
            {
                _freezeFrameWin?.Close();
            }
            catch (InvalidOperationException)
            {
                // Already closed.
            }
        };
    }

    private void OnColorModelTabSelected(object sender, RoutedEventArgs e)
    {
        PersistedState.Data().ActiveColorModelTab = ((TabItem) e.Source).Name;
    }

    //
    // -------------
    //

    private void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (e.Key)
        {
            case Key.Escape:
                _freezeFrameWin?.Close();
                Close();
                break;
            case Key.F3:
                StartEyedropper();
                break;
        }
    }

    // This allows inputs to lose focus when the user clicks outside of them:
    private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        MainGrid.Focus();
    }

    private void MainWindow_OnClosing(object sender, CancelEventArgs e)
    {
        PersistedState.Save();
    }

    private void MainWindow_SourceInitialized(object sender, EventArgs e)
    {
        var hWnd = new WindowInteropHelper((System.Windows.Window) sender).Handle;
        WindowHelper.DisableMaximize(hWnd);
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (!EphemeralState.Data.DebugMode) return;
        var debugWin = new DebugWindow(Left + Width, Top);
        debugWin.Show();
    }

    private void MainWindow_OnStateChanged(object sender, EventArgs e)
    {
        if (WindowState != WindowState.Normal) return;
        if (_globalState.HistoryVisible) _historyWin.Focus();
    }

    private void MainWindow_OnLocationChanged(object sender, EventArgs e)
    {
        RepositionHistoryWin();
    }

    private void RepositionHistoryWin()
    {
        _historyWin.Left = Left + Width;
        _historyWin.Top = Top;
    }

    public static LinearGradientBrush NewBrush() =>
        new()
        {
            StartPoint = new Point(0, .5),
            EndPoint = new Point(1, .5)
        };
}