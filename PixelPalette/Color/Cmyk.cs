using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PixelPalette.Color
{
    public readonly struct Cmyk : IColor
    {
        public static readonly Cmyk Empty = new Cmyk();

        private const string StringPattern =
            @"^cmyk\(\s*(?<c>0|1(?:\.0)?|0?\.\d+)\s*,\s*(?<m>0|1(?:\.0)?|0?\.\d+)\s*,\s*(?<y>0|1(?:\.0)?|0?\.\d+)\s*,\s*(?<k>0|1(?:\.0)?|0?\.\d+)\s*\)$";

        private const string ScaledStringPattern =
            @"^cmyk\(\s*(?<c>\d+(?:\.\d+)?)%?\s*,\s*(?<m>\d+(?:\.\d+)?)%?\s*,\s*(?<y>\d+(?:\.\d+)?)%?\s*,\s*(?<k>\d+(?:\.\d+)?)%?\s*\)$";

        /// <summary>
        /// Cyan on a scale of 0-1
        /// </summary>
        public double Cyan { get; }

        /// <summary>
        /// Magenta on a scale of 0-1
        /// </summary>
        public double Magenta { get; }

        /// <summary>
        /// Yellow on a scale of 0-1
        /// </summary>
        public double Yellow { get; }

        /// <summary>
        /// Key (black) on a scale of 0-1
        /// </summary>
        public double Key { get; }

        /// <summary>
        /// Cyan on a scale of 0-100
        /// </summary>
        public double ScaledCyan => Cyan * 100.0;

        /// <summary>
        /// Magenta on a scale of 0-100
        /// </summary>
        public double ScaledMagenta => Magenta * 100.0;

        /// <summary>
        /// Yellow on a scale of 0-100
        /// </summary>
        public double ScaledYellow => Yellow * 100.0;

        /// <summary>
        /// Key (black) on a scale of 0-100
        /// </summary>
        public double ScaledKey => Key * 100.0;

        /// <summary>
        /// Cyan on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedCyan => Round(Cyan);

        /// <summary>
        /// Magenta on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedMagenta => Round(Magenta);

        /// <summary>
        /// Yellow on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedYellow => Round(Yellow);

        /// <summary>
        /// Key (black) on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedKey => Round(Key);

        /// <summary>
        /// Cyan on a scale of 0-100, rounded to 0 decimal places
        /// </summary>
        public double RoundedScaledCyan => Round(ScaledCyan, 0);

        /// <summary>
        /// Magenta on a scale of 0-100, rounded to 0 decimal places
        /// </summary>
        public double RoundedScaledMagenta => Round(ScaledMagenta, 0);

        /// <summary>
        /// Yellow on a scale of 0-100, rounded to 0 decimal places
        /// </summary>
        public double RoundedScaledYellow => Round(ScaledYellow, 0);

        /// <summary>
        /// Key (black) on a scale of 0-100, rounded to 0 decimal places
        /// </summary>
        public double RoundedScaledKey => Round(ScaledKey, 0);

        /// <param name="c">0-1</param>
        /// <param name="m">0-1</param>
        /// <param name="y">0-1</param>
        /// <param name="k">0-1</param>
        public Cmyk(double c, double m, double y, double k)
        {
            Cyan = ClampedComponent(c);
            Magenta = ClampedComponent(m);
            Yellow = ClampedComponent(y);
            Key = ClampedComponent(k);
        }

        /// <param name="c">0-100</param>
        /// <param name="m">0-100</param>
        /// <param name="y">0-100</param>
        /// <param name="k">0-100</param>
        public static Cmyk FromScaledValues(double c, double m, double y, double k)
        {
            return new Cmyk(c / 100.0, m / 100.0, y / 100.0, k / 100.0);
        }

        public static bool IsValidComponent(double value)
        {
            return value >= 0.0 && value <= 1.0;
        }

        public static bool IsValidScaledComponent(double value)
        {
            return value >= 0.0 && value <= 100.0;
        }

        public static Cmyk? FromString(string theString)
        {
            var regex = new Regex(StringPattern, RegexOptions.IgnoreCase);
            var match = regex.Match(theString);
            if (match.Success)
            {
                return new Cmyk(
                    double.Parse(match.Groups["c"].Value),
                    double.Parse(match.Groups["m"].Value),
                    double.Parse(match.Groups["y"].Value),
                    double.Parse(match.Groups["k"].Value)
                );
            }

            return null;
        }

        public static Cmyk? FromScaledString(string theString)
        {
            var regex = new Regex(ScaledStringPattern, RegexOptions.IgnoreCase);
            var match = regex.Match(theString);
            if (!match.Success) return null;
            var c = double.Parse(match.Groups["c"].Value);
            var m = double.Parse(match.Groups["m"].Value);
            var y = double.Parse(match.Groups["y"].Value);
            var k = double.Parse(match.Groups["k"].Value);
            return FromScaledValues(c, m, y, k);
        }

        public static double ClampedComponent(double v)
        {
            return v > 1.0 ? 1.0 : v < 0.0 ? 0.0 : v;
        }

        public static double ClampedScaledComponent(double v)
        {
            return v > 100.0 ? 100.0 : v < 0.0 ? 0.0 : v;
        }

        public Cmyk WithCyan(double c)
        {
            return new Cmyk(c, Magenta, Yellow, Key);
        }

        public Cmyk WithMagenta(double m)
        {
            return new Cmyk(Cyan, m, Yellow, Key);
        }

        public Cmyk WithYellow(double y)
        {
            return new Cmyk(Cyan, Magenta, y, Key);
        }

        public Cmyk WithKey(double k)
        {
            return new Cmyk(Cyan, Magenta, Yellow, k);
        }

        public Cmyk WithScaledCyan(double c)
        {
            return new Cmyk(c / 100.0, Magenta, Yellow, Key);
        }

        public Cmyk WithScaledMagenta(double m)
        {
            return new Cmyk(Cyan, m / 100.0, Yellow, Key);
        }

        public Cmyk WithScaledYellow(double y)
        {
            return new Cmyk(Cyan, Magenta, y / 100.0, Key);
        }

        public Cmyk WithScaledKey(double k)
        {
            return new Cmyk(Cyan, Magenta, Yellow, k / 100.0);
        }

        private static double Round(double v, int precision = 3)
        {
            return Math.Round(v, precision, MidpointRounding.AwayFromZero);
        }

        public override string ToString()
        {
            return $"cmyk({RoundedCyan}, {RoundedMagenta}, {RoundedYellow}, {RoundedKey})";
        }

        public string ToScaledString()
        {
            return $"cmyk({RoundedScaledCyan}%, {RoundedScaledMagenta}%, {RoundedScaledYellow}%, {RoundedScaledKey}%)";
        }

        /// <summary>
        /// Convert CMYK to RGB. Be aware, there may be some loss of color accuracy.
        /// CMYK should be tailored to a color profile anyway.
        /// </summary>
        public Rgb ToRgb()
        {
            // Formula from https://www.rapidtables.com/convert/color/cmyk-to-rgb.html

            var c1 = Cyan;
            var m1 = Magenta;
            var y1 = Yellow;
            var k1 = Key;

            var r = (1 - c1) * (1 - k1);
            var g = (1 - m1) * (1 - k1);
            var b = (1 - y1) * (1 - k1);

            return new Rgb(r, g, b);
        }

        public bool Equals(Cmyk other)
        {
            return Cyan.Equals(other.Cyan) &&
                   Magenta.Equals(other.Magenta) &&
                   Yellow.Equals(other.Yellow) &&
                   Key.Equals(other.Key);
        }

        public override bool Equals(object obj)
        {
            return obj is Cmyk other && Equals(other);
        }

        public static bool operator ==(Cmyk item1, Cmyk item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(Cmyk item1, Cmyk item2)
        {
            return !item1.Equals(item2);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return HashCode.Combine(Cyan, Magenta, Yellow, Key);
        }
    }
}
