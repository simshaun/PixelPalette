using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using PixelPalette.State;

namespace PixelPalette.Window;

public sealed class MainWindowViewModel : INotifyPropertyChanged
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

    private TabItem? _activeColorModelTabItem;
    private Brush? _colorPreviewBrush;

    public TabItem? ActiveColorModelTabItem
    {
        get => _activeColorModelTabItem;
        set => SetField(ref _activeColorModelTabItem, value);
    }

    public Brush? ColorPreviewBrush
    {
        get => _colorPreviewBrush;
        private set => SetField(ref _colorPreviewBrush, value);
    }

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

#region boilerplate

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }

#endregion
}