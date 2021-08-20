using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PixelPalette.Annotations;
using PixelPalette.Color;

namespace PixelPalette
{
    public class GlobalState : INotifyPropertyChanged
    {
        private Rgb _rgb = Rgb.Empty;

        public Rgb Rgb
        {
            get => _rgb;
            set => SetField(ref _rgb, value);
        }

        private bool _historyVisible;

        public bool HistoryVisible
        {
            get => _historyVisible;
            set => SetField(ref _historyVisible, value);
        }

        // Boilerplate

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            OnPropertyChanged(propertyName);
        }
    }
}
