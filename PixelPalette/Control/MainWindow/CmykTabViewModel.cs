using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;

namespace PixelPalette.Control.MainWindow
{
    public sealed class CmykTabViewModel : INotifyPropertyChanged
    {
        private GlobalState GlobalState { get; }

        private string _text = "";
        private string _scaledText = "";
        private string _cyan = "";
        private string _magenta = "";
        private string _yellow = "";
        private string _key = "";
        private string _scaledCyan = "";
        private string _scaledMagenta = "";
        private string _scaledYellow = "";
        private string _scaledKey = "";

        private LinearGradientBrush? _cyanGradientFill;
        private LinearGradientBrush? _magentaGradientFill;
        private LinearGradientBrush? _yellowGradientFill;
        private LinearGradientBrush? _keyGradientFill;

        public CmykTabViewModel(GlobalState globalState)
        {
            GlobalState = globalState;
            GlobalState.PropertyChanged += (_, ev) =>
            {
                if (ev.PropertyName == "Cmyk") RefreshValues();
            };
            RefreshValues();
        }

        public string Text
        {
            get => _text;
            set => SetField(ref _text, value, TextEventArgs);
        }

        public string ScaledText
        {
            get => _scaledText;
            set => SetField(ref _scaledText, value, ScaledTextEventArgs);
        }

        public string Cyan
        {
            get => _cyan;
            set => SetField(ref _cyan, value, CyanEventArgs);
        }

        public string Magenta
        {
            get => _magenta;
            set => SetField(ref _magenta, value, MagentaEventArgs);
        }

        public string Yellow
        {
            get => _yellow;
            set => SetField(ref _yellow, value, YellowEventArgs);
        }

        public string Key
        {
            get => _key;
            set => SetField(ref _key, value, KeyEventArgs);
        }

        public string ScaledCyan
        {
            get => _scaledCyan;
            set => SetField(ref _scaledCyan, value, ScaledCyanEventArgs);
        }

        public string ScaledMagenta
        {
            get => _scaledMagenta;
            set => SetField(ref _scaledMagenta, value, ScaledMagentaEventArgs);
        }

        public string ScaledYellow
        {
            get => _scaledYellow;
            set => SetField(ref _scaledYellow, value, ScaledYellowEventArgs);
        }

        public string ScaledKey
        {
            get => _scaledKey;
            set => SetField(ref _scaledKey, value, ScaledKeyEventArgs);
        }

        public LinearGradientBrush? CyanGradientFill
        {
            get => _cyanGradientFill;
            private set => SetField(ref _cyanGradientFill, value, CyanGradientFillEventArgs);
        }

        public LinearGradientBrush? MagentaGradientFill
        {
            get => _magentaGradientFill;
            private set => SetField(ref _magentaGradientFill, value, MagentaGradientFillEventArgs);
        }

        public LinearGradientBrush? YellowGradientFill
        {
            get => _yellowGradientFill;
            private set => SetField(ref _yellowGradientFill, value, YellowGradientFillEventArgs);
        }

        public LinearGradientBrush? KeyGradientFill
        {
            get => _keyGradientFill;
            private set => SetField(ref _keyGradientFill, value, KeyGradientFillEventArgs);
        }

        private static readonly PropertyChangedEventArgs TextEventArgs = new(nameof(Text));
        private static readonly PropertyChangedEventArgs ScaledTextEventArgs = new(nameof(ScaledText));
        private static readonly PropertyChangedEventArgs CyanEventArgs = new(nameof(Cyan));
        private static readonly PropertyChangedEventArgs MagentaEventArgs = new(nameof(Magenta));
        private static readonly PropertyChangedEventArgs YellowEventArgs = new(nameof(Yellow));
        private static readonly PropertyChangedEventArgs KeyEventArgs = new(nameof(Key));
        private static readonly PropertyChangedEventArgs ScaledCyanEventArgs = new(nameof(ScaledCyan));
        private static readonly PropertyChangedEventArgs ScaledMagentaEventArgs = new(nameof(ScaledMagenta));
        private static readonly PropertyChangedEventArgs ScaledYellowEventArgs = new(nameof(ScaledYellow));
        private static readonly PropertyChangedEventArgs ScaledKeyEventArgs = new(nameof(ScaledKey));
        private static readonly PropertyChangedEventArgs CyanGradientFillEventArgs = new(nameof(CyanGradientFill));
        private static readonly PropertyChangedEventArgs MagentaGradientFillEventArgs = new(nameof(MagentaGradientFill));
        private static readonly PropertyChangedEventArgs YellowGradientFillEventArgs = new(nameof(YellowGradientFill));
        private static readonly PropertyChangedEventArgs KeyGradientFillEventArgs = new(nameof(KeyGradientFill));

        private void RefreshValues()
        {
            _isUserUpdate = false;

            Text = GlobalState.Cmyk.ToString();
            ScaledText = GlobalState.Cmyk.ToScaledString();
            Cyan = GlobalState.Cmyk.RoundedCyan.ToString(CultureInfo.InvariantCulture);
            Magenta = GlobalState.Cmyk.RoundedMagenta.ToString(CultureInfo.InvariantCulture);
            Yellow = GlobalState.Cmyk.RoundedYellow.ToString(CultureInfo.InvariantCulture);
            Key = GlobalState.Cmyk.RoundedKey.ToString(CultureInfo.InvariantCulture);
            ScaledCyan = GlobalState.Cmyk.RoundedScaledCyan.ToString(CultureInfo.InvariantCulture);
            ScaledMagenta = GlobalState.Cmyk.RoundedScaledMagenta.ToString(CultureInfo.InvariantCulture);
            ScaledYellow = GlobalState.Cmyk.RoundedScaledYellow.ToString(CultureInfo.InvariantCulture);
            ScaledKey = GlobalState.Cmyk.RoundedScaledKey.ToString(CultureInfo.InvariantCulture);

            var cyanGradientFill = Window.MainWindow.NewBrush();
            var magentaGradientFill = Window.MainWindow.NewBrush();
            var yellowGradientFill = Window.MainWindow.NewBrush();
            var keyGradientFill = Window.MainWindow.NewBrush();

            cyanGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithCyan(0.0).ToRgb().ToMediaColor(), 0.0));
            cyanGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithCyan(1.0).ToRgb().ToMediaColor(), 1.0));
            magentaGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithMagenta(0.0).ToRgb().ToMediaColor(), 0.0));
            magentaGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithMagenta(1.0).ToRgb().ToMediaColor(), 1.0));
            yellowGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithYellow(0.0).ToRgb().ToMediaColor(), 0.0));
            yellowGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithYellow(1.0).ToRgb().ToMediaColor(), 1.0));
            keyGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithKey(0.0).ToRgb().ToMediaColor(), 0.0));
            keyGradientFill.GradientStops.Add(new GradientStop(GlobalState.Cmyk.WithKey(1.0).ToRgb().ToMediaColor(), 1.0));

            CyanGradientFill = cyanGradientFill;
            MagentaGradientFill = magentaGradientFill;
            YellowGradientFill = yellowGradientFill;
            KeyGradientFill = keyGradientFill;

            _isUserUpdate = true;
        }

#region boilerplate

        private bool _isUserUpdate = true;
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<PropertyChangedEventArgs>? PropertyChangedByUser;

        private void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
            if (_isUserUpdate) PropertyChangedByUser?.Invoke(this, eventArgs);
        }

        private void SetField<T>(ref T field, T value, PropertyChangedEventArgs eventArgs)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            OnPropertyChanged(eventArgs);
        }

#endregion
    }
}
