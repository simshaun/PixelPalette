using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PixelPalette.Color
{
    public readonly struct Hsl
    {
        public static readonly Hsl Empty = new Hsl();

        public static readonly double MinHue = 0;
        public static readonly double MaxHue = 360;
        public static readonly double MinSaturation = 0;
        public static readonly double MaxSaturation = 100;
        public static readonly double MinLuminance = 0;
        public static readonly double MaxLuminance = 100;

        /// <summary>
        /// Hue on a scale of 0-360
        /// </summary>
        public double Hue { get; }

        /// <summary>
        /// Saturation on a scale of 0-100
        /// </summary>
        public double Saturation { get; }

        /// <summary>
        /// Luminance on a scale of 0-100
        /// </summary>
        public double Luminance { get; }

        /// <summary>
        /// Rounded Hue
        /// </summary>
        public double RoundedHue => Round(Hue);

        /// <summary>
        /// Saturation on a scale of 0-100, rounded to 2 decimal places
        /// </summary>
        public double RoundedSaturation => Round(Saturation);

        /// <summary>
        /// Luminance on a scale of 0-100, rounded to 2 decimal places
        /// </summary>
        public double RoundedLuminance => Round(Luminance);

        public Hsl(double h, double s, double l)
        {
            Hue = ClampedHue(h);
            Saturation = ClampedSaturation(s);
            Luminance = ClampedLuminance(l);
        }

        public static Hsl? FromString(string theString)
        {
            var regex = new Regex(@"hsl\(\s*(?<hue>\d+)\s*,\s*(?<sat>\d+)%?\s*,\s*(?<lum>\d+)%?\s*\)", RegexOptions.IgnoreCase);
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

        public static double ClampedHue(double h)
        {
            return h > MaxHue ? MaxHue : h < MinHue ? MinHue : h;
        }

        public static double ClampedSaturation(double s)
        {
            return s > MaxSaturation ? MaxSaturation : s < MinSaturation ? MinSaturation : s;
        }

        public static double ClampedLuminance(double l)
        {
            return l > MaxLuminance ? MaxLuminance : l < MinLuminance ? MinLuminance : l;
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

        private static double Round(double d)
        {
            return Math.Round(d, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Lighten the color by X amount (0-100)
        /// </summary>
        public Hsl Lighter(int amount = 10)
        {
            return new Hsl(Hue, Saturation, Round(Luminance + amount));
        }

        /// <summary>
        /// Darken the color by X amount (0-100)
        /// </summary>
        public Hsl Darker(int amount = 10)
        {
            return new Hsl(Hue, Saturation, Round(Luminance - amount));
        }

        public override string ToString()
        {
            return $"hsl({RoundedHue}, {RoundedSaturation}%, {RoundedLuminance}%)";
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Rgb ToRgb()
        {
            // Formula from https://www.rapidtables.com/convert/color/hsl-to-rgb.html

            var hue = Hue;
            var sat = Saturation / 100;
            var lum = Luminance / 100;

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

            return new Rgb(
                Convert.ToInt32((r1 + m) * 255),
                Convert.ToInt32((g1 + m) * 255),
                Convert.ToInt32((b1 + m) * 255)
            );
        }

        public Hsv ToHsv()
        {
            // Formula from https://en.wikipedia.org/wiki/HSL_and_HSV#HSL_to_HSV

            var hue = Hue;
            var sat1 = Saturation / 100;
            var lum1 = Luminance / 100;

            var val = lum1 + sat1 * Math.Min(lum1, 1 - lum1);
            var sat = 0.0;
            if (val > 0)
            {
                sat = 2 * (1 - (lum1 / val));
            }

            return new Hsv(hue, sat * 100, val * 100);
        }

        public System.Windows.Media.Color ToMediaColor()
        {
            var rgb = ToRgb();
            return System.Windows.Media.Color.FromRgb((byte) rgb.Red, (byte) rgb.Green, (byte) rgb.Blue);
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