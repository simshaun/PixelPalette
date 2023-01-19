using CommunityToolkit.Mvvm.ComponentModel;
using PixelPalette.Color;

namespace PixelPalette.Window;

public sealed partial class CursorTrailWindowViewModel : ObservableObject
{
    [ObservableProperty] private Rgb? _rgb;
    [ObservableProperty] private string _hex = "";
    [ObservableProperty] private string _hexTextColor = "";
    [ObservableProperty] private string _gridLineColor = "";
}