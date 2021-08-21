using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;

namespace PixelPalette.Control.MainWindow
{
    public sealed class RgbTabViewModel : INotifyPropertyChanged
    {
        private GlobalState GlobalState { get; }

        private string _text = "";
        private string _scaledText = "";
        private string _redText = "";
        private string _greenText = "";
        private string _blueText = "";
        private string _scaledRedText = "";
        private string _scaledGreenText = "";
        private string _scaledBlueText = "";

        private double _r;
        private double _g;
        private double _b;

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

        public string RedText
        {
            get => _redText;
            set => SetField(ref _redText, value, RedTextEventArgs);
        }

        public string GreenText
        {
            get => _greenText;
            set => SetField(ref _greenText, value, GreenTextEventArgs);
        }

        public string BlueText
        {
            get => _blueText;
            set => SetField(ref _blueText, value, BlueTextEventArgs);
        }

        public string ScaledRedText
        {
            get => _scaledRedText;
            set => SetField(ref _scaledRedText, value, ScaledRedTextEventArgs);
        }

        public string ScaledGreenText
        {
            get => _scaledGreenText;
            set => SetField(ref _scaledGreenText, value, ScaledGreenTextEventArgs);
        }

        public string ScaledBlueText
        {
            get => _scaledBlueText;
            set => SetField(ref _scaledBlueText, value, ScaledBlueTextEventArgs);
        }

        public double R
        {
            get => _r;
            set => SetField(ref _r, value, REventArgs);
        }

        public double G
        {
            get => _g;
            set => SetField(ref _g, value, GEventArgs);
        }

        public double B
        {
            get => _b;
            set => SetField(ref _b, value, BEventArgs);
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

        private static readonly PropertyChangedEventArgs TextEventArgs = new(nameof(Text));
        private static readonly PropertyChangedEventArgs ScaledTextEventArgs = new(nameof(ScaledText));
        private static readonly PropertyChangedEventArgs RedTextEventArgs = new(nameof(RedText));
        private static readonly PropertyChangedEventArgs GreenTextEventArgs = new(nameof(GreenText));
        private static readonly PropertyChangedEventArgs BlueTextEventArgs = new(nameof(BlueText));
        private static readonly PropertyChangedEventArgs ScaledRedTextEventArgs = new(nameof(ScaledRedText));
        private static readonly PropertyChangedEventArgs ScaledGreenTextEventArgs = new(nameof(ScaledGreenText));
        private static readonly PropertyChangedEventArgs ScaledBlueTextEventArgs = new(nameof(ScaledBlueText));
        private static readonly PropertyChangedEventArgs REventArgs = new(nameof(R));
        private static readonly PropertyChangedEventArgs GEventArgs = new(nameof(G));
        private static readonly PropertyChangedEventArgs BEventArgs = new(nameof(B));
        private static readonly PropertyChangedEventArgs RedGradientFillEventArgs = new(nameof(RedGradientFill));
        private static readonly PropertyChangedEventArgs GreenGradientFillEventArgs = new(nameof(GreenGradientFill));
        private static readonly PropertyChangedEventArgs BlueGradientFillEventArgs = new(nameof(BlueGradientFill));

        private void RefreshValues()
        {
            _isUserUpdate = false;

            Text = GlobalState.Rgb.ToString();
            ScaledText = GlobalState.Rgb.ToScaledString();
            RedText = GlobalState.Rgb.RoundedRed.ToString(CultureInfo.InvariantCulture);
            GreenText = GlobalState.Rgb.RoundedGreen.ToString(CultureInfo.InvariantCulture);
            BlueText = GlobalState.Rgb.RoundedBlue.ToString(CultureInfo.InvariantCulture);
            ScaledRedText = GlobalState.Rgb.ScaledRed.ToString();
            ScaledGreenText = GlobalState.Rgb.ScaledGreen.ToString();
            ScaledBlueText = GlobalState.Rgb.ScaledBlue.ToString();
            R = GlobalState.Rgb.Red;
            G = GlobalState.Rgb.Green;
            B = GlobalState.Rgb.Blue;

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

        private void SetField<T>(ref T field, T value, PropertyChangedEventArgs eventArgs)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            OnPropertyChanged(eventArgs);
        }

#endregion
    }
}
