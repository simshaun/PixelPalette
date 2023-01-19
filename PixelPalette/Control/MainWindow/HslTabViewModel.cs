using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using PixelPalette.Color;
using PixelPalette.State;

namespace PixelPalette.Control.MainWindow;

public sealed partial class HslTabViewModel : ObservableObject
{
    public GlobalState GlobalState { get; }

    [ObservableProperty] private string _text = "";
    [ObservableProperty] private string _scaledText = "";
    [ObservableProperty] private string _hue = "";
    [ObservableProperty] private string _saturation = "";
    [ObservableProperty] private string _luminance = "";
    [ObservableProperty] private string _scaledHue = "";
    [ObservableProperty] private string _scaledSaturation = "";
    [ObservableProperty] private string _scaledLuminance = "";

    [ObservableProperty] private LinearGradientBrush? _hueGradientFill;
    [ObservableProperty] private LinearGradientBrush? _saturationGradientFill;
    [ObservableProperty] private LinearGradientBrush? _luminanceGradientFill;

    public HslTabViewModel(GlobalState globalState)
    {
        GlobalState = globalState;
        GlobalState.PropertyChanged += (_, ev) =>
        {
            if (ev.PropertyName == "Hsl") RefreshValues();
        };
        RefreshValues();
    }

    private void RefreshValues()
    {
        _isUserUpdate = false;

        Text = GlobalState.Hsl.ToString();
        ScaledText = GlobalState.Hsl.ToScaledString();
        Hue = GlobalState.Hsl.RoundedHue.ToString(CultureInfo.InvariantCulture);
        Saturation = GlobalState.Hsl.RoundedSaturation.ToString(CultureInfo.InvariantCulture);
        Luminance = GlobalState.Hsl.RoundedLuminance.ToString(CultureInfo.InvariantCulture);
        ScaledHue = GlobalState.Hsl.RoundedScaledHue.ToString(CultureInfo.InvariantCulture);
        ScaledSaturation = GlobalState.Hsl.RoundedScaledSaturation.ToString(CultureInfo.InvariantCulture);
        ScaledLuminance = GlobalState.Hsl.RoundedScaledLuminance.ToString(CultureInfo.InvariantCulture);

        var hueGradientFill = Window.MainWindow.NewBrush();
        var hslSaturationGradientFill = Window.MainWindow.NewBrush();
        var hslLuminanceGradientFill = Window.MainWindow.NewBrush();

        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(0, 100, 50).ToMediaColor(), 0.0));
        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(60, 100, 50).ToMediaColor(), 0.16));
        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(120, 100, 50).ToMediaColor(), 0.33));
        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(180, 100, 50).ToMediaColor(), 0.5));
        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(240, 100, 50).ToMediaColor(), 0.66));
        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(300, 100, 50).ToMediaColor(), 0.83));
        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(360, 100, 50).ToMediaColor(), 1.0));

        hslSaturationGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsl.WithSaturation(0.0).ToMediaColor(), 0.0));
        hslSaturationGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsl.WithSaturation(1.0).ToMediaColor(), 1.0));

        hslLuminanceGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsl.WithLuminance(0.0).ToMediaColor(), 0.0));
        hslLuminanceGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsl.WithLuminance(0.5).ToMediaColor(), 0.5)); // Gives color
        hslLuminanceGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsl.WithLuminance(1.0).ToMediaColor(), 1.0));

        HueGradientFill = hueGradientFill;
        SaturationGradientFill = hslSaturationGradientFill;
        LuminanceGradientFill = hslLuminanceGradientFill;

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
