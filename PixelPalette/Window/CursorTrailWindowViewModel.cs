using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PixelPalette.Annotations;

namespace PixelPalette.Window
{
    public class CursorTrailWindowViewModel : INotifyPropertyChanged
    {
        private string _hex;
        private string _hexTextColor;
        private string _gridLineColor;

        public string Hex
        {
            get => _hex;
            set => SetField(ref _hex, value);
        }

        public string HexTextColor
        {
            get => _hexTextColor;
            set => SetField(ref _hexTextColor, value);
        }

        public string GridLineColor
        {
            get => _gridLineColor;
            set => SetField(ref _gridLineColor, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}