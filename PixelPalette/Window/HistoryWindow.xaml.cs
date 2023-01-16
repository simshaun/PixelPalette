using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Interop;
using PixelPalette.State;
using PixelPalette.Util;

namespace PixelPalette.Window;

public partial class HistoryWindow
{
    public HistoryWindow(GlobalState globalState, double initLeft, double initTop)
    {
        InitializeComponent();
        Left = initLeft;
        Top = initTop;

        var vm = new HistoryWindowViewModel();
        DataContext = vm;

        void AddToHistory(object? _)
        {
            if (vm.States.Count != 0 && vm.States.Last().Color.ToRgb().Equals(globalState.Rgb)) return;
            vm.States.Add(new HistoryItem(globalState.Rgb));
        }

        AddToHistory(null);
        var debouncer = new Debouncer();
        globalState.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == "Rgb")
            {
                debouncer.Debounce(TimeSpan.FromMilliseconds(1000), AddToHistory);
            }
        };
    }

    private void HistoryWindow_OnSourceInitialized(object sender, EventArgs e)
    {
        var hWnd = new WindowInteropHelper((System.Windows.Window) sender).Handle;
        WindowHelper.DisableMaximize(hWnd);
        WindowHelper.DisableAltTab(hWnd);
    }

    private bool _ignoreSelectionFlag;
    public event EventHandler<HistoryItemSelectedEventArgs>? HistoryItemSelected;

    private void History_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_ignoreSelectionFlag) return;
        if (e.AddedItems.Count <= 0) return;

        // Hacky to allow de-selecting
        _ignoreSelectionFlag = true;
        History.UnselectAll();
        History.SelectedItems.Add(e.AddedItems[0]);
        e.Handled = true;
        _ignoreSelectionFlag = false;

        HistoryItemSelected?.Invoke(this, new HistoryItemSelectedEventArgs(e.AddedItems[0] as HistoryItem ?? throw new InvalidOperationException()));
    }
}

public class HistoryItemSelectedEventArgs : EventArgs
{
    public HistoryItemSelectedEventArgs(HistoryItem historyItem)
    {
        HistoryItem = historyItem;
    }

    public HistoryItem HistoryItem { get; }
}