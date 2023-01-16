using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace PixelPalette.Color;

public readonly partial struct Hex
{
    public static readonly Hex Empty = new();

    public int Red { get; }
    public int Green { get; }
    public int Blue { get; }

    public string RedPart { get; }
    public string GreenPart { get; }
    public string BluePart { get; }

    public Hex(int r, int g, int b)
    {
        Red = r > 255 ? 255 : r < 0 ? 0 : r;
        Green = g > 255 ? 255 : g < 0 ? 0 : g;
        Blue = b > 255 ? 255 : b < 0 ? 0 : b;
        // ReSharper disable UseFormatSpecifierInInterpolation
        RedPart = Red.ToString("x2").ToUpper();
        GreenPart = Green.ToString("x2").ToUpper();
        BluePart = Blue.ToString("x2").ToUpper();
        // ReSharper restore UseFormatSpecifierInInterpolation
    }

    public Hex WithRed(int r)
    {
        return new Hex(r, Green, Blue);
    }

    public Hex WithRed(string r)
    {
        return new Hex(ClampedComponent(r), Green, Blue);
    }

    public Hex WithGreen(int g)
    {
        return new Hex(Red, g, Blue);
    }

    public Hex WithGreen(string g)
    {
        return new Hex(Red, ClampedComponent(g), Blue);
    }

    public Hex WithBlue(int b)
    {
        return new Hex(Red, Green, b);
    }

    public Hex WithBlue(string b)
    {
        return new Hex(Red, Green, ClampedComponent(b));
    }

    public Hex(string hex)
    {
        if (!IsValidHex(hex))
        {
            throw new ArgumentException($"{hex} is not a valid hex");
        }

        // Expand shorthand form (e.g. "03F") to full form (e.g. "0033FF")
        var expandedHex =
            ShorthandRegex()
                .Replace(hex, match =>
                {
                    var r = match.Groups[1].Value;
                    var g = match.Groups[2].Value;
                    var b = match.Groups[3].Value;
                    return r + r + g + g + b + b;
                });

        var result = ExpandedHexRegex().Match(expandedHex);

        Red = int.Parse(result.Groups[1].Value, NumberStyles.HexNumber);
        Green = int.Parse(result.Groups[2].Value, NumberStyles.HexNumber);
        Blue = int.Parse(result.Groups[3].Value, NumberStyles.HexNumber);
        // ReSharper disable UseFormatSpecifierInInterpolation
        RedPart = Red.ToString("x2").ToUpper();
        GreenPart = Green.ToString("x2").ToUpper();
        BluePart = Blue.ToString("x2").ToUpper();
        // ReSharper restore UseFormatSpecifierInInterpolation
    }

    public static Hex? From6CharString(string theString)
    {
        if (!IsValid6CharHex(theString)) return null;
        return new Hex(theString);
    }

    public static Hex? FromString(string theString)
    {
        try
        {
            return new Hex(theString);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    public override string ToString()
    {
        return "#" + (RedPart + GreenPart + BluePart).ToUpper();
    }

    public Rgb ToRgb()
    {
        return Rgb.FromScaledValues(Red, Green, Blue);
    }

    public static bool IsValidHex(string hex)
    {
        return HexRegex().IsMatch(hex);
    }

    public static bool IsValid6CharHex(string hex)
    {
        return SixCharHexRegex().IsMatch(hex);
    }

    public static bool IsValidHexPart(string hexPart)
    {
        var isInt = int.TryParse(hexPart, NumberStyles.HexNumber, null, out var partAsInt);

        return isInt && partAsInt is >= 0 and <= 255;
    }

    public static int ClampedComponent(int c)
    {
        return c > 255 ? 255 : c < 0 ? 0 : c;
    }

    public static int ClampedComponent(string c)
    {
        var isInt = int.TryParse(c, NumberStyles.HexNumber, null, out var value);
        return !isInt ? 0 : ClampedComponent(value);
    }

    public bool Equals(Hex other)
    {
        return Red == other.Red && Green == other.Green && Blue == other.Blue;
    }

    public override bool Equals(object? obj)
    {
        return obj is Hex other && Equals(other);
    }

    public static bool operator ==(Hex item1, Hex item2)
    {
        return item1.Equals(item2);
    }

    public static bool operator !=(Hex item1, Hex item2)
    {
        return !item1.Equals(item2);
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        return HashCode.Combine(Red, Green, Blue);
    }

    [GeneratedRegex("^#?([0-9A-F]{3}){1,2}$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex HexRegex();

    [GeneratedRegex("^#?([0-9A-F]{6})$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex SixCharHexRegex();

    [GeneratedRegex("^#?([a-f\\d])([a-f\\d])([a-f\\d])$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex ShorthandRegex();

    [GeneratedRegex("^#?([a-f\\d]{2})([a-f\\d]{2})([a-f\\d]{2})$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex ExpandedHexRegex();
}
