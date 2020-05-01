using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PixelPalette.Color
{
    /// <summary>
    /// HSV/HSB (same thing)
    /// </summary>
    public readonly struct Hsv
    {
        public static readonly Hsv Empty = new Hsv();

        public static readonly double MinHue = 0;
        public static readonly double MaxHue = 360;
        public static readonly double MinSaturation = 0;
        public static readonly double MaxSaturation = 100;
        public static readonly double MinValue = 0;
        public static readonly double MaxValue = 100;

        /// <summary>
        /// Hue on a scale of 0-360
        /// </summary>
        public double Hue { get; }

        /// <summary>
        /// Saturation on a scale of 0-100
        /// </summary>
        public double Saturation { get; }

        /// <summary>
        /// Value on a scale of 0-100
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Hue, rounded to 2 decimal places
        /// </summary>
        public double RoundedHue => Round(Hue);

        /// <summary>
        /// Saturation on a scale of 0-100, rounded to 2 decimal places
        /// </summary>
        public double RoundedSaturation => Round(Saturation);

        /// <summary>
        /// Value on a scale of 0-100, rounded to 2 decimal places
        /// </summary>
        public double RoundedValue => Round(Value);

        public Hsv(double h, double s, double v)
        {
            Hue = ClampedHue(h);
            Saturation = ClampedSaturation(s);
            Value = ClampedValue(v);
        }

        public static Hsv? FromString(string theString)
        {
            var regex = new Regex(@"hsv\(\s*(?<hue>\d+(?:\.\d+)?)\s*,\s*(?<sat>\d+(?:\.\d+)?)%?\s*,\s*(?<val>\d+(?:\.\d+)?)%?\s*\)", RegexOptions.IgnoreCase);
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

        public static double ClampedHue(double h)
        {
            return h > MaxHue ? MaxHue : h < MinHue ? MinHue : h;
        }

        public static double ClampedSaturation(double s)
        {
            return s > MaxSaturation ? MaxSaturation : s < MinSaturation ? MinSaturation : s;
        }

        public static double ClampedValue(double v)
        {
            return v > MaxValue ? MaxValue : v < MinValue ? MinValue : v;
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

        private static double Round(double d)
        {
            return Math.Round(d, 2, MidpointRounding.AwayFromZero);
        }

        public override string ToString()
        {
            return $"hsv({RoundedHue}, {RoundedSaturation}%, {RoundedValue}%)";
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Rgb ToRgb()
        {
            // Formula from https://www.rapidtables.com/convert/color/hsv-to-rgb.html

            var hue = Hue;
            var sat = Saturation / 100;
            var val = Value / 100;

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

            return new Rgb(
                Convert.ToInt32((r1 + m) * 255),
                Convert.ToInt32((g1 + m) * 255),
                Convert.ToInt32((b1 + m) * 255)
            );
        }

        public Hsl ToHsl()
        {
            // Formula from https://en.wikipedia.org/wiki/HSL_and_HSV#HSV_to_HSL

            var hue = Hue;
            var val1 = Value / 100;
            var sat1 = Saturation / 100;

            var val = val1 * (1 - (sat1 / 2));
            var sat = 0.0;
            if (val > 0 && val < 1)
            {
                sat = (val1 - val) / Math.Min(val, 1 - val);
            }

            return new Hsl(hue, sat * 100, val * 100);
        }

        public System.Windows.Media.Color ToMediaColor()
        {
            var rgb = ToRgb();
            return System.Windows.Media.Color.FromRgb((byte) rgb.Red, (byte) rgb.Green, (byte) rgb.Blue);
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