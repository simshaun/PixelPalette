using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace PixelPalette.Control.MainWindow
{
    public sealed class RgbTabViewModel : INotifyPropertyChanged
    {
        public GlobalState GlobalState { get; }

        private string _rgbText = "";
        private string _rgbScaledText = "";
        private string _rgbRedText = "";
        private string _rgbGreenText = "";
        private string _rgbBlueText = "";
        private string _rgbScaledRedText = "";
        private string _rgbScaledGreenText = "";
        private string _rgbScaledBlueText = "";

        private double _rgbR;
        private double _rgbG;
        private double _rgbB;

        private LinearGradientBrush? _redGradientFill;
        private LinearGradientBrush? _greenGradientFill;
        private LinearGradientBrush? _blueGradientFill;

        public RgbTabViewModel(GlobalState globalState)
        {
            GlobalState = globalState;
            GlobalState.PropertyChanged += (_, ev) =>
            {
                if (ev.PropertyName == "Rgb") RefreshValues();
            };
            RefreshValues();
        }

        public string RgbText
        {
            get => _rgbText;
            set => SetField(ref _rgbText, value, RgbTextEventArgs);
        }

        public string RgbScaledText
        {
            get => _rgbScaledText;
            set => SetField(ref _rgbScaledText, value, RgbScaledTextEventArgs);
        }

        public string RgbRedText
        {
            get => _rgbRedText;
            set => SetField(ref _rgbRedText, value, RgbRedTextEventArgs);
        }

        public string RgbGreenText
        {
            get => _rgbGreenText;
            set => SetField(ref _rgbGreenText, value, RgbGreenTextEventArgs);
        }

        public string RgbBlueText
        {
            get => _rgbBlueText;
            set => SetField(ref _rgbBlueText, value, RgbBlueTextEventArgs);
        }

        public string RgbScaledRedText
        {
            get => _rgbScaledRedText;
            set => SetField(ref _rgbScaledRedText, value, RgbScaledRedTextEventArgs);
        }

        public string RgbScaledGreenText
        {
            get => _rgbScaledGreenText;
            set => SetField(ref _rgbScaledGreenText, value, RgbScaledGreenTextEventArgs);
        }

        public string RgbScaledBlueText
        {
            get => _rgbScaledBlueText;
            set => SetField(ref _rgbScaledBlueText, value, RgbScaledBlueTextEventArgs);
        }

        public double RgbR
        {
            get => _rgbR;
            set => SetField(ref _rgbR, value, RgbREventArgs);
        }

        public double RgbG
        {
            get => _rgbG;
            set => SetField(ref _rgbG, value, RgbGEventArgs);
        }

        public double RgbB
        {
            get => _rgbB;
            set => SetField(ref _rgbB, value, RgbBEventArgs);
        }

        public LinearGradientBrush? RedGradientFill
        {
            get => _redGradientFill;
            private set => SetField(ref _redGradientFill, value, RedGradientFillEventArgs);
        }

        public LinearGradientBrush? GreenGradientFill
        {
            get => _greenGradientFill;
            private set => SetField(ref _greenGradientFill, value, GreenGradientFillEventArgs);
        }

        public LinearGradientBrush? BlueGradientFill
        {
            get => _blueGradientFill;
            private set => SetField(ref _blueGradientFill, value, BlueGradientFillEventArgs);
        }

        private static readonly PropertyChangedEventArgs RgbTextEventArgs = new(nameof(RgbText));
        private static readonly PropertyChangedEventArgs RgbScaledTextEventArgs = new(nameof(RgbScaledText));
        private static readonly PropertyChangedEventArgs RgbRedTextEventArgs = new(nameof(RgbRedText));
        private static readonly PropertyChangedEventArgs RgbGreenTextEventArgs = new(nameof(RgbGreenText));
        private static readonly PropertyChangedEventArgs RgbBlueTextEventArgs = new(nameof(RgbBlueText));
        private static readonly PropertyChangedEventArgs RgbScaledRedTextEventArgs = new(nameof(RgbScaledRedText));
        private static readonly PropertyChangedEventArgs RgbScaledGreenTextEventArgs = new(nameof(RgbScaledGreenText));
        private static readonly PropertyChangedEventArgs RgbScaledBlueTextEventArgs = new(nameof(RgbScaledBlueText));
        private static readonly PropertyChangedEventArgs RgbREventArgs = new(nameof(RgbR));
        private static readonly PropertyChangedEventArgs RgbGEventArgs = new(nameof(RgbG));
        private static readonly PropertyChangedEventArgs RgbBEventArgs = new(nameof(RgbB));
        private static readonly PropertyChangedEventArgs RedGradientFillEventArgs = new(nameof(RedGradientFill));
        private static readonly PropertyChangedEventArgs GreenGradientFillEventArgs = new(nameof(GreenGradientFill));
        private static readonly PropertyChangedEventArgs BlueGradientFillEventArgs = new(nameof(BlueGradientFill));

        private void RefreshValues()
        {
            _isUserUpdate = false;

            RgbText = GlobalState.Rgb.ToString();
            RgbScaledText = GlobalState.Rgb.ToScaledString();
            RgbRedText = GlobalState.Rgb.RoundedRed.ToString(CultureInfo.InvariantCulture);
            RgbGreenText = GlobalState.Rgb.RoundedGreen.ToString(CultureInfo.InvariantCulture);
            RgbBlueText = GlobalState.Rgb.RoundedBlue.ToString(CultureInfo.InvariantCulture);
            RgbScaledRedText = GlobalState.Rgb.ScaledRed.ToString();
            RgbScaledGreenText = GlobalState.Rgb.ScaledGreen.ToString();
            RgbScaledBlueText = GlobalState.Rgb.ScaledBlue.ToString();
            RgbR = GlobalState.Rgb.Red;
            RgbG = GlobalState.Rgb.Green;
            RgbB = GlobalState.Rgb.Blue;

            var redGradientFill = Window.MainWindow.NewBrush();
            var greenGradientFill = Window.MainWindow.NewBrush();
            var blueGradientFill = Window.MainWindow.NewBrush();

            redGradientFill.GradientStops.Add(new GradientStop(GlobalState.Rgb.WithRed(0.0).ToMediaColor(), 0.0));
            redGradientFill.GradientStops.Add(new GradientStop(GlobalState.Rgb.WithRed(1.0).ToMediaColor(), 1.0));
            greenGradientFill.GradientStops.Add(new GradientStop(GlobalState.Rgb.WithGreen(0.0).ToMediaColor(), 0.0));
            greenGradientFill.GradientStops.Add(new GradientStop(GlobalState.Rgb.WithGreen(1.0).ToMediaColor(), 1.0));
            blueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Rgb.WithBlue(0.0).ToMediaColor(), 0.0));
            blueGradientFill.GradientStops.Add(new GradientStop(GlobalState.Rgb.WithBlue(1.0).ToMediaColor(), 1.0));

            RedGradientFill = redGradientFill;
            GreenGradientFill = greenGradientFill;
            BlueGradientFill = blueGradientFill;

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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (_isUserUpdate) PropertyChangedByUser?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetField<T>(ref T field, T value, PropertyChangedEventArgs eventArgs)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            OnPropertyChanged(eventArgs);
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
