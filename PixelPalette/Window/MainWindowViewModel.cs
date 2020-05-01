using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PixelPalette.Annotations;
using PixelPalette.Color;

namespace PixelPalette.Window
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private TabItem _activeColorModelTabItem;

        public TabItem ActiveColorModelTabItem
        {
            get => _activeColorModelTabItem;
            set => SetField(ref _activeColorModelTabItem, value);
        }

        public void LoadFromPersistedData(PersistedData data, TabControl colorModelTabs)
        {
            ActiveColorModelTabItem = colorModelTabs.Items.OfType<TabItem>()
                .SingleOrDefault(t => t.Name == (data.ActiveColorModelTab ?? "RgbTab"));

            switch (data.ActiveColorModelTab)
            {
                case "RgbTab":
                    var rgb = Rgb.FromString(data.ActiveColorValue);
                    if (rgb.HasValue)
                    {
                        RefreshFromRgb(rgb.Value);
                    }

                    break;
                case "HexTab":
                    var hex = Hex.FromString(data.ActiveColorValue);
                    if (hex.HasValue)
                    {
                        RefreshFromHex(hex.Value);
                    }

                    break;
                case "CmykTab":
                    var cmyk = Cmyk.FromString(data.ActiveColorValue);
                    if (cmyk.HasValue)
                    {
                        RefreshFromCmyk(cmyk.Value);
                    }

                    break;
                case "HslTab":
                    var hsl = Hsl.FromString(data.ActiveColorValue);
                    if (hsl.HasValue)
                    {
                        RefreshFromHsl(hsl.Value);
                    }

                    break;
                case "HsvTab":
                    var hsv = Hsv.FromString(data.ActiveColorValue);
                    if (hsv.HasValue)
                    {
                        RefreshFromHsv(hsv.Value);
                    }

                    break;
                case "LabTab":
                    var lab = Lab.FromString(data.ActiveColorValue);
                    if (lab.HasValue)
                    {
                        RefreshFromLab(lab.Value);
                    }

                    break;
            }
        }

        public void SaveToPersistedData(PersistedData data)
        {
            data.ActiveColorModelTab = _activeColorModelTabItem.Name;
            data.ActiveColorValue = data.ActiveColorModelTab switch
            {
                "RgbTab" => Rgb.ToString(),
                "HexTab" => Hex.ToString(),
                "CmykTab" => Cmyk.ToString(),
                "HslTab" => Hsl.ToString(),
                "HsvTab" => Hsv.ToString(),
                "LabTab" => Lab.ToString(),
                _ => data.ActiveColorValue
            };
        }

        private Rgb _rgb = Rgb.Empty;
        private Hsl _hsl = Hsl.Empty;
        private Hsv _hsv = Hsv.Empty;
        private Hex _hex = Hex.Empty;
        private Cmyk _cmyk = Cmyk.Empty;
        private Lab _lab = Lab.Empty;

        private string _hexText;
        private string _hexRed;
        private string _hexGreen;
        private string _hexBlue;

        private string _rgbText;
        private int _red;
        private int _green;
        private int _blue;

        private string _hslText;
        private double _hslHue;
        private double _hslSaturation;
        private double _hslLuminance;
        private double _hslRoundedHue;
        private double _hslRoundedSaturation;
        private double _hslRoundedLuminance;

        private string _hsvText;
        private double _hsvHue;
        private double _hsvSaturation;
        private double _hsvValue;
        private double _hsvRoundedHue;
        private double _hsvRoundedSaturation;
        private double _hsvRoundedValue;

        private string _cmykText;
        private double _cmykCyan;
        private double _cmykMagenta;
        private double _cmykYellow;
        private double _cmykKey;

        private string _labText;
        private double _labL;
        private double _labA;
        private double _labB;

        private LinearGradientBrush _redGradientFill;
        private LinearGradientBrush _greenGradientFill;
        private LinearGradientBrush _blueGradientFill;
        private LinearGradientBrush _hueGradientFill;
        private LinearGradientBrush _hslSaturationGradientFill;
        private LinearGradientBrush _hslLuminanceGradientFill;
        private LinearGradientBrush _hsvSaturationGradientFill;
        private LinearGradientBrush _hsvValueGradientFill;
        private LinearGradientBrush _cmykCyanGradientFill;
        private LinearGradientBrush _cmykMagentaGradientFill;
        private LinearGradientBrush _cmykYellowGradientFill;
        private LinearGradientBrush _cmykKeyGradientFill;
        private LinearGradientBrush _labLGradientFill;
        private LinearGradientBrush _labAGradientFill;
        private LinearGradientBrush _labBGradientFill;

        #region Getters/setters

        public Rgb Rgb
        {
            get => _rgb;
            set => SetField(ref _rgb, value);
        }

        public Hsl Hsl
        {
            get => _hsl;
            set => SetField(ref _hsl, value);
        }

        public Hsv Hsv
        {
            get => _hsv;
            set => SetField(ref _hsv, value);
        }

        public Hex Hex
        {
            get => _hex;
            set => SetField(ref _hex, value);
        }

        public Cmyk Cmyk
        {
            get => _cmyk;
            set => SetField(ref _cmyk, value);
        }

        public Lab Lab
        {
            get => _lab;
            set => SetField(ref _lab, value);
        }

        public string HexText
        {
            get => _hexText;
            set => SetField(ref _hexText, value);
        }

        public string HexRed
        {
            get => _hexRed;
            set => SetField(ref _hexRed, value);
        }

        public string HexGreen
        {
            get => _hexGreen;
            set => SetField(ref _hexGreen, value);
        }

        public string HexBlue
        {
            get => _hexBlue;
            set => SetField(ref _hexBlue, value);
        }


        public string RgbText
        {
            get => _rgbText;
            set => SetField(ref _rgbText, value);
        }

        public int Red
        {
            get => _red;
            set => SetField(ref _red, value);
        }

        public int Green
        {
            get => _green;
            set => SetField(ref _green, value);
        }

        public int Blue
        {
            get => _blue;
            set => SetField(ref _blue, value);
        }

        public string HslText
        {
            get => _hslText;
            set => SetField(ref _hslText, value);
        }

        public double HslHue
        {
            get => _hslHue;
            set => SetField(ref _hslHue, value);
        }

        public double HslSaturation
        {
            get => _hslSaturation;
            set => SetField(ref _hslSaturation, value);
        }

        public double HslLuminance
        {
            get => _hslLuminance;
            set => SetField(ref _hslLuminance, value);
        }

        public double HslRoundedHue
        {
            get => _hslRoundedHue;
            set => SetField(ref _hslRoundedHue, value);
        }

        public double HslRoundedSaturation
        {
            get => _hslRoundedSaturation;
            set => SetField(ref _hslRoundedSaturation, value);
        }

        public double HslRoundedLuminance
        {
            get => _hslRoundedLuminance;
            set => SetField(ref _hslRoundedLuminance, value);
        }

        public string HsvText
        {
            get => _hsvText;
            set => SetField(ref _hsvText, value);
        }

        public double HsvHue
        {
            get => _hsvHue;
            set => SetField(ref _hsvHue, value);
        }

        public double HsvSaturation
        {
            get => _hsvSaturation;
            set => SetField(ref _hsvSaturation, value);
        }

        public double HsvValue
        {
            get => _hsvValue;
            set => SetField(ref _hsvValue, value);
        }

        public double HsvRoundedHue
        {
            get => _hsvRoundedHue;
            set => SetField(ref _hsvRoundedHue, value);
        }

        public double HsvRoundedSaturation
        {
            get => _hsvRoundedSaturation;
            set => SetField(ref _hsvRoundedSaturation, value);
        }

        public double HsvRoundedValue
        {
            get => _hsvRoundedValue;
            set => SetField(ref _hsvRoundedValue, value);
        }

        public string CmykText
        {
            get => _cmykText;
            set => SetField(ref _cmykText, value);
        }

        public double CmykCyan
        {
            get => _cmykCyan;
            set => SetField(ref _cmykCyan, value);
        }

        public double CmykMagenta
        {
            get => _cmykMagenta;
            set => SetField(ref _cmykMagenta, value);
        }

        public double CmykYellow
        {
            get => _cmykYellow;
            set => SetField(ref _cmykYellow, value);
        }

        public double CmykKey
        {
            get => _cmykKey;
            set => SetField(ref _cmykKey, value);
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

        public LinearGradientBrush RedGradientFill
        {
            get => _redGradientFill;
            set => SetField(ref _redGradientFill, value);
        }

        public LinearGradientBrush GreenGradientFill
        {
            get => _greenGradientFill;
            set => SetField(ref _greenGradientFill, value);
        }

        public LinearGradientBrush BlueGradientFill
        {
            get => _blueGradientFill;
            set => SetField(ref _blueGradientFill, value);
        }

        public LinearGradientBrush HueGradientFill
        {
            get => _hueGradientFill;
            set => SetField(ref _hueGradientFill, value);
        }

        public LinearGradientBrush HslSaturationGradientFill
        {
            get => _hslSaturationGradientFill;
            set => SetField(ref _hslSaturationGradientFill, value);
        }

        public LinearGradientBrush HslLuminanceGradientFill
        {
            get => _hslLuminanceGradientFill;
            set => SetField(ref _hslLuminanceGradientFill, value);
        }

        public LinearGradientBrush HsvSaturationGradientFill
        {
            get => _hsvSaturationGradientFill;
            set => SetField(ref _hsvSaturationGradientFill, value);
        }

        public LinearGradientBrush HsvValueGradientFill
        {
            get => _hsvValueGradientFill;
            set => SetField(ref _hsvValueGradientFill, value);
        }

        public LinearGradientBrush CmykCyanGradientFill
        {
            get => _cmykCyanGradientFill;
            set => SetField(ref _cmykCyanGradientFill, value);
        }

        public LinearGradientBrush CmykMagentaGradientFill
        {
            get => _cmykMagentaGradientFill;
            set => SetField(ref _cmykMagentaGradientFill, value);
        }

        public LinearGradientBrush CmykYellowGradientFill
        {
            get => _cmykYellowGradientFill;
            set => SetField(ref _cmykYellowGradientFill, value);
        }

        public LinearGradientBrush CmykKeyGradientFill
        {
            get => _cmykKeyGradientFill;
            set => SetField(ref _cmykKeyGradientFill, value);
        }

        public LinearGradientBrush LabLGradientFill
        {
            get => _labLGradientFill;
            set => SetField(ref _labLGradientFill, value);
        }

        public LinearGradientBrush LabAGradientFill
        {
            get => _labAGradientFill;
            set => SetField(ref _labAGradientFill, value);
        }

        public LinearGradientBrush LabBGradientFill
        {
            get => _labBGradientFill;
            set => SetField(ref _labBGradientFill, value);
        }

        #endregion

        public void RefreshFromRgb(Rgb rgb)
        {
            if (_rgb != Rgb.Empty && rgb == Rgb) return;

            PersistedState.Data.ActiveColorModelTab = "RgbTab";
            PersistedState.Data.ActiveColorValue = rgb.ToString();

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

            RefreshValues();
        }

        public void RefreshFromHex(Hex hex)
        {
            if (_hex != Hex.Empty && hex == Hex) return;

            PersistedState.Data.ActiveColorModelTab = "HexTab";
            PersistedState.Data.ActiveColorValue = hex.ToString();

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

            RefreshValues();
        }

        public void RefreshFromHsl(Hsl hsl)
        {
            if (_hsl != Hsl.Empty && hsl == Hsl) return;

            PersistedState.Data.ActiveColorModelTab = "HslTab";
            PersistedState.Data.ActiveColorValue = hsl.ToString();

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

            RefreshValues();
        }

        public void RefreshFromHsv(Hsv hsv)
        {
            if (_hsv != Hsv.Empty && hsv == Hsv) return;

            PersistedState.Data.ActiveColorModelTab = "HsvTab";
            PersistedState.Data.ActiveColorValue = hsv.ToString();

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

            RefreshValues();
        }

        public void RefreshFromCmyk(Cmyk cmyk)
        {
            if (_cmyk != Cmyk.Empty && cmyk == Cmyk) return;

            PersistedState.Data.ActiveColorModelTab = "CmykTab";
            PersistedState.Data.ActiveColorValue = cmyk.ToString();

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

            RefreshValues();
        }

        public void RefreshFromLab(Lab lab)
        {
            if (_lab != Lab.Empty && lab == Lab) return;

            PersistedState.Data.ActiveColorModelTab = "LabTab";
            PersistedState.Data.ActiveColorValue = lab.ToString();

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

            RefreshValues();
        }

        private void RefreshValues()
        {
            HexText = _hex.ToString();
            HexRed = _hex.RedPart;
            HexGreen = _hex.GreenPart;
            HexBlue = _hex.BluePart;

            RgbText = _rgb.ToString();
            Red = _rgb.Red;
            Green = _rgb.Green;
            Blue = _rgb.Blue;

            HslText = _hsl.ToString();
            HslHue = (int) _hsl.Hue;
            HslSaturation = _hsl.Saturation;
            HslLuminance = _hsl.Luminance;
            HslRoundedHue = _hsl.RoundedHue;
            HslRoundedSaturation = _hsl.RoundedSaturation;
            HslRoundedLuminance = _hsl.RoundedLuminance;

            HsvText = _hsv.ToString();
            HsvHue = (int) _hsv.Hue;
            HsvSaturation = _hsv.Saturation;
            HsvValue = _hsv.Value;
            HsvRoundedHue = _hsv.RoundedHue;
            HsvRoundedSaturation = _hsv.RoundedSaturation;
            HsvRoundedValue = _hsv.RoundedValue;

            CmykText = _cmyk.ToString();
            CmykCyan = _cmyk.RoundedCyan;
            CmykMagenta = _cmyk.RoundedMagenta;
            CmykYellow = _cmyk.RoundedYellow;
            CmykKey = _cmyk.RoundedKey;

            LabText = _lab.ToString();
            LabL = _lab.RoundedL;
            LabA = _lab.RoundedA;
            LabB = _lab.RoundedB;

            static LinearGradientBrush NewBrush() => new LinearGradientBrush
            {
                StartPoint = new Point(0, .5),
                EndPoint = new Point(1, .5)
            };

            var redGradientFill = NewBrush();
            var greenGradientFill = NewBrush();
            var blueGradientFill = NewBrush();
            var hueGradientFill = NewBrush();
            var hslSaturationGradientFill = NewBrush();
            var hslLuminanceGradientFill = NewBrush();
            var hsvSaturationGradientFill = NewBrush();
            var hsvValueGradientFill = NewBrush();
            var cmykCyanGradientFill = NewBrush();
            var cmykMagentaGradientFill = NewBrush();
            var cmykYellowGradientFill = NewBrush();
            var cmykKeyGradientFill = NewBrush();
            var labLGradientFill = NewBrush();
            var labAGradientFill = NewBrush();
            var labBGradientFill = NewBrush();

            redGradientFill.GradientStops.Add(new GradientStop(_rgb.WithRed(0).ToMediaColor(), 0.0));
            redGradientFill.GradientStops.Add(new GradientStop(_rgb.WithRed(255).ToMediaColor(), 1.0));

            greenGradientFill.GradientStops.Add(new GradientStop(_rgb.WithGreen(0).ToMediaColor(), 0.0));
            greenGradientFill.GradientStops.Add(new GradientStop(_rgb.WithGreen(255).ToMediaColor(), 1.0));

            blueGradientFill.GradientStops.Add(new GradientStop(_rgb.WithBlue(0).ToMediaColor(), 0.0));
            blueGradientFill.GradientStops.Add(new GradientStop(_rgb.WithBlue(255).ToMediaColor(), 1.0));

            hueGradientFill.GradientStops.Add(new GradientStop(new Hsl(0, 100, 50).ToMediaColor(), 0.0));
            hueGradientFill.GradientStops.Add(new GradientStop(new Hsl(60, 100, 50).ToMediaColor(), 0.16));
            hueGradientFill.GradientStops.Add(new GradientStop(new Hsl(120, 100, 50).ToMediaColor(), 0.33));
            hueGradientFill.GradientStops.Add(new GradientStop(new Hsl(180, 100, 50).ToMediaColor(), 0.5));
            hueGradientFill.GradientStops.Add(new GradientStop(new Hsl(240, 100, 50).ToMediaColor(), 0.66));
            hueGradientFill.GradientStops.Add(new GradientStop(new Hsl(300, 100, 50).ToMediaColor(), 0.83));
            hueGradientFill.GradientStops.Add(new GradientStop(new Hsl(360, 100, 50).ToMediaColor(), 1.0));

            // HSL gradients
            hslSaturationGradientFill.GradientStops.Add(
                new GradientStop(_hsl.WithSaturation(Hsl.MinSaturation).ToMediaColor(), 0.0));
            hslSaturationGradientFill.GradientStops.Add(
                new GradientStop(_hsl.WithSaturation(Hsl.MaxSaturation).ToMediaColor(), 1.0));

            hslLuminanceGradientFill.GradientStops.Add(
                new GradientStop(_hsl.WithLuminance(Hsl.MinLuminance).ToMediaColor(), 0.0));
            hslLuminanceGradientFill.GradientStops.Add(
                new GradientStop(_hsl.WithLuminance(Hsl.MaxLuminance / 2).ToMediaColor(), 0.5)); // Gives color
            hslLuminanceGradientFill.GradientStops.Add(
                new GradientStop(_hsl.WithLuminance(Hsl.MaxLuminance).ToMediaColor(), 1.0));

            // HSV gradients
            hsvSaturationGradientFill.GradientStops.Add(
                new GradientStop(_hsv.WithSaturation(Hsv.MinSaturation).ToMediaColor(), 0.0));
            hsvSaturationGradientFill.GradientStops.Add(
                new GradientStop(_hsv.WithSaturation(Hsv.MaxSaturation).ToMediaColor(), 1.0));

            hsvValueGradientFill.GradientStops.Add(
                new GradientStop(_hsv.WithValue(Hsv.MinValue).ToMediaColor(), 0.0));
            hsvValueGradientFill.GradientStops.Add(
                new GradientStop(_hsv.WithValue(Hsv.MaxValue / 2).ToMediaColor(), 0.5)); // Gives color
            hsvValueGradientFill.GradientStops.Add(
                new GradientStop(_hsv.WithValue(Hsv.MaxValue).ToMediaColor(), 1.0));

            // CMYK gradients
            cmykCyanGradientFill.GradientStops.Add(
                new GradientStop(_cmyk.WithCyan(Cmyk.MinComponentValue).ToRgb().ToMediaColor(), 0.0));
            cmykCyanGradientFill.GradientStops.Add(
                new GradientStop(_cmyk.WithCyan(Cmyk.MaxComponentValue).ToRgb().ToMediaColor(), 1.0));

            cmykMagentaGradientFill.GradientStops.Add(
                new GradientStop(_cmyk.WithMagenta(Cmyk.MinComponentValue).ToRgb().ToMediaColor(), 0.0));
            cmykMagentaGradientFill.GradientStops.Add(
                new GradientStop(_cmyk.WithMagenta(Cmyk.MaxComponentValue).ToRgb().ToMediaColor(), 1.0));

            cmykYellowGradientFill.GradientStops.Add(
                new GradientStop(_cmyk.WithYellow(Cmyk.MinComponentValue).ToRgb().ToMediaColor(), 0.0));
            cmykYellowGradientFill.GradientStops.Add(
                new GradientStop(_cmyk.WithYellow(Cmyk.MaxComponentValue).ToRgb().ToMediaColor(), 1.0));

            cmykKeyGradientFill.GradientStops.Add(
                new GradientStop(_cmyk.WithKey(Cmyk.MinComponentValue).ToRgb().ToMediaColor(), 0.0));
            cmykKeyGradientFill.GradientStops.Add(
                new GradientStop(_cmyk.WithKey(Cmyk.MaxComponentValue).ToRgb().ToMediaColor(), 1.0));

            // LAB gradients
            labLGradientFill.GradientStops.Add(new GradientStop(_lab.WithL(Lab.MinL).ToRgb().ToMediaColor(), 0.0));
            labLGradientFill.GradientStops.Add(new GradientStop(_lab.WithL(Lab.MaxL).ToRgb().ToMediaColor(), 1.0));

            labAGradientFill.GradientStops.Add(new GradientStop(_lab.WithA(Lab.MinA).ToRgb().ToMediaColor(), 0.0));
            labAGradientFill.GradientStops.Add(new GradientStop(_lab.WithA(Lab.MaxA).ToRgb().ToMediaColor(), 1.0));

            labBGradientFill.GradientStops.Add(new GradientStop(_lab.WithB(Lab.MinB).ToRgb().ToMediaColor(), 0.0));
            labBGradientFill.GradientStops.Add(new GradientStop(_lab.WithB(Lab.MaxB).ToRgb().ToMediaColor(), 1.0));

            RedGradientFill = redGradientFill;
            GreenGradientFill = greenGradientFill;
            BlueGradientFill = blueGradientFill;
            HueGradientFill = hueGradientFill;
            HslSaturationGradientFill = hslSaturationGradientFill;
            HslLuminanceGradientFill = hslLuminanceGradientFill;
            HsvSaturationGradientFill = hsvSaturationGradientFill;
            HsvValueGradientFill = hsvValueGradientFill;
            CmykCyanGradientFill = cmykCyanGradientFill;
            CmykMagentaGradientFill = cmykMagentaGradientFill;
            CmykYellowGradientFill = cmykYellowGradientFill;
            CmykKeyGradientFill = cmykKeyGradientFill;
            LabLGradientFill = labLGradientFill;
            LabAGradientFill = labAGradientFill;
            LabBGradientFill = labBGradientFill;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}