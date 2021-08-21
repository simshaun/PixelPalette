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
        public GlobalState GlobalState { get; }

        private string _labText = "";
        private double _labL;
        private double _labA;
        private double _labB;

        private LinearGradientBrush? _labLGradientFill;
        private LinearGradientBrush? _labAGradientFill;
        private LinearGradientBrush? _labBGradientFill;

        public LabTabViewModel(GlobalState globalState)
        {
            GlobalState = globalState;
            GlobalState.PropertyChanged += (_, ev) =>
            {
                if (ev.PropertyName == "Lab") RefreshValues();
            };
            RefreshValues();
        }

        public string LabText
        {
            get => _labText;
            set => SetField(ref _labText, value);
        }

        public double LabL
        {
            get => _labL;
            set => SetField(ref _labL, value);
        }

        public double LabA
        {
            get => _labA;
            set => SetField(ref _labA, value);
        }

        public double LabB
        {
            get => _labB;
            set => SetField(ref _labB, value);
        }

        public LinearGradientBrush? LabLGradientFill
        {
            get => _labLGradientFill;
            private set => SetField(ref _labLGradientFill, value);
        }

        public LinearGradientBrush? LabAGradientFill
        {
            get => _labAGradientFill;
            private set => SetField(ref _labAGradientFill, value);
        }

        public LinearGradientBrush? LabBGradientFill
        {
            get => _labBGradientFill;
            private set => SetField(ref _labBGradientFill, value);
        }

        private void RefreshValues()
        {
            _isUserUpdate = false;

            LabText = GlobalState.Lab.ToString();
            LabL = GlobalState.Lab.RoundedL;
            LabA = GlobalState.Lab.RoundedA;
            LabB = GlobalState.Lab.RoundedB;

            var labLGradientFill = Window.MainWindow.NewBrush();
            var labAGradientFill = Window.MainWindow.NewBrush();
            var labBGradientFill = Window.MainWindow.NewBrush();

            labLGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithL(Lab.MinL).ToRgb().ToMediaColor(), 0.0));
            labLGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithL(Lab.MaxL).ToRgb().ToMediaColor(), 1.0));
            labAGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithA(Lab.MinA).ToRgb().ToMediaColor(), 0.0));
            labAGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithA(Lab.MaxA).ToRgb().ToMediaColor(), 1.0));
            labBGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithB(Lab.MinB).ToRgb().ToMediaColor(), 0.0));
            labBGradientFill.GradientStops.Add(new GradientStop(GlobalState.Lab.WithB(Lab.MaxB).ToRgb().ToMediaColor(), 1.0));

            LabLGradientFill = labLGradientFill;
            LabAGradientFill = labAGradientFill;
            LabBGradientFill = labBGradientFill;

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
