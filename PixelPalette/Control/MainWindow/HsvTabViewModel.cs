using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using PixelPalette.Color;

namespace PixelPalette.Control.MainWindow
{
    public sealed class HsvTabViewModel : INotifyPropertyChanged
    {
        public GlobalState GlobalState { get; }

        private string _text = "";
        private string _scaledText = "";
        private string _hue = "";
        private string _saturation = "";
        private string _value = "";
        private string _scaledHue = "";
        private string _scaledSaturation = "";
        private string _scaledValue = "";

        private LinearGradientBrush? _hueGradientFill;
        private LinearGradientBrush? _saturationGradientFill;
        private LinearGradientBrush? _valueGradientFill;

        public HsvTabViewModel(GlobalState globalState)
        {
            GlobalState = globalState;
            GlobalState.PropertyChanged += (_, ev) =>
            {
                if (ev.PropertyName == "Hsv") RefreshValues();
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

        public string Hue
        {
            get => _hue;
            set => SetField(ref _hue, value, HueEventArgs);
        }

        public string Saturation
        {
            get => _saturation;
            set => SetField(ref _saturation, value, SaturationEventArgs);
        }

        public string Value
        {
            get => _value;
            set => SetField(ref _value, value, ValueEventArgs);
        }

        public string ScaledHue
        {
            get => _scaledHue;
            set => SetField(ref _scaledHue, value, ScaledHueEventArgs);
        }

        public string ScaledSaturation
        {
            get => _scaledSaturation;
            set => SetField(ref _scaledSaturation, value, ScaledSaturationEventArgs);
        }

        public string ScaledValue
        {
            get => _scaledValue;
            set => SetField(ref _scaledValue, value, ScaledValueEventArgs);
        }

        public LinearGradientBrush? HueGradientFill
        {
            get => _hueGradientFill;
            private set => SetField(ref _hueGradientFill, value, HueGradientFillEventArgs);
        }

        public LinearGradientBrush? SaturationGradientFill
        {
            get => _saturationGradientFill;
            private set => SetField(ref _saturationGradientFill, value, SaturationGradientFillEventArgs);
        }

        public LinearGradientBrush? ValueGradientFill
        {
            get => _valueGradientFill;
            private set => SetField(ref _valueGradientFill, value, ValueGradientFillEventArgs);
        }

        private static readonly PropertyChangedEventArgs TextEventArgs = new(nameof(Text));
        private static readonly PropertyChangedEventArgs ScaledTextEventArgs = new(nameof(ScaledText));
        private static readonly PropertyChangedEventArgs HueEventArgs = new(nameof(Hue));
        private static readonly PropertyChangedEventArgs SaturationEventArgs = new(nameof(Saturation));
        private static readonly PropertyChangedEventArgs ValueEventArgs = new(nameof(Value));
        private static readonly PropertyChangedEventArgs ScaledHueEventArgs = new(nameof(ScaledHue));
        private static readonly PropertyChangedEventArgs ScaledSaturationEventArgs = new(nameof(ScaledSaturation));
        private static readonly PropertyChangedEventArgs ScaledValueEventArgs = new(nameof(ScaledValue));
        private static readonly PropertyChangedEventArgs HueGradientFillEventArgs = new(nameof(HueGradientFill));
        private static readonly PropertyChangedEventArgs SaturationGradientFillEventArgs = new(nameof(SaturationGradientFill));
        private static readonly PropertyChangedEventArgs ValueGradientFillEventArgs = new(nameof(ValueGradientFill));

        private void RefreshValues()
        {
            _isUserUpdate = false;

            Text = GlobalState.Hsv.ToString();
            ScaledText = GlobalState.Hsv.ToScaledString();
            Hue = GlobalState.Hsv.RoundedHue.ToString(CultureInfo.InvariantCulture);
            Saturation = GlobalState.Hsv.RoundedSaturation.ToString(CultureInfo.InvariantCulture);
            Value = GlobalState.Hsv.RoundedValue.ToString(CultureInfo.InvariantCulture);
            ScaledHue = GlobalState.Hsv.RoundedScaledHue.ToString(CultureInfo.InvariantCulture);
            ScaledSaturation = GlobalState.Hsv.RoundedScaledSaturation.ToString(CultureInfo.InvariantCulture);
            ScaledValue = GlobalState.Hsv.RoundedScaledValue.ToString(CultureInfo.InvariantCulture);

            var hueGradientFill = Window.MainWindow.NewBrush();
            var saturationGradientFill = Window.MainWindow.NewBrush();
            var valueGradientFill = Window.MainWindow.NewBrush();

            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(0, 100, 50).ToMediaColor(), 0.0));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(60, 100, 50).ToMediaColor(), 0.16));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(120, 100, 50).ToMediaColor(), 0.33));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(180, 100, 50).ToMediaColor(), 0.5));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(240, 100, 50).ToMediaColor(), 0.66));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(300, 100, 50).ToMediaColor(), 0.83));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(360, 100, 50).ToMediaColor(), 1.0));

            saturationGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithSaturation(0.0).ToMediaColor(), 0.0));
            saturationGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithSaturation(1.0).ToMediaColor(), 1.0));

            valueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithValue(0.0).ToMediaColor(), 0.0));
            valueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithValue(0.5).ToMediaColor(), 0.5)); // Gives color
            valueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithValue(1.0).ToMediaColor(), 1.0));

            HueGradientFill = hueGradientFill;
            SaturationGradientFill = saturationGradientFill;
            ValueGradientFill = valueGradientFill;

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
