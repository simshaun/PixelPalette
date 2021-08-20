using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using PixelPalette.Annotations;
using PixelPalette.Color;

namespace PixelPalette.Window
{
    public class HistoryItem
    {
        public Brush ColorBrush { get; }
        public string ColorString { get; }
        public IColor Color { get; }

        public HistoryItem(IColor color)
        {
            Color = color;
            ColorString = color.ToRgb().ToHex().ToString();
            ColorBrush = new SolidColorBrush(Color.ToRgb().ToMediaColor());
        }
    }

    public sealed class HistoryWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<HistoryItem> _states = new ObservableCollection<HistoryItem>();

        public ObservableCollection<HistoryItem> States
        {
            get => _states;
            set => SetField(ref _states, value);
        }

        // Boilerplate:

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
