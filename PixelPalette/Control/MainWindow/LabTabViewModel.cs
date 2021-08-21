using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using PixelPalette.Color;

namespace PixelPalette.Control.MainWindow
{
    public sealed class LabTabViewModel : INotifyPropertyChanged
    {
        private GlobalState GlobalState { get; }

        private string _text = "";
        private double _l;
        private double _a;
        private double _b;

        private LinearGradientBrush? _lGradientFill;
        private LinearGradientBrush? _aGradientFill;
        private LinearGradientBrush? _bGradientFill;

        public LabTabViewModel(GlobalState globalState)
        {
            GlobalState = globalState;
            GlobalState.PropertyChanged += (_, ev) =>
            {
                if (ev.PropertyName == "Lab") RefreshValues();
            };
            RefreshValues();
        }

        public string Text
        {
            get => _text;
            set => SetField(ref _text, value, TextEventArgs);
        }

        public double L
        {
            get => _l;
            set => SetField(ref _l, value, LEventArgs);
        }

        public double A
        {
            get => _a;
            set => SetField(ref _a, value, AEventArgs);
        }

        public double B
        {
            get => _b;
            set => SetField(ref _b, value, BEventArgs);
        }

        public LinearGradientBrush? LGradientFill
        {
            get => _lGradientFill;
            private set => SetField(ref _lGradientFill, value, LGradientFillEventArgs);
        }

        public LinearGradientBrush? AGradientFill
        {
            get => _aGradientFill;
            private set => SetField(ref _aGradientFill, value, AGradientFillEventArgs);
        }

        public LinearGradientBrush? BGradientFill
        {
            get => _bGradientFill;
            private set => SetField(ref _bGradientFill, value, BGradientFillEventArgs);
        }

        private static readonly PropertyChangedEventArgs TextEventArgs = new(nameof(Text));
        private static readonly PropertyChangedEventArgs LEventArgs = new(nameof(L));
        private static readonly PropertyChangedEventArgs AEventArgs = new(nameof(A));
        private static readonly PropertyChangedEventArgs BEventArgs = new(nameof(B));
        private static readonly PropertyChangedEventArgs LGradientFillEventArgs = new(nameof(LGradientFill));
        private static readonly PropertyChangedEventArgs AGradientFillEventArgs = new(nameof(AGradientFill));
        private static readonly PropertyChangedEventArgs BGradientFillEventArgs = new(nameof(BGradientFill));

        private void RefreshValues()
        {
            _isUserUpdate = false;

            Text = GlobalState.Lab.ToString();
            L = GlobalState.Lab.RoundedL;
            A = GlobalState.Lab.RoundedA;
            B = GlobalState.Lab.RoundedB;

            var lGradientFill = Window.MainWindow.NewBrush();
            var aGradientFill = Window.MainWindow.NewBrush();
            var bGradientFill = Window.MainWindow.NewBrush();

            lGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithL(Lab.MinL).ToRgb().ToMediaColor(), 0.0));
            lGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithL(Lab.MaxL).ToRgb().ToMediaColor(), 1.0));
            aGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithA(Lab.MinA).ToRgb().ToMediaColor(), 0.0));
            aGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithA(Lab.MaxA).ToRgb().ToMediaColor(), 1.0));
            bGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithB(Lab.MinB).ToRgb().ToMediaColor(), 0.0));
            bGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithB(Lab.MaxB).ToRgb().ToMediaColor(), 1.0));

            LGradientFill = lGradientFill;
            AGradientFill = aGradientFill;
            BGradientFill = bGradientFill;

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
