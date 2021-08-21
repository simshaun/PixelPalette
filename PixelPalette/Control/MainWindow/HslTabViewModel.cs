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

        private string _hslText = "";
        private string _hslScaledText = "";
        private string _hslHue = "";
        private string _hslSaturation = "";
        private string _hslLuminance = "";
        private string _hslScaledHue = "";
        private string _hslScaledSaturation = "";
        private string _hslScaledLuminance = "";

        private LinearGradientBrush? _hueGradientFill;
        private LinearGradientBrush? _hslSaturationGradientFill;
        private LinearGradientBrush? _hslLuminanceGradientFill;

        public HslTabViewModel(GlobalState globalState)
        {
            GlobalState = globalState;
            GlobalState.PropertyChanged += (_, ev) =>
            {
                if (ev.PropertyName == "Hsl") RefreshValues();
            };
            RefreshValues();
        }

        public string HslText
        {
            get => _hslText;
            set => SetField(ref _hslText, value);
        }

        public string HslScaledText
        {
            get => _hslScaledText;
            set => SetField(ref _hslScaledText, value);
        }

        public string HslHue
        {
            get => _hslHue;
            set => SetField(ref _hslHue, value);
        }

        public string HslSaturation
        {
            get => _hslSaturation;
            set => SetField(ref _hslSaturation, value);
        }

        public string HslLuminance
        {
            get => _hslLuminance;
            set => SetField(ref _hslLuminance, value);
        }

        public string HslScaledHue
        {
            get => _hslScaledHue;
            set => SetField(ref _hslScaledHue, value);
        }

        public string HslScaledSaturation
        {
            get => _hslScaledSaturation;
            set => SetField(ref _hslScaledSaturation, value);
        }

        public string HslScaledLuminance
        {
            get => _hslScaledLuminance;
            set => SetField(ref _hslScaledLuminance, value);
        }

        public LinearGradientBrush? HueGradientFill
        {
            get => _hueGradientFill;
            private set => SetField(ref _hueGradientFill, value);
        }

        public LinearGradientBrush? HslSaturationGradientFill
        {
            get => _hslSaturationGradientFill;
            private set => SetField(ref _hslSaturationGradientFill, value);
        }

        public LinearGradientBrush? HslLuminanceGradientFill
        {
            get => _hslLuminanceGradientFill;
            private set => SetField(ref _hslLuminanceGradientFill, value);
        }

        private void RefreshValues()
        {
            _isUserUpdate = false;

            HslText = GlobalState.Hsl.ToString();
            HslScaledText = GlobalState.Hsl.ToScaledString();
            HslHue = GlobalState.Hsl.RoundedHue.ToString(CultureInfo.InvariantCulture);
            HslSaturation = GlobalState.Hsl.RoundedSaturation.ToString(CultureInfo.InvariantCulture);
            HslLuminance = GlobalState.Hsl.RoundedLuminance.ToString(CultureInfo.InvariantCulture);
            HslScaledHue = GlobalState.Hsl.RoundedScaledHue.ToString(CultureInfo.InvariantCulture);
            HslScaledSaturation = GlobalState.Hsl.RoundedScaledSaturation.ToString(CultureInfo.InvariantCulture);
            HslScaledLuminance = GlobalState.Hsl.RoundedScaledLuminance.ToString(CultureInfo.InvariantCulture);

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
            HslSaturationGradientFill = hslSaturationGradientFill;
            HslLuminanceGradientFill = hslLuminanceGradientFill;

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
