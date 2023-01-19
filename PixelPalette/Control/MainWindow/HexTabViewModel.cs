using System;
using System.ComponentModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using PixelPalette.State;

namespace PixelPalette.Control.MainWindow;

public sealed partial class HexTabViewModel : ObservableObject
{
    public GlobalState GlobalState { get; }

    [ObservableProperty] private string _text = "";
    [ObservableProperty] private string _red = "";
    [ObservableProperty] private string _green = "";
    [ObservableProperty] private string _blue = "";

    [ObservableProperty] private LinearGradientBrush? _redGradientFill;
    [ObservableProperty] private LinearGradientBrush? _greenGradientFill;
    [ObservableProperty] private LinearGradientBrush? _blueGradientFill;

    public HexTabViewModel(GlobalState globalState)
    {
        GlobalState = globalState;
        GlobalState.PropertyChanged += (_, ev) =>
        {
            if (ev.PropertyName == "Hex") RefreshValues();
        };
        RefreshValues();
    }

    private void RefreshValues()
    {
        _isUserUpdate = false;

        Text = GlobalState.Hex.ToString();
        Red = GlobalState.Hex.RedPart;
        Green = GlobalState.Hex.GreenPart;
        Blue = GlobalState.Hex.BluePart;

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
