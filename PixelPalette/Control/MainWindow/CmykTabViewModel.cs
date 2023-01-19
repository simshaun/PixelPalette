using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using PixelPalette.State;

namespace PixelPalette.Control.MainWindow;

public sealed partial class CmykTabViewModel : ObservableObject
{
    private GlobalState GlobalState { get; }

    [ObservableProperty] private string _text = "";
    [ObservableProperty] private string _scaledText = "";
    [ObservableProperty] private string _cyan = "";
    [ObservableProperty] private string _magenta = "";
    [ObservableProperty] private string _yellow = "";
    [ObservableProperty] private string _key = "";
    [ObservableProperty] private string _scaledCyan = "";
    [ObservableProperty] private string _scaledMagenta = "";
    [ObservableProperty] private string _scaledYellow = "";
    [ObservableProperty] private string _scaledKey = "";

    [ObservableProperty] private LinearGradientBrush? _cyanGradientFill;
    [ObservableProperty] private LinearGradientBrush? _magentaGradientFill;
    [ObservableProperty] private LinearGradientBrush? _yellowGradientFill;
    [ObservableProperty] private LinearGradientBrush? _keyGradientFill;

    public CmykTabViewModel(GlobalState globalState)
    {
        GlobalState = globalState;
        GlobalState.PropertyChanged += (_, ev) =>
        {
            if (ev.PropertyName == "Cmyk") RefreshValues();
        };
        RefreshValues();
    }

    private void RefreshValues()
    {
        _isUserUpdate = false;

        Text = GlobalState.Cmyk.ToString();
        ScaledText = GlobalState.Cmyk.ToScaledString();
        Cyan = GlobalState.Cmyk.RoundedCyan.ToString(CultureInfo.InvariantCulture);
        Magenta = GlobalState.Cmyk.RoundedMagenta.ToString(CultureInfo.InvariantCulture);
        Yellow = GlobalState.Cmyk.RoundedYellow.ToString(CultureInfo.InvariantCulture);
        Key = GlobalState.Cmyk.RoundedKey.ToString(CultureInfo.InvariantCulture);
        ScaledCyan = GlobalState.Cmyk.RoundedScaledCyan.ToString(CultureInfo.InvariantCulture);
        ScaledMagenta = GlobalState.Cmyk.RoundedScaledMagenta.ToString(CultureInfo.InvariantCulture);
        ScaledYellow = GlobalState.Cmyk.RoundedScaledYellow.ToString(CultureInfo.InvariantCulture);
        ScaledKey = GlobalState.Cmyk.RoundedScaledKey.ToString(CultureInfo.InvariantCulture);

        var cyanGradientFill = Window.MainWindow.NewBrush();
        var magentaGradientFill = Window.MainWindow.NewBrush();
        var yellowGradientFill = Window.MainWindow.NewBrush();
        var keyGradientFill = Window.MainWindow.NewBrush();

        cyanGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithCyan(0.0).ToRgb().ToMediaColor(), 0.0));
        cyanGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithCyan(1.0).ToRgb().ToMediaColor(), 1.0));
        magentaGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithMagenta(0.0).ToRgb().ToMediaColor(), 0.0));
        magentaGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithMagenta(1.0).ToRgb().ToMediaColor(), 1.0));
        yellowGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithYellow(0.0).ToRgb().ToMediaColor(), 0.0));
        yellowGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithYellow(1.0).ToRgb().ToMediaColor(), 1.0));
        keyGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithKey(0.0).ToRgb().ToMediaColor(), 0.0));
        keyGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithKey(1.0).ToRgb().ToMediaColor(), 1.0));

        CyanGradientFill = cyanGradientFill;
        MagentaGradientFill = magentaGradientFill;
        YellowGradientFill = yellowGradientFill;
        KeyGradientFill = keyGradientFill;

        _isUserUpdate = true;
    }

#region boilerplate

    private bool _isUserUpdate = true;
    public event EventHandler<PropertyChangedEventArgs>? PropertyChangedByUser;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (_isUserUpdate) PropertyChangedByUser?.Invoke(this, e);
    }

#endregion
}
