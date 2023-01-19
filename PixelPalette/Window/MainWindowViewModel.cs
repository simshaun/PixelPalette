using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using PixelPalette.State;

namespace PixelPalette.Window;

public sealed partial class MainWindowViewModel : ObservableObject
{
    public GlobalState GlobalState { get; }

    public MainWindowViewModel(GlobalState globalState)
    {
        GlobalState = globalState;
        GlobalState.PropertyChanged += (_, ev) =>
        {
            if (ev.PropertyName == "Rgb")
            {
                RefreshValues();
            }
        };
    }

    [ObservableProperty] private TabItem? _activeColorModelTabItem;
    [ObservableProperty] private Brush? _colorPreviewBrush;

    public void LoadFromPersistedData(PersistedData data, TabControl colorModelTabs)
    {
        ActiveColorModelTabItem = colorModelTabs
                                  .Items
                                  .OfType<TabItem>()
                                  .SingleOrDefault(t => t.Name == (data.ActiveColorModelTab ?? "RgbTab"));
    }

    private void RefreshValues()
    {
        ColorPreviewBrush = new SolidColorBrush(GlobalState.Rgb.ToMediaColor());
    }
}
