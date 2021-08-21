using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace PixelPalette.Control.MainWindow
{
    public sealed class HexTabViewModel : INotifyPropertyChanged
    {
        public GlobalState GlobalState { get; }

        private string _text = "";
        private string _red = "";
        private string _green = "";
        private string _blue = "";

        private LinearGradientBrush? _redGradientFill;
        private LinearGradientBrush? _greenGradientFill;
        private LinearGradientBrush? _blueGradientFill;

        public HexTabViewModel(GlobalState globalState)
        {
            GlobalState = globalState;
            GlobalState.PropertyChanged += (_, ev) =>
            {
                if (ev.PropertyName == "Hex") RefreshValues();
            };
            RefreshValues();
        }

        public string Text
        {
            get => _text;
            set => SetField(ref _text, value, TextEventArgs);
        }

        public string Red
        {
            get => _red;
            set => SetField(ref _red, value, RedEventArgs);
        }

        public string Green
        {
            get => _green;
            set => SetField(ref _green, value, GreenEventArgs);
        }

        public string Blue
        {
            get => _blue;
            set => SetField(ref _blue, value, BlueEventArgs);
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
        private static readonly PropertyChangedEventArgs RedEventArgs = new(nameof(Red));
        private static readonly PropertyChangedEventArgs GreenEventArgs = new(nameof(Green));
        private static readonly PropertyChangedEventArgs BlueEventArgs = new(nameof(Blue));
        private static readonly PropertyChangedEventArgs RedGradientFillEventArgs = new(nameof(RedGradientFill));
        private static readonly PropertyChangedEventArgs GreenGradientFillEventArgs = new(nameof(GreenGradientFill));
        private static readonly PropertyChangedEventArgs BlueGradientFillEventArgs = new(nameof(BlueGradientFill));

        private void RefreshValues()
        {
            _isUserUpdate = false;

            Text = GlobalState.Hex.ToString();
            Red = GlobalState.Hex.RedPart;
            Green = GlobalState.Hex.GreenPart;
            Blue = GlobalState.Hex.BluePart;

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
