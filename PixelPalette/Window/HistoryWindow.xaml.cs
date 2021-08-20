using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;

namespace PixelPalette.Window
{
    public partial class HistoryWindow
    {
        private readonly HistoryWindowViewModel _vm;

        public HistoryWindow(HistoryWindowViewModel vm, double initLeft, double initTop)
        {
            InitializeComponent();
            Left = initLeft;
            Top = initTop;

            _vm = vm;
            DataContext = _vm;

            var dt = new DispatcherTimer(
                new TimeSpan(0, 0, 1),
                DispatcherPriority.Normal, (sender, args) =>
                {
                    if (_vm.States.Count != 0 && _vm.States.First().Color.ToRgb().Equals(GlobalState.Rgb)) return;
                    _vm.States.Insert(0, new HistoryItem(GlobalState.Rgb));
                },
                Application.Current.Dispatcher
            );
            dt.Start();
        }

        private void HistoryWindow_OnSourceInitialized(object sender, EventArgs e)
        {
            var hWnd = new WindowInteropHelper((System.Windows.Window) sender).Handle;
            WindowHelper.DisableMaximize(hWnd);
        }

        private bool _ignoreSelectionFlag = false;
        public event EventHandler<HistoryItemSelectedEventArgs> HistoryItemSelected;

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

            HistoryItemSelected?.Invoke(this, new HistoryItemSelectedEventArgs
            {
                HistoryItem = (HistoryItem) e.AddedItems[0]
            });
        }
    }

    public class HistoryItemSelectedEventArgs : EventArgs
    {
        public HistoryItem HistoryItem { get; set; }
    }
}
