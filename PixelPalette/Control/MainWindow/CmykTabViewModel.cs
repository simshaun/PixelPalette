using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace PixelPalette.Control.MainWindow
{
    public sealed class CmykTabViewModel : INotifyPropertyChanged
    {
        public GlobalState GlobalState { get; }

        private string _cmykText = "";
        private string _cmykScaledText = "";
        private string _cmykCyan = "";
        private string _cmykMagenta = "";
        private string _cmykYellow = "";
        private string _cmykKey = "";
        private string _cmykScaledCyan = "";
        private string _cmykScaledMagenta = "";
        private string _cmykScaledYellow = "";
        private string _cmykScaledKey = "";

        private LinearGradientBrush? _cmykCyanGradientFill;
        private LinearGradientBrush? _cmykMagentaGradientFill;
        private LinearGradientBrush? _cmykYellowGradientFill;
        private LinearGradientBrush? _cmykKeyGradientFill;

        public CmykTabViewModel(GlobalState globalState)
        {
            GlobalState = globalState;
            GlobalState.PropertyChanged += (_, ev) =>
            {
                if (ev.PropertyName == "Cmyk") RefreshValues();
            };
            RefreshValues();
        }

        public string CmykText
        {
            get => _cmykText;
            set => SetField(ref _cmykText, value);
        }

        public string CmykScaledText
        {
            get => _cmykScaledText;
            set => SetField(ref _cmykScaledText, value);
        }

        public string CmykCyan
        {
            get => _cmykCyan;
            set => SetField(ref _cmykCyan, value);
        }

        public string CmykMagenta
        {
            get => _cmykMagenta;
            set => SetField(ref _cmykMagenta, value);
        }

        public string CmykYellow
        {
            get => _cmykYellow;
            set => SetField(ref _cmykYellow, value);
        }

        public string CmykKey
        {
            get => _cmykKey;
            set => SetField(ref _cmykKey, value);
        }

        public string CmykScaledCyan
        {
            get => _cmykScaledCyan;
            set => SetField(ref _cmykScaledCyan, value);
        }

        public string CmykScaledMagenta
        {
            get => _cmykScaledMagenta;
            set => SetField(ref _cmykScaledMagenta, value);
        }

        public string CmykScaledYellow
        {
            get => _cmykScaledYellow;
            set => SetField(ref _cmykScaledYellow, value);
        }

        public string CmykScaledKey
        {
            get => _cmykScaledKey;
            set => SetField(ref _cmykScaledKey, value);
        }

        public LinearGradientBrush? CmykCyanGradientFill
        {
            get => _cmykCyanGradientFill;
            private set => SetField(ref _cmykCyanGradientFill, value);
        }

        public LinearGradientBrush? CmykMagentaGradientFill
        {
            get => _cmykMagentaGradientFill;
            private set => SetField(ref _cmykMagentaGradientFill, value);
        }

        public LinearGradientBrush? CmykYellowGradientFill
        {
            get => _cmykYellowGradientFill;
            private set => SetField(ref _cmykYellowGradientFill, value);
        }

        public LinearGradientBrush? CmykKeyGradientFill
        {
            get => _cmykKeyGradientFill;
            private set => SetField(ref _cmykKeyGradientFill, value);
        }

        private void RefreshValues()
        {
            _isUserUpdate = false;

            CmykText = GlobalState.Cmyk.ToString();
            CmykScaledText = GlobalState.Cmyk.ToScaledString();
            CmykCyan = GlobalState.Cmyk.RoundedCyan.ToString(CultureInfo.InvariantCulture);
            CmykMagenta = GlobalState.Cmyk.RoundedMagenta.ToString(CultureInfo.InvariantCulture);
            CmykYellow = GlobalState.Cmyk.RoundedYellow.ToString(CultureInfo.InvariantCulture);
            CmykKey = GlobalState.Cmyk.RoundedKey.ToString(CultureInfo.InvariantCulture);
            CmykScaledCyan = GlobalState.Cmyk.RoundedScaledCyan.ToString(CultureInfo.InvariantCulture);
            CmykScaledMagenta = GlobalState.Cmyk.RoundedScaledMagenta.ToString(CultureInfo.InvariantCulture);
            CmykScaledYellow = GlobalState.Cmyk.RoundedScaledYellow.ToString(CultureInfo.InvariantCulture);
            CmykScaledKey = GlobalState.Cmyk.RoundedScaledKey.ToString(CultureInfo.InvariantCulture);

            var cmykCyanGradientFill = Window.MainWindow.NewBrush();
            var cmykMagentaGradientFill = Window.MainWindow.NewBrush();
            var cmykYellowGradientFill = Window.MainWindow.NewBrush();
            var cmykKeyGradientFill = Window.MainWindow.NewBrush();

            cmykCyanGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithCyan(0.0).ToRgb().ToMediaColor(), 0.0));
            cmykCyanGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithCyan(1.0).ToRgb().ToMediaColor(), 1.0));
            cmykMagentaGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithMagenta(0.0).ToRgb().ToMediaColor(), 0.0));
            cmykMagentaGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithMagenta(1.0).ToRgb().ToMediaColor(), 1.0));
            cmykYellowGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithYellow(0.0).ToRgb().ToMediaColor(), 0.0));
            cmykYellowGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithYellow(1.0).ToRgb().ToMediaColor(), 1.0));
            cmykKeyGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithKey(0.0).ToRgb().ToMediaColor(), 0.0));
            cmykKeyGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithKey(1.0).ToRgb().ToMediaColor(), 1.0));

            CmykCyanGradientFill = cmykCyanGradientFill;
            CmykMagentaGradientFill = cmykMagentaGradientFill;
            CmykYellowGradientFill = cmykYellowGradientFill;
            CmykKeyGradientFill = cmykKeyGradientFill;

            _isUserUpdate = true;
        }

#region boilerplate

        private bool _isUserUpdate = true;
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<PropertyChangedEventArgs>? PropertyChangedByUser;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (_isUserUpdate) PropertyChangedByUser?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            OnPropertyChanged(propertyName);
        }

#endregion
    }
}
