using System;
using System.ComponentModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using PixelPalette.Color;
using PixelPalette.State;

namespace PixelPalette.Control.MainWindow;

public sealed partial class LabTabViewModel : ObservableObject
{
    private GlobalState GlobalState { get; }

    [ObservableProperty] private string _text = "";
    [ObservableProperty] private double _l;
    [ObservableProperty] private double _a;
    [ObservableProperty] private double _b;

    [ObservableProperty] private LinearGradientBrush? _lGradientFill;
    [ObservableProperty] private LinearGradientBrush? _aGradientFill;
    [ObservableProperty] private LinearGradientBrush? _bGradientFill;

    public LabTabViewModel(GlobalState globalState)
    {
        GlobalState = globalState;
        GlobalState.PropertyChanged += (_, ev) =>
        {
            if (ev.PropertyName == "Lab") RefreshValues();
        };
        RefreshValues();
    }

    private void RefreshValues()
    {
        _isUserUpdate = false;

        Text = GlobalState.Lab.ToString();
        L = GlobalState.Lab.RoundedL;
        A = GlobalState.Lab.RoundedA;
        B = GlobalState.Lab.RoundedB;

        var lGradientFill = Window.MainWindow.NewBrush();
        var aGradientFill = Window.MainWindow.NewBrush();
        var bGradientFill = Window.MainWindow.NewBrush();

        lGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithL(Lab.MinL).ToRgb().ToMediaColor(), 0.0));
        lGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithL(Lab.MaxL).ToRgb().ToMediaColor(), 1.0));
        aGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithA(Lab.MinA).ToRgb().ToMediaColor(), 0.0));
        aGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithA(Lab.MaxA).ToRgb().ToMediaColor(), 1.0));
        bGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithB(Lab.MinB).ToRgb().ToMediaColor(), 0.0));
        bGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithB(Lab.MaxB).ToRgb().ToMediaColor(), 1.0));

        LGradientFill = lGradientFill;
        AGradientFill = aGradientFill;
        BGradientFill = bGradientFill;

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
