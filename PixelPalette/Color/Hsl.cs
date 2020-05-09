using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PixelPalette.Color
{
    public readonly struct Hsl
    {
        public static readonly Hsl Empty = new Hsl();

        private const string StringPattern =
            @"^hsl\(\s*(?<hue>0|1(?:\.0)?|0?\.\d+)\s*,\s*(?<sat>0|1(?:\.0)?|0\.\d+)\s*,\s*(?<lum>0|1(?:\.0)?|0\.\d+)\s*\)$";

        private const string ScaledStringPattern =
            @"^hsl\(\s*(?<hue>\d+(?:\.\d+)?)\s*,\s*(?<sat>\d+(?:\.\d+)?)%?\s*,\s*(?<lum>\d+(?:\.\d+)?)%?\s*\)$";

        /// <summary>
        /// Hue on a scale of 0-1
        /// </summary>
        public double Hue { get; }

        /// <summary>
        /// Saturation on a scale of 0-1
        /// </summary>
        public double Saturation { get; }

        /// <summary>
        /// Luminance on a scale of 0-1
        /// </summary>
        public double Luminance { get; }

        /// <summary>
        /// Hue on a scale of 0-360
        /// </summary>
        public double ScaledHue => Hue * 360.0;

        /// <summary>
        /// Saturation on a scale of 0-100
        /// </summary>
        public double ScaledSaturation => Saturation * 100.0;

        /// <summary>
        /// Luminance on a scale of 0-100
        /// </summary>
        public double ScaledLuminance => Luminance * 100.0;

        /// <summary>
        /// Hue on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedHue => Round(Hue);

        /// <summary>
        /// Saturation on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedSaturation => Round(Saturation);

        /// <summary>
        /// Luminance on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedLuminance => Round(Luminance);

        /// <summary>
        /// Hue on a scale of 0-360, rounded to 2 decimal places
        /// </summary>
        public double RoundedScaledHue => Round(ScaledHue, 2);

        /// <summary>
        /// Saturation on a scale of 0-100, rounded to 2 decimal places
        /// </summary>
        public double RoundedScaledSaturation => Round(ScaledSaturation, 2);

        /// <summary>
        /// Luminance on a scale of 0-100, rounded to 2 decimal places
        /// </summary>
        public double RoundedScaledLuminance => Round(ScaledLuminance, 2);

        /// <param name="h">0-1</param>
        /// <param name="s">0-1</param>
        /// <param name="l">0-1</param>
        public Hsl(double h, double s, double l)
        {
            Hue = ClampedHue(h);
            Saturation = ClampedSaturation(s);
            Luminance = ClampedLuminance(l);
        }

        /// <param name="h">0-360</param>
        /// <param name="s">0-100</param>
        /// <param name="l">0-100</param>
        public static Hsl FromScaledValues(double h, double s, double l)
        {
            return new Hsl(
                ClampedScaledHue(h) / 360.0,
                ClampedScaledSaturation(s) / 100.0,
                ClampedScaledLuminance(l) / 100.0);
        }

        public static bool IsValidHue(double value)
        {
            return value >= 0.0 && value <= 1.0;
        }

        public static bool IsValidSaturation(double value)
        {
            return value >= 0.0 && value <= 1.0;
        }

        public static bool IsValidLuminance(double value)
        {
            return value >= 0.0 && value <= 1.0;
        }

        public static bool IsValidScaledHue(double value)
        {
            return value >= 0.0 && value <= 360.0;
        }

        public static bool IsValidScaledSaturation(double value)
        {
            return value >= 0.0 && value <= 100.0;
        }

        public static bool IsValidScaledLuminance(double value)
        {
            return value >= 0.0 && value <= 100.0;
        }

        public static Hsl? FromString(string theString)
        {
            var regex = new Regex(StringPattern, RegexOptions.IgnoreCase);
            var match = regex.Match(theString);
            if (match.Success)
            {
                return new Hsl(
                    double.Parse(match.Groups["hue"].Value),
                    double.Parse(match.Groups["sat"].Value),
                    double.Parse(match.Groups["lum"].Value)
                );
            }

            return null;
        }

        public static Hsl? FromScaledString(string theString)
        {
            var regex = new Regex(ScaledStringPattern, RegexOptions.IgnoreCase);
            var match = regex.Match(theString);
            if (!match.Success) return null;
            var h = double.Parse(match.Groups["hue"].Value);
            var s = double.Parse(match.Groups["sat"].Value);
            var l = double.Parse(match.Groups["lum"].Value);
            if (!IsValidScaledHue(h) || !IsValidScaledSaturation(s) || !IsValidScaledLuminance(l)) return null;
            return FromScaledValues(h, s, l);
        }

        public static double ClampedHue(double h)
        {
            return h > 1.0 ? 1.0 : h < 0.0 ? 0.0 : h;
        }

        public static double ClampedSaturation(double s)
        {
            return s > 1.0 ? 1.0 : s < 0.0 ? 0.0 : s;
        }

        public static double ClampedLuminance(double l)
        {
            return l > 1.0 ? 1.0 : l < 0.0 ? 0.0 : l;
        }

        public static double ClampedScaledHue(double h)
        {
            return h > 360.0 ? 360.0 : h < 0.0 ? 0.0 : h;
        }

        public static double ClampedScaledSaturation(double s)
        {
            return s > 100.0 ? 100.0 : s < 0.0 ? 0.0 : s;
        }

        public static double ClampedScaledLuminance(double l)
        {
            return l > 100.0 ? 100.0 : l < 0.0 ? 0.0 : l;
        }

        public Hsl WithHue(double h)
        {
            return new Hsl(h, Saturation, Luminance);
        }

        public Hsl WithSaturation(double s)
        {
            return new Hsl(Hue, s, Luminance);
        }

        public Hsl WithLuminance(double l)
        {
            return new Hsl(Hue, Saturation, l);
        }

        public Hsl WithScaledHue(double h)
        {
            return new Hsl(h / 360.0, Saturation, Luminance);
        }

        public Hsl WithScaledSaturation(double s)
        {
            return new Hsl(Hue, s / 100.0, Luminance);
        }

        public Hsl WithScaledLuminance(double l)
        {
            return new Hsl(Hue, Saturation, l / 100.0);
        }

        private static double Round(double d, int precision = 3)
        {
            return Math.Round(d, precision, MidpointRounding.AwayFromZero);
        }

        public override string ToString()
        {
            return $"hsl({RoundedHue}, {RoundedSaturation}, {RoundedLuminance})";
        }

        public string ToScaledString()
        {
            return $"hsl({RoundedScaledHue}, {RoundedScaledSaturation}%, {RoundedScaledLuminance}%)";
        }

        /// <summary>
        /// Lighten the color by X amount (0-100)
        /// </summary>
        public Hsl Lighter(int amount = 10)
        {
            return new Hsl(Hue, Saturation, Round(Luminance + amount / 100.0));
        }

        /// <summary>
        /// Darken the color by X amount (0-100)
        /// </summary>
        public Hsl Darker(int amount = 10)
        {
            return new Hsl(Hue, Saturation, Round(Luminance - amount / 100.0));
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Rgb ToRgb()
        {
            // Formula from https://www.rapidtables.com/convert/color/hsl-to-rgb.html

            var hue = ScaledHue;
            var sat = Saturation;
            var lum = Luminance;

            // 360deg is actually 0deg (Red)
            if (hue >= 360)
            {
                hue = 0;
            }

            var C = (1 - Math.Abs(2 * lum - 1)) * sat;
            var X = C * (1 - Math.Abs(hue / 60 % 2 - 1));
            var m = lum - C / 2;

            var r1 = 0.0;
            var g1 = 0.0;
            var b1 = 0.0;

            if (hue >= 0 && hue < 60)
            {
                r1 = C;
                g1 = X;
                b1 = 0;
            }
            else if (hue >= 60 && hue < 120)
            {
                r1 = X;
                g1 = C;
                b1 = 0;
            }
            else if (hue >= 120 && hue < 180)
            {
                r1 = 0;
                g1 = C;
                b1 = X;
            }
            else if (hue >= 180 && hue < 240)
            {
                r1 = 0;
                g1 = X;
                b1 = C;
            }
            else if (hue >= 240 && hue < 300)
            {
                r1 = X;
                g1 = 0;
                b1 = C;
            }
            else if (hue >= 300 && hue < 360)
            {
                r1 = C;
                g1 = 0;
                b1 = X;
            }

            return new Rgb(Rgb.ClampedComponent(r1 + m), Rgb.ClampedComponent(g1 + m), Rgb.ClampedComponent(b1 + m));
        }

        public Hsv ToHsv()
        {
            // Formula from https://en.wikipedia.org/wiki/HSL_and_HSV#HSL_to_HSV

            var hue = ScaledHue;
            var sat1 = Saturation;
            var lum1 = Luminance;

            var val = lum1 + sat1 * Math.Min(lum1, 1 - lum1);
            var sat = 0.0;
            if (val > 0)
            {
                sat = 2 * (1 - lum1 / val);
            }

            return new Hsv(hue / 360.0, sat, val);
        }

        public System.Windows.Media.Color ToMediaColor()
        {
            return ToRgb().ToMediaColor();
        }

        public bool Equals(Hsl other)
        {
            return Hue.Equals(other.Hue) &&
                   Saturation.Equals(other.Saturation) &&
                   Luminance.Equals(other.Luminance);
        }

        public override bool Equals(object obj)
        {
            return obj is Hsl other && Equals(other);
        }

        public static bool operator ==(Hsl item1, Hsl item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(Hsl item1, Hsl item2)
        {
            return !item1.Equals(item2);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return HashCode.Combine(Hue, Saturation, Luminance);
        }
    }
}