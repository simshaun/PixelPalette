using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PixelPalette.Window
{
    public sealed class CursorTrailWindowViewModel : INotifyPropertyChanged
    {
        private string _hex = "";
        private string _hexTextColor = "";
        private string _gridLineColor = "";

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
}
