using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PixelPalette.Color
{
    /// <summary>
    /// HSV/HSB (same thing)
    /// </summary>
    public readonly struct Hsv : IColor
    {
        public static readonly Hsv Empty = new Hsv();

        private const string StringPattern =
            @"^hsv\(\s*(?<hue>0|1(?:\.0)?|0?\.\d+)\s*,\s*(?<sat>0|1(?:\.0)|0\.\d+)\s*,\s*(?<val>0|1(?:\.0)|0\.\d+)\s*\)$";

        private const string ScaledStringPattern =
            @"^hsv\(\s*(?<hue>\d+(?:\.\d+)?)\s*,\s*(?<sat>\d+(?:\.\d+)?)%?\s*,\s*(?<val>\d+(?:\.\d+)?)%?\s*\)$";

        /// <summary>
        /// Hue on a scale of 0-1
        /// </summary>
        public double Hue { get; }

        /// <summary>
        /// Saturation on a scale of 0-1
        /// </summary>
        public double Saturation { get; }

        /// <summary>
        /// Value on a scale of 0-1
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Hue on a scale of 0-360
        /// </summary>
        public double ScaledHue => Hue * 360.0;

        /// <summary>
        /// Saturation on a scale of 0-100
        /// </summary>
        public double ScaledSaturation => Saturation * 100.0;

        /// <summary>
        /// Value on a scale of 0-100
        /// </summary>
        public double ScaledValue => Value * 100.0;

        /// <summary>
        /// Hue on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedHue => Round(Hue);

        /// <summary>
        /// Saturation on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedSaturation => Round(Saturation);

        /// <summary>
        /// Value on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedValue => Round(Value);

        /// <summary>
        /// Hue on a scale of 0-360, rounded to 2 decimal places
        /// </summary>
        public double RoundedScaledHue => Round(ScaledHue, 2);

        /// <summary>
        /// Saturation on a scale of 0-100, rounded to 2 decimal places
        /// </summary>
        public double RoundedScaledSaturation => Round(ScaledSaturation, 2);

        /// <summary>
        /// Value on a scale of 0-100, rounded to 2 decimal places
        /// </summary>
        public double RoundedScaledValue => Round(ScaledValue, 2);

        /// <param name="h">0-1</param>
        /// <param name="s">0-1</param>
        /// <param name="v">0-1</param>
        public Hsv(double h, double s, double v)
        {
            Hue = ClampedHue(h);
            Saturation = ClampedSaturation(s);
            Value = ClampedValue(v);
        }

        /// <param name="h">0-360</param>
        /// <param name="s">0-100</param>
        /// <param name="v">0-100</param>
        public static Hsv FromScaledValues(double h, double s, double v)
        {
            return new Hsv(
                ClampedScaledHue(h) / 360.0,
                ClampedScaledSaturation(s) / 100.0,
                ClampedScaledValue(v) / 100.0);
        }

        public static bool IsValidHue(double value)
        {
            return value >= 0.0 && value <= 1.0;
        }

        public static bool IsValidSaturation(double value)
        {
            return value >= 0.0 && value <= 1.0;
        }

        public static bool IsValidValue(double value)
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

        public static bool IsValidScaledValue(double value)
        {
            return value >= 0.0 && value <= 100.0;
        }

        public static Hsv? FromString(string theString)
        {
            var regex = new Regex(StringPattern, RegexOptions.IgnoreCase);
            var match = regex.Match(theString);
            if (match.Success)
            {
                return new Hsv(
                    double.Parse(match.Groups["hue"].Value),
                    double.Parse(match.Groups["sat"].Value),
                    double.Parse(match.Groups["val"].Value)
                );
            }

            return null;
        }

        public static Hsv? FromScaledString(string theString)
        {
            var regex = new Regex(ScaledStringPattern, RegexOptions.IgnoreCase);
            var match = regex.Match(theString);
            if (!match.Success) return null;
            var h = double.Parse(match.Groups["hue"].Value);
            var s = double.Parse(match.Groups["sat"].Value);
            var v = double.Parse(match.Groups["val"].Value);
            if (!IsValidScaledHue(h) || !IsValidScaledSaturation(s) || !IsValidScaledValue(v)) return null;
            return FromScaledValues(h, s, v);
        }

        public static double ClampedHue(double h)
        {
            return h > 1.0 ? 1.0 : h < 0.0 ? 0.0 : h;
        }

        public static double ClampedSaturation(double s)
        {
            return s > 1.0 ? 1.0 : s < 0.0 ? 0.0 : s;
        }

        public static double ClampedValue(double v)
        {
            return v > 1.0 ? 1.0 : v < 0.0 ? 0.0 : v;
        }

        public static double ClampedScaledHue(double h)
        {
            return h > 360.0 ? 360.0 : h < 0.0 ? 0.0 : h;
        }

        public static double ClampedScaledSaturation(double s)
        {
            return s > 100.0 ? 100.0 : s < 0.0 ? 0.0 : s;
        }

        public static double ClampedScaledValue(double v)
        {
            return v > 100.0 ? 100.0 : v < 0.0 ? 0.0 : v;
        }

        public Hsv WithHue(double h)
        {
            return new Hsv(h, Saturation, Value);
        }

        public Hsv WithSaturation(double s)
        {
            return new Hsv(Hue, s, Value);
        }

        public Hsv WithValue(double v)
        {
            return new Hsv(Hue, Saturation, v);
        }

        public Hsv WithScaledHue(double h)
        {
            return new Hsv(h / 360.0, Saturation, Value);
        }

        public Hsv WithScaledSaturation(double s)
        {
            return new Hsv(Hue, s / 100.0, Value);
        }

        public Hsv WithScaledValue(double v)
        {
            return new Hsv(Hue, Saturation, v / 100.0);
        }

        private static double Round(double d, int precision = 3)
        {
            return Math.Round(d, precision, MidpointRounding.AwayFromZero);
        }

        public override string ToString()
        {
            return $"hsv({RoundedHue}, {RoundedSaturation}, {RoundedValue})";
        }

        public string ToScaledString()
        {
            return $"hsv({RoundedScaledHue}, {RoundedScaledSaturation}%, {RoundedScaledValue}%)";
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Rgb ToRgb()
        {
            // Formula from https://www.rapidtables.com/convert/color/hsv-to-rgb.html

            var hue = ScaledHue;
            var sat = Saturation;
            var val = Value;

            // 360deg is actually 0deg (Red)
            if (hue >= 360)
            {
                hue = 0;
            }

            var C = val * sat;
            var X = C * (1 - Math.Abs(hue / 60 % 2 - 1));
            var m = val - C;

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

        public Hsl ToHsl()
        {
            // Formula from https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_HSL

            var hue = ScaledHue;
            var sat1 = Saturation;
            var val1 = Value;

            var val = val1 * (1 - (sat1 / 2));
            var sat = 0.0;
            if (val > 0 && val < 1)
            {
                sat = (val1 - val) / Math.Min(val, 1 - val);
            }

            return new Hsl(hue / 360.0, sat, val);
        }

        public System.Windows.Media.Color ToMediaColor()
        {
            return ToRgb().ToMediaColor();
        }

        public bool Equals(Hsv other)
        {
            return Hue.Equals(other.Hue) &&
                   Saturation.Equals(other.Saturation) &&
                   Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is Hsv other && Equals(other);
        }

        public static bool operator ==(Hsv item1, Hsv item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(Hsv item1, Hsv item2)
        {
            return !item1.Equals(item2);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return HashCode.Combine(Hue, Saturation, Value);
        }
    }
}