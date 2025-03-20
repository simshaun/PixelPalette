using CommunityToolkit.Mvvm.ComponentModel;
using ColorModel = PixelPalette.Color;

namespace PixelPalette.State;

public partial class GlobalState : ObservableObject
{
    private static GlobalState? _instance;
    public static GlobalState Instance => _instance ??= new GlobalState();

    [ObservableProperty] private bool _historyVisible;

    [ObservableProperty] private ColorModel.Rgb _rgb = ColorModel.Rgb.Empty;
    [ObservableProperty] private ColorModel.Hsl _hsl = ColorModel.Hsl.Empty;
    [ObservableProperty] private ColorModel.Hsv _hsv = ColorModel.Hsv.Empty;
    [ObservableProperty] private ColorModel.Hex _hex = ColorModel.Hex.Empty;
    [ObservableProperty] private ColorModel.Cmyk _cmyk = ColorModel.Cmyk.Empty;
    [ObservableProperty] private ColorModel.Lab _lab = ColorModel.Lab.Empty;

    public void LoadFromPersistedData(PersistedData data)
    {
        switch (data.ActiveColorModel)
        {
            case "Rgb":
                var rgb = ColorModel.Rgb.FromString(data.ActiveColorValue ?? string.Empty);
                if (rgb.HasValue) RefreshFromRgb(rgb.Value);
                break;
            case "Hex":
                var hex = ColorModel.Hex.FromString(data.ActiveColorValue ?? string.Empty);
                if (hex.HasValue) RefreshFromHex(hex.Value);
                break;
            case "Cmyk":
                var cmyk = ColorModel.Cmyk.FromString(data.ActiveColorValue ?? string.Empty);
                if (cmyk.HasValue) RefreshFromCmyk(cmyk.Value);
                break;
            case "Hsl":
                var hsl = ColorModel.Hsl.FromString(data.ActiveColorValue ?? string.Empty);
                if (hsl.HasValue) RefreshFromHsl(hsl.Value);
                break;
            case "Hsv":
                var hsv = ColorModel.Hsv.FromString(data.ActiveColorValue ?? string.Empty);
                if (hsv.HasValue) RefreshFromHsv(hsv.Value);
                break;
            case "Lab":
                var lab = ColorModel.Lab.FromString(data.ActiveColorValue ?? string.Empty);
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
            _ => data.ActiveColorValue,
        };
    }

    public void RefreshFromRgb(ColorModel.Rgb rgb)
    {
        if (Rgb != ColorModel.Rgb.Empty && rgb == Rgb) return;

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

    public void RefreshFromHex(ColorModel.Hex hex)
    {
        if (Hex != ColorModel.Hex.Empty && hex == Hex) return;

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

    public void RefreshFromHsl(ColorModel.Hsl hsl)
    {
        if (Hsl != ColorModel.Hsl.Empty && hsl == Hsl) return;

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

    public void RefreshFromHsv(ColorModel.Hsv hsv)
    {
        if (Hsv != ColorModel.Hsv.Empty && hsv == Hsv) return;

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

    public void RefreshFromCmyk(ColorModel.Cmyk cmyk)
    {
        if (Cmyk != ColorModel.Cmyk.Empty && cmyk == Cmyk) return;

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

    public void RefreshFromLab(ColorModel.Lab lab)
    {
        if (Lab != ColorModel.Lab.Empty && lab == Lab) return;

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
}
