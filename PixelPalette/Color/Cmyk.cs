using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PixelPalette.Color
{
    public readonly struct Cmyk
    {
        public static readonly Cmyk Empty = new Cmyk();

        public static readonly double MinComponentValue = 0;
        public static readonly double MaxComponentValue = 100;

        /// <summary>
        /// Cyan on a scale of 0-100
        /// </summary>
        public double Cyan { get; }

        /// <summary>
        /// Magenta on a scale of 0-100
        /// </summary>
        public double Magenta { get; }

        /// <summary>
        /// Yellow on a scale of 0-100
        /// </summary>
        public double Yellow { get; }

        /// <summary>
        /// Key (Black) on a scale of 0-100
        /// </summary>
        public double Key { get; }

        public double RoundedCyan => Round(Cyan);
        public double RoundedMagenta => Round(Magenta);
        public double RoundedYellow => Round(Yellow);
        public double RoundedKey => Round(Key);

        /// <param name="c">0-100</param>
        /// <param name="m">0-100</param>
        /// <param name="y">0-100</param>
        /// <param name="k">0-100</param>
        public Cmyk(double c, double m, double y, double k)
        {
            Cyan = ClampedComponent(c);
            Magenta = ClampedComponent(m);
            Yellow = ClampedComponent(y);
            Key = ClampedComponent(k);
        }

        public static Cmyk? FromString(string theString)
        {
            var regex = new Regex(
                @"cmyk\(\s*(?<c>\d+)\s*,\s*(?<m>\d+)%?\s*,\s*(?<y>\d+)%?\s*,\s*(?<k>\d+)%?\s*\)",
                RegexOptions.IgnoreCase
            );
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

        public static double ClampedComponent(double v)
        {
            return v > MaxComponentValue ? MaxComponentValue : v < MinComponentValue ? MinComponentValue : v;
        }

        private static double Round(double v)
        {
            return Math.Round(v, 0, MidpointRounding.ToPositiveInfinity);
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

        public override string ToString()
        {
            return $"cmyk({RoundedCyan}%, {RoundedMagenta}%, {RoundedYellow}%, {RoundedKey}%)";
        }

        /// <summary>
        /// Convert CMYK to RGB. Be aware, there may be some loss of color accuracy.
        /// CMYK should be tailored to a color profile anyway.
        /// </summary>
        public Rgb ToRgb()
        {
            // Formula from https://www.rapidtables.com/convert/color/cmyk-to-rgb.html

            var c1 = Cyan / 100.0;
            var m1 = Magenta / 100.0;
            var y1 = Yellow / 100.0;
            var k1 = Key / 100.0;

            var r = 255 * (1 - c1) * (1 - k1);
            var g = 255 * (1 - m1) * (1 - k1);
            var b = 255 * (1 - y1) * (1 - k1);

            return new Rgb(
                (int) Math.Round(r, 0, MidpointRounding.ToPositiveInfinity),
                (int) Math.Round(g, 0, MidpointRounding.ToPositiveInfinity),
                (int) Math.Round(b, 0, MidpointRounding.ToPositiveInfinity)
            );
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