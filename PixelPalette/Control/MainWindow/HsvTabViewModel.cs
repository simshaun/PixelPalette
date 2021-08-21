using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using PixelPalette.Color;

namespace PixelPalette.Control.MainWindow
{
    public sealed class HsvTabViewModel : INotifyPropertyChanged
    {
        public GlobalState GlobalState { get; }

        private string _hsvText = "";
        private string _hsvScaledText = "";
        private string _hsvHue = "";
        private string _hsvSaturation = "";
        private string _hsvValue = "";
        private string _hsvScaledHue = "";
        private string _hsvScaledSaturation = "";
        private string _hsvScaledValue = "";

        private LinearGradientBrush? _hueGradientFill;
        private LinearGradientBrush? _hsvSaturationGradientFill;
        private LinearGradientBrush? _hsvValueGradientFill;

        public HsvTabViewModel(GlobalState globalState)
        {
            GlobalState = globalState;
            GlobalState.PropertyChanged += (_, ev) =>
            {
                if (ev.PropertyName == "Hsv") RefreshValues();
            };
            RefreshValues();
        }

        public string HsvText
        {
            get => _hsvText;
            set => SetField(ref _hsvText, value);
        }

        public string HsvScaledText
        {
            get => _hsvScaledText;
            set => SetField(ref _hsvScaledText, value);
        }

        public string HsvHue
        {
            get => _hsvHue;
            set => SetField(ref _hsvHue, value);
        }

        public string HsvSaturation
        {
            get => _hsvSaturation;
            set => SetField(ref _hsvSaturation, value);
        }

        public string HsvValue
        {
            get => _hsvValue;
            set => SetField(ref _hsvValue, value);
        }

        public string HsvScaledHue
        {
            get => _hsvScaledHue;
            set => SetField(ref _hsvScaledHue, value);
        }

        public string HsvScaledSaturation
        {
            get => _hsvScaledSaturation;
            set => SetField(ref _hsvScaledSaturation, value);
        }

        public string HsvScaledValue
        {
            get => _hsvScaledValue;
            set => SetField(ref _hsvScaledValue, value);
        }

        public LinearGradientBrush? HueGradientFill
        {
            get => _hueGradientFill;
            private set => SetField(ref _hueGradientFill, value);
        }

        public LinearGradientBrush? HsvSaturationGradientFill
        {
            get => _hsvSaturationGradientFill;
            private set => SetField(ref _hsvSaturationGradientFill, value);
        }

        public LinearGradientBrush? HsvValueGradientFill
        {
            get => _hsvValueGradientFill;
            private set => SetField(ref _hsvValueGradientFill, value);
        }

        private void RefreshValues()
        {
            _isUserUpdate = false;

            HsvText = GlobalState.Hsv.ToString();
            HsvScaledText = GlobalState.Hsv.ToScaledString();
            HsvHue = GlobalState.Hsv.RoundedHue.ToString(CultureInfo.InvariantCulture);
            HsvSaturation = GlobalState.Hsv.RoundedSaturation.ToString(CultureInfo.InvariantCulture);
            HsvValue = GlobalState.Hsv.RoundedValue.ToString(CultureInfo.InvariantCulture);
            HsvScaledHue = GlobalState.Hsv.RoundedScaledHue.ToString(CultureInfo.InvariantCulture);
            HsvScaledSaturation = GlobalState.Hsv.RoundedScaledSaturation.ToString(CultureInfo.InvariantCulture);
            HsvScaledValue = GlobalState.Hsv.RoundedScaledValue.ToString(CultureInfo.InvariantCulture);

            var hueGradientFill = Window.MainWindow.NewBrush();
            var hsvSaturationGradientFill = Window.MainWindow.NewBrush();
            var hsvValueGradientFill = Window.MainWindow.NewBrush();

            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(0, 100, 50).ToMediaColor(), 0.0));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(60, 100, 50).ToMediaColor(), 0.16));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(120, 100, 50).ToMediaColor(), 0.33));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(180, 100, 50).ToMediaColor(), 0.5));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(240, 100, 50).ToMediaColor(), 0.66));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(300, 100, 50).ToMediaColor(), 0.83));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(360, 100, 50).ToMediaColor(), 1.0));

            hsvSaturationGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithSaturation(0.0).ToMediaColor(), 0.0));
            hsvSaturationGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithSaturation(1.0).ToMediaColor(), 1.0));

            hsvValueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithValue(0.0).ToMediaColor(), 0.0));
            hsvValueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithValue(0.5).ToMediaColor(), 0.5)); // Gives color
            hsvValueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Hsv.WithValue(1.0).ToMediaColor(), 1.0));

            HueGradientFill = hueGradientFill;
            HsvSaturationGradientFill = hsvSaturationGradientFill;
            HsvValueGradientFill = hsvValueGradientFill;

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
