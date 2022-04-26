using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PixelPalette.Color;

namespace PixelPalette.State
{
    public class GlobalState : INotifyPropertyChanged
    {
        private static GlobalState? _instance;
        public static GlobalState Instance => _instance ??= new GlobalState();

        private bool _historyVisible;

        public bool HistoryVisible
        {
            get => _historyVisible;
            set => SetField(ref _historyVisible, value);
        }

        private Rgb _rgb = Rgb.Empty;
        private Hsl _hsl = Hsl.Empty;
        private Hsv _hsv = Hsv.Empty;
        private Hex _hex = Hex.Empty;
        private Cmyk _cmyk = Cmyk.Empty;
        private Lab _lab = Lab.Empty;

        public Rgb Rgb
        {
            get => _rgb;
            private set => SetField(ref _rgb, value, RgbEventArgs);
        }

        public Hsl Hsl
        {
            get => _hsl;
            private set => SetField(ref _hsl, value, HslEventArgs);
        }

        public Hsv Hsv
        {
            get => _hsv;
            private set => SetField(ref _hsv, value, HsvEventArgs);
        }

        public Hex Hex
        {
            get => _hex;
            private set => SetField(ref _hex, value, HexEventArgs);
        }

        public Cmyk Cmyk
        {
            get => _cmyk;
            private set => SetField(ref _cmyk, value, CmykEventArgs);
        }

        public Lab Lab
        {
            get => _lab;
            private set => SetField(ref _lab, value, LabEventArgs);
        }

        private static readonly PropertyChangedEventArgs RgbEventArgs = new(nameof(Rgb));
        private static readonly PropertyChangedEventArgs HslEventArgs = new(nameof(Hsl));
        private static readonly PropertyChangedEventArgs HsvEventArgs = new(nameof(Hsv));
        private static readonly PropertyChangedEventArgs HexEventArgs = new(nameof(Hex));
        private static readonly PropertyChangedEventArgs CmykEventArgs = new(nameof(Cmyk));
        private static readonly PropertyChangedEventArgs LabEventArgs = new(nameof(Lab));

        public void LoadFromPersistedData(PersistedData data)
        {
            switch (data.ActiveColorModel)
            {
                case "Rgb":
                    var rgb = Rgb.FromString(data.ActiveColorValue ?? string.Empty);
                    if (rgb.HasValue) RefreshFromRgb(rgb.Value);
                    break;
                case "Hex":
                    var hex = Hex.FromString(data.ActiveColorValue ?? string.Empty);
                    if (hex.HasValue) RefreshFromHex(hex.Value);
                    break;
                case "Cmy":
                    var cmyk = Cmyk.FromString(data.ActiveColorValue ?? string.Empty);
                    if (cmyk.HasValue) RefreshFromCmyk(cmyk.Value);
                    break;
                case "Hsl":
                    var hsl = Hsl.FromString(data.ActiveColorValue ?? string.Empty);
                    if (hsl.HasValue) RefreshFromHsl(hsl.Value);
                    break;
                case "Hsv":
                    var hsv = Hsv.FromString(data.ActiveColorValue ?? string.Empty);
                    if (hsv.HasValue) RefreshFromHsv(hsv.Value);
                    break;
                case "Lab":
                    var lab = Lab.FromString(data.ActiveColorValue ?? string.Empty);
                    if (lab.HasValue) RefreshFromLab(lab.Value);
                    break;
            }
        }

        public void SaveToPersistedData(PersistedData data, string? activeColorModelTab)
        {
            data.ActiveColorModelTab = activeColorModelTab;
            data.ActiveColorValue = data.ActiveColorModel switch
            {
                "Rgb" => Rgb.ToString(),
                "Hex" => Hex.ToString(),
                "Cmyk" => Cmyk.ToString(),
                "Hsl" => Hsl.ToString(),
                "Hsv" => Hsv.ToString(),
                "Lab" => Lab.ToString(),
                _ => data.ActiveColorValue
            };
        }

        public void RefreshFromRgb(Rgb rgb)
        {
            if (_rgb != Rgb.Empty && rgb == Rgb) return;

            PersistedState.Data().ActiveColorModel = "Rgb";
            PersistedState.Data().ActiveColorValue = rgb.ToString();

            var hsl = rgb.ToHsl();
            var hsv = hsl.ToHsv(); // Less work than RGB to HSV
            var hex = rgb.ToHex();
            var cmyk = rgb.ToCmyk();
            var lab = rgb.ToLab();

            Rgb = rgb;
            Hsl = hsl;
            Hsv = hsv;
            Hex = hex;
            Cmyk = cmyk;
            Lab = lab;
        }

        public void RefreshFromHex(Hex hex)
        {
            if (_hex != Hex.Empty && hex == Hex) return;

            PersistedState.Data().ActiveColorModel = "Hex";
            PersistedState.Data().ActiveColorValue = hex.ToString();

            var rgb = hex.ToRgb();
            var hsl = rgb.ToHsl();
            var hsv = hsl.ToHsv(); // Less work than RGB to HSV
            var cmyk = rgb.ToCmyk();
            var lab = rgb.ToLab();

            Rgb = rgb;
            Hsl = hsl;
            Hsv = hsv;
            Hex = hex;
            Cmyk = cmyk;
            Lab = lab;
        }

        public void RefreshFromHsl(Hsl hsl)
        {
            if (_hsl != Hsl.Empty && hsl == Hsl) return;

            PersistedState.Data().ActiveColorModel = "Hsl";
            PersistedState.Data().ActiveColorValue = hsl.ToString();

            var rgb = hsl.ToRgb();
            var hsv = hsl.ToHsv(); // Less work than RGB to HSV
            var hex = rgb.ToHex();
            var cmyk = rgb.ToCmyk();
            var lab = rgb.ToLab();

            Rgb = rgb;
            Hsl = hsl;
            Hsv = hsv;
            Hex = hex;
            Cmyk = cmyk;
            Lab = lab;
        }

        public void RefreshFromHsv(Hsv hsv)
        {
            if (_hsv != Hsv.Empty && hsv == Hsv) return;

            PersistedState.Data().ActiveColorModel = "Hsv";
            PersistedState.Data().ActiveColorValue = hsv.ToString();

            var rgb = hsv.ToRgb();
            var hsl = hsv.ToHsl(); // Less work than RGB to HSL
            var hex = rgb.ToHex();
            var cmyk = rgb.ToCmyk();
            var lab = rgb.ToLab();

            Rgb = rgb;
            Hsl = hsl;
            Hsv = hsv;
            Hex = hex;
            Cmyk = cmyk;
            Lab = lab;
        }

        public void RefreshFromCmyk(Cmyk cmyk)
        {
            if (_cmyk != Cmyk.Empty && cmyk == Cmyk) return;

            PersistedState.Data().ActiveColorModel = "Cmyk";
            PersistedState.Data().ActiveColorValue = cmyk.ToString();

            var rgb = cmyk.ToRgb();
            var hsl = rgb.ToHsl();
            var hsv = rgb.ToHsv();
            var hex = rgb.ToHex();
            var lab = rgb.ToLab();

            Rgb = rgb;
            Hsl = hsl;
            Hsv = hsv;
            Hex = hex;
            Cmyk = cmyk;
            Lab = lab;
        }

        public void RefreshFromLab(Lab lab)
        {
            if (_lab != Lab.Empty && lab == Lab) return;

            PersistedState.Data().ActiveColorModel = "Lab";
            PersistedState.Data().ActiveColorValue = lab.ToString();

            var rgb = lab.ToRgb();
            var hsl = rgb.ToHsl();
            var hsv = rgb.ToHsv();
            var hex = rgb.ToHex();
            var cmyk = rgb.ToCmyk();

            Rgb = rgb;
            Hsl = hsl;
            Hsv = hsv;
            Hex = hex;
            Cmyk = cmyk;
            Lab = lab;
        }

#region boilerplate

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(PropertyChangedEventArgs eventArgs) => PropertyChanged?.Invoke(this, eventArgs);

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
