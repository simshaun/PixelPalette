using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        public bool IsUserUpdate = true;

        public TabItem ActiveColorModelTabItem
        {
            get => _activeColorModelTabItem;
            set => SetField(ref _activeColorModelTabItem, value);
        }

        public void LoadFromPersistedData(PersistedData data, TabControl colorModelTabs)
        {
            ActiveColorModelTabItem = colorModelTabs
                                      .Items
                                      .OfType<TabItem>()
                                      .SingleOrDefault(t => t.Name == (data.ActiveColorModelTab ?? "RgbTab"));

            switch (data.ActiveColorModel)
            {
                case "Rgb":
                    var rgb = Rgb.FromString(data.ActiveColorValue);
                    if (rgb.HasValue) RefreshFromRgb(rgb.Value);
                    break;
                case "Hex":
                    var hex = Hex.FromString(data.ActiveColorValue);
                    if (hex.HasValue) RefreshFromHex(hex.Value);
                    break;
                case "Cmy":
                    var cmyk = Cmyk.FromString(data.ActiveColorValue);
                    if (cmyk.HasValue) RefreshFromCmyk(cmyk.Value);
                    break;
                case "Hsl":
                    var hsl = Hsl.FromString(data.ActiveColorValue);
                    if (hsl.HasValue) RefreshFromHsl(hsl.Value);
                    break;
                case "Hsv":
                    var hsv = Hsv.FromString(data.ActiveColorValue);
                    if (hsv.HasValue) RefreshFromHsv(hsv.Value);
                    break;
                case "Lab":
                    var lab = Lab.FromString(data.ActiveColorValue);
                    if (lab.HasValue) RefreshFromLab(lab.Value);
                    break;
            }
        }

        public void SaveToPersistedData(PersistedData data)
        {
            data.ActiveColorModelTab = _activeColorModelTabItem.Name;
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

        private Rgb _rgb = Rgb.Empty;
        private Hsl _hsl = Hsl.Empty;
        private Hsv _hsv = Hsv.Empty;
        private Hex _hex = Hex.Empty;
        private Cmyk _cmyk = Cmyk.Empty;
        private Lab _lab = Lab.Empty;

        private Brush _colorPreviewBrush;

        private string _hexText;
        private string _hexRed;
        private string _hexGreen;
        private string _hexBlue;

        private string _rgbText;
        private string _rgbScaledText;
        private string _rgbRed;
        private string _rgbGreen;
        private string _rgbBlue;
        private string _rgbScaledRed;
        private string _rgbScaledGreen;
        private string _rgbScaledBlue;

        private string _hslText;
        private string _hslScaledText;
        private string _hslHue;
        private string _hslSaturation;
        private string _hslLuminance;
        private string _hslScaledHue;
        private string _hslScaledSaturation;
        private string _hslScaledLuminance;

        private string _hsvText;
        private string _hsvScaledText;
        private string _hsvHue;
        private string _hsvSaturation;
        private string _hsvValue;
        private string _hsvScaledHue;
        private string _hsvScaledSaturation;
        private string _hsvScaledValue;

        private string _cmykText;
        private string _cmykScaledText;
        private string _cmykCyan;
        private string _cmykMagenta;
        private string _cmykYellow;
        private string _cmykKey;
        private string _cmykScaledCyan;
        private string _cmykScaledMagenta;
        private string _cmykScaledYellow;
        private string _cmykScaledKey;

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

        public Brush ColorPreviewBrush
        {
            get => _colorPreviewBrush;
            set => SetField(ref _colorPreviewBrush, value);
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

        public string RgbScaledText
        {
            get => _rgbScaledText;
            set => SetField(ref _rgbScaledText, value);
        }

        public string RgbRed
        {
            get => _rgbRed;
            set => SetField(ref _rgbRed, value);
        }

        public string RgbGreen
        {
            get => _rgbGreen;
            set => SetField(ref _rgbGreen, value);
        }

        public string RgbBlue
        {
            get => _rgbBlue;
            set => SetField(ref _rgbBlue, value);
        }

        public string RgbScaledRed
        {
            get => _rgbScaledRed;
            set => SetField(ref _rgbScaledRed, value);
        }

        public string RgbScaledGreen
        {
            get => _rgbScaledGreen;
            set => SetField(ref _rgbScaledGreen, value);
        }

        public string RgbScaledBlue
        {
            get => _rgbScaledBlue;
            set => SetField(ref _rgbScaledBlue, value);
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

        public string CmykText
        {
            get => _cmykText;
            set => SetField(ref _cmykText, value);
        }

        public string CmykScaledText
        {
            get => _cmykScaledText;
            set => SetField(ref _cmykScaledText, value);
        }

        public string CmykCyan
        {
            get => _cmykCyan;
            set => SetField(ref _cmykCyan, value);
        }

        public string CmykMagenta
        {
            get => _cmykMagenta;
            set => SetField(ref _cmykMagenta, value);
        }

        public string CmykYellow
        {
            get => _cmykYellow;
            set => SetField(ref _cmykYellow, value);
        }

        public string CmykKey
        {
            get => _cmykKey;
            set => SetField(ref _cmykKey, value);
        }

        public string CmykScaledCyan
        {
            get => _cmykScaledCyan;
            set => SetField(ref _cmykScaledCyan, value);
        }

        public string CmykScaledMagenta
        {
            get => _cmykScaledMagenta;
            set => SetField(ref _cmykScaledMagenta, value);
        }

        public string CmykScaledYellow
        {
            get => _cmykScaledYellow;
            set => SetField(ref _cmykScaledYellow, value);
        }

        public string CmykScaledKey
        {
            get => _cmykScaledKey;
            set => SetField(ref _cmykScaledKey, value);
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

            PersistedState.Data.ActiveColorModel = "Rgb";
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

            PersistedState.Data.ActiveColorModel = "Hex";
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

            PersistedState.Data.ActiveColorModel = "Hsl";
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

            PersistedState.Data.ActiveColorModel = "Hsv";
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

            PersistedState.Data.ActiveColorModel = "Cmyk";
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

            PersistedState.Data.ActiveColorModel = "Lab";
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
            IsUserUpdate = false;

            ColorPreviewBrush = new SolidColorBrush(_rgb.ToMediaColor());

            HexText = _hex.ToString();
            HexRed = _hex.RedPart;
            HexGreen = _hex.GreenPart;
            HexBlue = _hex.BluePart;

            RgbText = _rgb.ToString();
            RgbScaledText = _rgb.ToScaledString();
            RgbRed = _rgb.RoundedRed.ToString(CultureInfo.InvariantCulture);
            RgbGreen = _rgb.RoundedGreen.ToString(CultureInfo.InvariantCulture);
            RgbBlue = _rgb.RoundedBlue.ToString(CultureInfo.InvariantCulture);
            RgbScaledRed = _rgb.ScaledRed.ToString();
            RgbScaledGreen = _rgb.ScaledGreen.ToString();
            RgbScaledBlue = _rgb.ScaledBlue.ToString();

            HslText = _hsl.ToString();
            HslScaledText = _hsl.ToScaledString();
            HslHue = _hsl.RoundedHue.ToString(CultureInfo.InvariantCulture);
            HslSaturation = _hsl.RoundedSaturation.ToString(CultureInfo.InvariantCulture);
            HslLuminance = _hsl.RoundedLuminance.ToString(CultureInfo.InvariantCulture);
            HslScaledHue = _hsl.RoundedScaledHue.ToString(CultureInfo.InvariantCulture);
            HslScaledSaturation = _hsl.RoundedScaledSaturation.ToString(CultureInfo.InvariantCulture);
            HslScaledLuminance = _hsl.RoundedScaledLuminance.ToString(CultureInfo.InvariantCulture);

            HsvText = _hsv.ToString();
            HsvScaledText = _hsv.ToScaledString();
            HsvHue = _hsv.RoundedHue.ToString(CultureInfo.InvariantCulture);
            HsvSaturation = _hsv.RoundedSaturation.ToString(CultureInfo.InvariantCulture);
            HsvValue = _hsv.RoundedValue.ToString(CultureInfo.InvariantCulture);
            HsvScaledHue = _hsv.RoundedScaledHue.ToString(CultureInfo.InvariantCulture);
            HsvScaledSaturation = _hsv.RoundedScaledSaturation.ToString(CultureInfo.InvariantCulture);
            HsvScaledValue = _hsv.RoundedScaledValue.ToString(CultureInfo.InvariantCulture);

            CmykText = _cmyk.ToString();
            CmykScaledText = _cmyk.ToScaledString();
            CmykCyan = _cmyk.RoundedCyan.ToString(CultureInfo.InvariantCulture);
            CmykMagenta = _cmyk.RoundedMagenta.ToString(CultureInfo.InvariantCulture);
            CmykYellow = _cmyk.RoundedYellow.ToString(CultureInfo.InvariantCulture);
            CmykKey = _cmyk.RoundedKey.ToString(CultureInfo.InvariantCulture);
            CmykScaledCyan = _cmyk.RoundedScaledCyan.ToString(CultureInfo.InvariantCulture);
            CmykScaledMagenta = _cmyk.RoundedScaledMagenta.ToString(CultureInfo.InvariantCulture);
            CmykScaledYellow = _cmyk.RoundedScaledYellow.ToString(CultureInfo.InvariantCulture);
            CmykScaledKey = _cmyk.RoundedScaledKey.ToString(CultureInfo.InvariantCulture);

            LabText = _lab.ToString();
            LabL = _lab.RoundedL;
            LabA = _lab.RoundedA;
            LabB = _lab.RoundedB;

            static LinearGradientBrush NewBrush() =>
                new LinearGradientBrush
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

            redGradientFill.GradientStops.Add(new GradientStop(_rgb.WithRed(0.0).ToMediaColor(), 0.0));
            redGradientFill.GradientStops.Add(new GradientStop(_rgb.WithRed(1.0).ToMediaColor(), 1.0));
            greenGradientFill.GradientStops.Add(new GradientStop(_rgb.WithGreen(0.0).ToMediaColor(), 0.0));
            greenGradientFill.GradientStops.Add(new GradientStop(_rgb.WithGreen(1.0).ToMediaColor(), 1.0));
            blueGradientFill.GradientStops.Add(new GradientStop(_rgb.WithBlue(0.0).ToMediaColor(), 0.0));
            blueGradientFill.GradientStops.Add(new GradientStop(_rgb.WithBlue(1.0).ToMediaColor(), 1.0));

            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(0, 100, 50).ToMediaColor(), 0.0));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(60, 100, 50).ToMediaColor(), 0.16));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(120, 100, 50).ToMediaColor(), 0.33));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(180, 100, 50).ToMediaColor(), 0.5));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(240, 100, 50).ToMediaColor(), 0.66));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(300, 100, 50).ToMediaColor(), 0.83));
            hueGradientFill.GradientStops.Add(new GradientStop(Hsl.FromScaledValues(360, 100, 50).ToMediaColor(), 1.0));

            // HSL gradients
            hslSaturationGradientFill.GradientStops.Add(new GradientStop(_hsl.WithSaturation(0.0).ToMediaColor(), 0.0));
            hslSaturationGradientFill.GradientStops.Add(new GradientStop(_hsl.WithSaturation(1.0).ToMediaColor(), 1.0));

            hslLuminanceGradientFill.GradientStops.Add(new GradientStop(_hsl.WithLuminance(0.0).ToMediaColor(), 0.0));
            hslLuminanceGradientFill.GradientStops.Add(new GradientStop(_hsl.WithLuminance(0.5).ToMediaColor(), 0.5)); // Gives color
            hslLuminanceGradientFill.GradientStops.Add(new GradientStop(_hsl.WithLuminance(1.0).ToMediaColor(), 1.0));

            // HSV gradients
            hsvSaturationGradientFill.GradientStops.Add(new GradientStop(_hsv.WithSaturation(0.0).ToMediaColor(), 0.0));
            hsvSaturationGradientFill.GradientStops.Add(new GradientStop(_hsv.WithSaturation(1.0).ToMediaColor(), 1.0));

            hsvValueGradientFill.GradientStops.Add(new GradientStop(_hsv.WithValue(0.0).ToMediaColor(), 0.0));
            hsvValueGradientFill.GradientStops.Add(new GradientStop(_hsv.WithValue(0.5).ToMediaColor(), 0.5)); // Gives color
            hsvValueGradientFill.GradientStops.Add(new GradientStop(_hsv.WithValue(1.0).ToMediaColor(), 1.0));

            // CMYK gradients
            cmykCyanGradientFill.GradientStops.Add(new GradientStop(_cmyk.WithCyan(0.0).ToRgb().ToMediaColor(), 0.0));
            cmykCyanGradientFill.GradientStops.Add(new GradientStop(_cmyk.WithCyan(1.0).ToRgb().ToMediaColor(), 1.0));
            cmykMagentaGradientFill.GradientStops.Add(new GradientStop(_cmyk.WithMagenta(0.0).ToRgb().ToMediaColor(), 0.0));
            cmykMagentaGradientFill.GradientStops.Add(new GradientStop(_cmyk.WithMagenta(1.0).ToRgb().ToMediaColor(), 1.0));
            cmykYellowGradientFill.GradientStops.Add(new GradientStop(_cmyk.WithYellow(0.0).ToRgb().ToMediaColor(), 0.0));
            cmykYellowGradientFill.GradientStops.Add(new GradientStop(_cmyk.WithYellow(1.0).ToRgb().ToMediaColor(), 1.0));
            cmykKeyGradientFill.GradientStops.Add(new GradientStop(_cmyk.WithKey(0.0).ToRgb().ToMediaColor(), 0.0));
            cmykKeyGradientFill.GradientStops.Add(new GradientStop(_cmyk.WithKey(1.0).ToRgb().ToMediaColor(), 1.0));

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

            IsUserUpdate = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (IsUserUpdate)
            {
                PropertyChangedByUser?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event EventHandler<PropertyChangedEventArgs> PropertyChangedByUser;
    }
}