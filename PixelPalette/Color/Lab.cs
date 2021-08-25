using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace PixelPalette.Color
{
    public readonly struct Lab
    {
        public static readonly Lab Empty = new();

        private const string StringPattern =
            @"^lab\(\s*(?<L>\d+(?:\.\d+)?)\s*,\s*(?<A>-?\d+(?:\.\d+)?)%?\s*,\s*(?<B>-?\d+(?:\.\d+)?)%?\s*\)$";

        public const double MinL = 0;
        public const double MaxL = 100;
        public const double MinA = -128;
        public const double MaxA = 127;
        public const double MinB = -128;
        public const double MaxB = 127;

        public double L { get; }

        public double A { get; }

        public double B { get; }

        /// <summary>
        /// L* rounded to 2 decimal places
        /// </summary>
        public double RoundedL => Round(L);

        /// <summary>
        /// a* rounded to 2 decimal places
        /// </summary>
        public double RoundedA => Round(A);

        /// <summary>
        /// b* rounded to 2 decimal places
        /// </summary>
        public double RoundedB => Round(B);

        public Lab(double l, double a, double b)
        {
            L = ClampedL(l);
            A = ClampedA(a);
            B = ClampedB(b);
        }

        public static bool IsValidL(double value)
        {
            return value is >= MinL and <= MaxL;
        }

        public static bool IsValidA(double value)
        {
            return value is >= MinA and <= MaxA;
        }

        public static bool IsValidB(double value)
        {
            return value is >= MinB and <= MaxB;
        }

        public static Lab? FromString(string theString)
        {
            var regex = new Regex(StringPattern, RegexOptions.IgnoreCase);
            var match = regex.Match(theString);
            if (!match.Success) return null;
            var l = double.Parse(match.Groups["L"].Value);
            var a = double.Parse(match.Groups["A"].Value);
            var b = double.Parse(match.Groups["B"].Value);
            if (!IsValidL(l) || !IsValidA(a) || !IsValidB(b)) return null;
            return new Lab(l, a, b);
        }

        /// <summary>
        /// Clamp L* to [0, 100]
        /// </summary>
        public static double ClampedL(double l)
        {
            return l > MaxL ? MaxL : l < MinL ? MinL : l;
        }

        /// <summary>
        /// Clamp a* to [-128, 127]
        /// </summary>
        public static double ClampedA(double a)
        {
            return a > MaxA ? MaxA : a < MinA ? MinA : a;
        }

        /// <summary>
        /// Clamp B* to [-128, 127]
        /// </summary>
        public static double ClampedB(double b)
        {
            return b > MaxB ? MaxB : b < MinB ? MinB : b;
        }

        public Lab WithL(double l)
        {
            return new Lab(l, A, B);
        }

        public Lab WithA(double a)
        {
            return new Lab(L, a, B);
        }

        public Lab WithB(double b)
        {
            return new Lab(L, A, b);
        }

        private static double Round(double v)
        {
            return Math.Round(v, 2, MidpointRounding.AwayFromZero);
        }

        public override string ToString()
        {
            return $"lab({RoundedL}, {RoundedA}, {RoundedB})";
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Xyz ToXyz()
        {
            // Formula from http://www.brucelindbloom.com/index.html?ColorCalculator.html

            const double refWhiteX = 95.047;
            const double refWhiteY = 100.0;
            const double refWhiteZ = 108.883;
            const double kE = 216.0 / 24389.0;
            const double kK = 24389.0 / 27.0;
            const double kKE = 8.0;

            var fy = (L + 16.0) / 116.0;
            var fx = 0.002 * A + fy;
            var fz = fy - 0.005 * B;

            var fx3 = fx * fx * fx;
            var fz3 = fz * fz * fz;

            var xr = fx3 > kE ? fx3 : (116.0 * fx - 16.0) / kK;
            var yr = L > kKE ? Math.Pow((L + 16.0) / 116.0, 3) : L / kK;
            var zr = fz3 > kE ? fz3 : (116.0 * fz - 16.0) / kK;

            var X = xr * refWhiteX;
            var Y = yr * refWhiteY;
            var Z = zr * refWhiteZ;

            return new Xyz(X, Y, Z);
        }

        public Rgb ToRgb()
        {
            return ToXyz().ToRgb();
        }

        public bool Equals(Lab other)
        {
            return L.Equals(other.L) &&
                   A.Equals(other.A) &&
                   B.Equals(other.B);
        }

        public override bool Equals(object? obj)
        {
            return obj is Lab other && Equals(other);
        }

        public static bool operator ==(Lab item1, Lab item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(Lab item1, Lab item2)
        {
            return !item1.Equals(item2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(L, A, B);
        }
    }
}