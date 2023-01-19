using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using PixelPalette.State;

namespace PixelPalette.Control.MainWindow;

public sealed partial class RgbTabViewModel : ObservableObject
{
    private GlobalState GlobalState { get; }

    [ObservableProperty] private string _text = "";
    [ObservableProperty] private string _scaledText = "";
    [ObservableProperty] private string _redText = "";
    [ObservableProperty] private string _greenText = "";
    [ObservableProperty] private string _blueText = "";
    [ObservableProperty] private string _scaledRedText = "";
    [ObservableProperty] private string _scaledGreenText = "";
    [ObservableProperty] private string _scaledBlueText = "";

    [ObservableProperty] private double _r;
    [ObservableProperty] private double _g;
    [ObservableProperty] private double _b;

    [ObservableProperty] private LinearGradientBrush? _redGradientFill;
    [ObservableProperty] private LinearGradientBrush? _greenGradientFill;
    [ObservableProperty] private LinearGradientBrush? _blueGradientFill;

    public RgbTabViewModel(GlobalState globalState)
    {
        GlobalState = globalState;
        GlobalState.PropertyChanged += (_, ev) =>
        {
            if (ev.PropertyName == "Rgb") RefreshValues();
        };
        RefreshValues();
    }

    private void RefreshValues()
    {
        _isUserUpdate = false;

        Text = GlobalState.Rgb.ToString();
        ScaledText = GlobalState.Rgb.ToScaledString();
        RedText = GlobalState.Rgb.RoundedRed.ToString(CultureInfo.InvariantCulture);
        GreenText = GlobalState.Rgb.RoundedGreen.ToString(CultureInfo.InvariantCulture);
        BlueText = GlobalState.Rgb.RoundedBlue.ToString(CultureInfo.InvariantCulture);
        ScaledRedText = GlobalState.Rgb.ScaledRed.ToString();
        ScaledGreenText = GlobalState.Rgb.ScaledGreen.ToString();
        ScaledBlueText = GlobalState.Rgb.ScaledBlue.ToString();
        R = GlobalState.Rgb.Red;
        G = GlobalState.Rgb.Green;
        B = GlobalState.Rgb.Blue;

        var redGradientFill = Window.MainWindow.NewBrush();
        var greenGradientFill = Window.MainWindow.NewBrush();
        var blueGradientFill = Window.MainWindow.NewBrush();

        redGradientFill.GradientStops.Add(new GradientStop(GlobalState.Rgb.WithRed(0.0).ToMediaColor(), 0.0));
        redGradientFill.GradientStops.Add(new GradientStop(GlobalState.Rgb.WithRed(1.0).ToMediaColor(), 1.0));
        greenGradientFill.GradientStops.Add(new GradientStop(GlobalState.Rgb.WithGreen(0.0).ToMediaColor(), 0.0));
        greenGradientFill.GradientStops.Add(new GradientStop(GlobalState.Rgb.WithGreen(1.0).ToMediaColor(), 1.0));
        blueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Rgb.WithBlue(0.0).ToMediaColor(), 0.0));
        blueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Rgb.WithBlue(1.0).ToMediaColor(), 1.0));

        RedGradientFill = redGradientFill;
        GreenGradientFill = greenGradientFill;
        BlueGradientFill = blueGradientFill;

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
