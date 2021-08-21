using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using PixelPalette.Color;

namespace PixelPalette.Control.MainWindow
{
    public sealed class HslTabViewModel : INotifyPropertyChanged
    {
        public GlobalState GlobalState { get; }

        private string _text = "";
        private string _scaledText = "";
        private string _hue = "";
        private string _saturation = "";
        private string _luminance = "";
        private string _scaledHue = "";
        private string _scaledSaturation = "";
        private string _scaledLuminance = "";

        private LinearGradientBrush? _hueGradientFill;
        private LinearGradientBrush? _saturationGradientFill;
        private LinearGradientBrush? _luminanceGradientFill;

        public HslTabViewModel(GlobalState globalState)
        {
            GlobalState = globalState;
            GlobalState.PropertyChanged += (_, ev) =>
            {
                if (ev.PropertyName == "Hsl") RefreshValues();
            };
            RefreshValues();
        }

        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        public string ScaledText
        {
            get => _scaledText;
            set => SetField(ref _scaledText, value);
        }

        public string Hue
        {
            get => _hue;
            set => SetField(ref _hue, value);
        }

        public string Saturation
        {
            get => _saturation;
            set => SetField(ref _saturation, value);
        }

        public string Luminance
        {
            get => _luminance;
            set => SetField(ref _luminance, value);
        }

        public string ScaledHue
        {
            get => _scaledHue;
            set => SetField(ref _scaledHue, value);
        }

        public string ScaledSaturation
        {
            get => _scaledSaturation;
            set => SetField(ref _scaledSaturation, value);
        }

        public string ScaledLuminance
        {
            get => _scaledLuminance;
            set => SetField(ref _scaledLuminance, value);
        }

        public LinearGradientBrush? HueGradientFill
        {
            get => _hueGradientFill;
            private set => SetField(ref _hueGradientFill, value);
        }

        public LinearGradientBrush? SaturationGradientFill
        {
            get => _saturationGradientFill;
            private set => SetField(ref _saturationGradientFill, value);
        }

        public LinearGradientBrush? LuminanceGradientFill
        {
            get => _luminanceGradientFill;
            private set => SetField(ref _luminanceGradientFill, value);
        }

        private void RefreshValues()
        {
            _isUserUpdate = false;

            Text = GlobalState.Hsl.ToString();
            ScaledText = GlobalState.Hsl.ToScaledString();
            Hue = GlobalState.Hsl.RoundedHue.ToString(CultureInfo.InvariantCulture);
            Saturation = GlobalState.Hsl.RoundedSaturation.ToString(CultureInfo.InvariantCulture);
            Luminance = GlobalState.Hsl.RoundedLuminance.ToString(CultureInfo.InvariantCulture);
            ScaledHue = GlobalState.Hsl.RoundedScaledHue.ToString(CultureInfo.InvariantCulture);
            ScaledSaturation = GlobalState.Hsl.RoundedScaledSaturation.ToString(CultureInfo.InvariantCulture);
            ScaledLuminance = GlobalState.Hsl.RoundedScaledLuminance.ToString(CultureInfo.InvariantCulture);

            var hueGradientFill = Window.MainWindow.NewBrush();
            var hslSaturationGradientFill = Window.MainWindow.NewBrush();
            var hslLuminanceGradientFill = Window.MainWindow.NewBrush();

            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(0, 100, 50).ToMediaColor(), 0.0));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(60, 100, 50).ToMediaColor(), 0.16));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(120, 100, 50).ToMediaColor(), 0.33));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(180, 100, 50).ToMediaColor(), 0.5));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(240, 100, 50).ToMediaColor(), 0.66));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(300, 100, 50).ToMediaColor(), 0.83));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(360, 100, 50).ToMediaColor(), 1.0));

            hslSaturationGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsl.WithSaturation(0.0).ToMediaColor(), 0.0));
            hslSaturationGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsl.WithSaturation(1.0).ToMediaColor(), 1.0));

            hslLuminanceGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsl.WithLuminance(0.0).ToMediaColor(), 0.0));
            hslLuminanceGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsl.WithLuminance(0.5).ToMediaColor(), 0.5)); // Gives color
            hslLuminanceGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsl.WithLuminance(1.0).ToMediaColor(), 1.0));

            HueGradientFill = hueGradientFill;
            SaturationGradientFill = hslSaturationGradientFill;
            LuminanceGradientFill = hslLuminanceGradientFill;

            _isUserUpdate = true;
        }

#region boilerplate

        private bool _isUserUpdate = true;
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<PropertyChangedEventArgs>? PropertyChangedByUser;

        private void OnPropertyChanged(string propertyName)
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, eventArgs);
            if (_isUserUpdate) PropertyChangedByUser?.Invoke(this, eventArgs);
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
