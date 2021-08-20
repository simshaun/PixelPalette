using System;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace PixelPalette.Color
{
    public readonly struct Xyz : IColor
    {
        public static readonly Xyz Empty = new();

        public const double MinX = 0;
        public const double MaxX = 95.0489;
        public const double MinY = 0;
        public const double MaxY = 100;
        public const double MinZ = 0;
        public const double MaxZ = 108.884;

        public double X { get; }

        public double Y { get; }

        public double Z { get; }

        /// <summary>
        /// X rounded to 2 decimal places
        /// </summary>
        public double RoundedX => Round(X);

        /// <summary>
        /// Y rounded to 2 decimal places
        /// </summary>
        public double RoundedY => Round(Y);

        /// <summary>
        /// Z rounded to 2 decimal places
        /// </summary>
        public double RoundedZ => Round(Z);

        public Xyz(double x, double y, double z)
        {
            X = ClampedX(x);
            Y = ClampedY(y);
            Z = ClampedZ(z);
        }

        /// <summary>
        /// Clamp X to reference white point of D65 illuminant: (0.950428, 1, 1.0889)
        /// </summary>
        public static double ClampedX(double x)
        {
            return x > MaxX ? MaxX : x < MinX ? MinX : x;
        }

        /// <summary>
        /// Clamp Y to reference white point of D65 illuminant: (0.950428, 1, 1.0889)
        /// </summary>
        public static double ClampedY(double y)
        {
            return y > MaxY ? MaxY : y < MinY ? MinY : y;
        }

        /// <summary>
        /// Clamp Z to reference white point of D65 illuminant: (0.950428, 1, 1.0889)
        /// </summary>
        public static double ClampedZ(double z)
        {
            return z > MaxZ ? MaxZ : z < MinZ ? MinZ : z;
        }

        public Xyz WithX(double x)
        {
            return new Xyz(x, Y, Z);
        }

        public Xyz WithY(double y)
        {
            return new Xyz(X, y, Z);
        }

        public Xyz WithZ(double z)
        {
            return new Xyz(X, Y, z);
        }

        private static double Round(double v)
        {
            return Math.Round(v, 2, MidpointRounding.ToPositiveInfinity);
        }

        public override string ToString()
        {
            return $"xyz({RoundedX}, {RoundedY}, {RoundedZ})";
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Rgb ToRgb()
        {
            // Using formula from https://www.easyrgb.com/en/math.php

            var x1 = X / 100.0;
            var y1 = Y / 100.0;
            var z1 = Z / 100.0;

            var r1 = x1 * 3.2406 + y1 * -1.5372 + z1 * -0.4986;
            var g1 = x1 * -0.9689 + y1 * 1.8758 + z1 * 0.0415;
            var b1 = x1 * 0.0557 + y1 * -0.2040 + z1 * 1.0570;

            if (r1 > 0.0031308)
            {
                r1 = 1.055 * Math.Pow(r1, 1.0 / 2.4) - 0.055;
            }
            else
            {
                r1 = 12.92 * r1;
            }

            if (g1 > 0.0031308)
            {
                g1 = 1.055 * Math.Pow(g1, 1.0 / 2.4) - 0.055;
            }
            else
            {
                g1 = 12.92 * g1;
            }

            if (b1 > 0.0031308)
            {
                b1 = 1.055 * Math.Pow(b1, 1.0 / 2.4) - 0.055;
            }
            else
            {
                b1 = 12.92 * b1;
            }

            return new Rgb(Rgb.ClampedComponent(r1), Rgb.ClampedComponent(g1), Rgb.ClampedComponent(b1));
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Lab ToLab()
        {
            // Formula from http://www.brucelindbloom.com/index.html?ColorCalculator.html

            const double refWhiteX = 95.047;
            const double refWhiteY = 100.0;
            const double refWhiteZ = 108.883;
            const double kE = 216.0 / 24389.0;
            const double kK = 24389.0 / 27.0;

            var xr = X / 100 / (refWhiteX / 100);
            var yr = Y / 100 / (refWhiteY / 100);
            var zr = Z / 100 / (refWhiteZ / 100);

            var fx = xr > kE ? Math.Pow(xr, 1.0 / 3.0) : (kK * xr + 16.0) / 116.0;
            var fy = yr > kE ? Math.Pow(yr, 1.0 / 3.0) : (kK * yr + 16.0) / 116.0;
            var fz = zr > kE ? Math.Pow(zr, 1.0 / 3.0) : (kK * zr + 16.0) / 116.0;

            var L = 116.0 * fy - 16.0;
            var A = 500.0 * (fx - fy);
            var B = 200.0 * (fy - fz);

            return new Lab(L, A, B);
        }

        public bool Equals(Xyz other)
        {
            return X.Equals(other.X) &&
                   Y.Equals(other.Y) &&
                   Z.Equals(other.Z);
        }

        public override bool Equals(object obj)
        {
            return obj is Xyz other && Equals(other);
        }

        public static bool operator ==(Xyz item1, Xyz item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(Xyz item1, Xyz item2)
        {
            return !item1.Equals(item2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }
}
