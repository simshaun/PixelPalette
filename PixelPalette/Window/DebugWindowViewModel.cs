using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PixelPalette.Annotations;

namespace PixelPalette.Window
{
    public class DebugWindowViewModel : INotifyPropertyChanged
    {
        public string LinesText => AppDebug.LinesText;

        // Boilerplate:

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