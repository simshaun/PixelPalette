using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using PixelPalette.Color;
using PixelPalette.State;

namespace PixelPalette.Control.MainWindow;

public sealed partial class HsvTabViewModel : ObservableObject
{
    public GlobalState GlobalState { get; }

    [ObservableProperty] private string _text = "";
    [ObservableProperty] private string _scaledText = "";
    [ObservableProperty] private string _hue = "";
    [ObservableProperty] private string _saturation = "";
    [ObservableProperty] private string _value = "";
    [ObservableProperty] private string _scaledHue = "";
    [ObservableProperty] private string _scaledSaturation = "";
    [ObservableProperty] private string _scaledValue = "";

    [ObservableProperty] private LinearGradientBrush? _hueGradientFill;
    [ObservableProperty] private LinearGradientBrush? _saturationGradientFill;
    [ObservableProperty] private LinearGradientBrush? _valueGradientFill;

    public HsvTabViewModel(GlobalState globalState)
    {
        GlobalState = globalState;
        GlobalState.PropertyChanged += (_, ev) =>
        {
            if (ev.PropertyName == "Hsv") RefreshValues();
        };
        RefreshValues();
    }

    private void RefreshValues()
    {
        _isUserUpdate = false;

        Text = GlobalState.Hsv.ToString();
        ScaledText = GlobalState.Hsv.ToScaledString();
        Hue = GlobalState.Hsv.RoundedHue.ToString(CultureInfo.InvariantCulture);
        Saturation = GlobalState.Hsv.RoundedSaturation.ToString(CultureInfo.InvariantCulture);
        Value = GlobalState.Hsv.RoundedValue.ToString(CultureInfo.InvariantCulture);
        ScaledHue = GlobalState.Hsv.RoundedScaledHue.ToString(CultureInfo.InvariantCulture);
        ScaledSaturation = GlobalState.Hsv.RoundedScaledSaturation.ToString(CultureInfo.InvariantCulture);
        ScaledValue = GlobalState.Hsv.RoundedScaledValue.ToString(CultureInfo.InvariantCulture);

        var hueGradientFill = Window.MainWindow.NewBrush();
        var saturationGradientFill = Window.MainWindow.NewBrush();
        var valueGradientFill = Window.MainWindow.NewBrush();

        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(0, 100, 50).ToMediaColor(), 0.0));
        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(60, 100, 50).ToMediaColor(), 0.16));
        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(120, 100, 50).ToMediaColor(), 0.33));
        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(180, 100, 50).ToMediaColor(), 0.5));
        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(240, 100, 50).ToMediaColor(), 0.66));
        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(300, 100, 50).ToMediaColor(), 0.83));
        hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(360, 100, 50).ToMediaColor(), 1.0));

        saturationGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithSaturation(0.0).ToMediaColor(), 0.0));
        saturationGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithSaturation(1.0).ToMediaColor(), 1.0));

        valueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithValue(0.0).ToMediaColor(), 0.0));
        valueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithValue(0.5).ToMediaColor(), 0.5)); // Gives color
        valueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithValue(1.0).ToMediaColor(), 1.0));

        HueGradientFill = hueGradientFill;
        SaturationGradientFill = saturationGradientFill;
        ValueGradientFill = valueGradientFill;

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