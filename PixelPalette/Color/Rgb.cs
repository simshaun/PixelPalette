using System;
using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra.Double;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace PixelPalette.Color
{
    public readonly struct Rgb
    {
        public static readonly Rgb Empty = new Rgb();

        // sRGB to CIE-XYZ matrix (D65) – http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        // Long values pulled from Bruce's color calculator JS
        private static readonly DenseMatrix RgbMatrix = DenseMatrix.OfArray(new[,]
        {
            {0.4124564390896922, 0.357576077643909, 0.18043748326639894},
            {0.21267285140562253, 0.715152155287818, 0.07217499330655958},
            {0.0193338955823293, 0.11919202588130297, 0.9503040785363679}
        });

        private const string StringPattern =
            @"^rgb\(\s*(?<red>0|1|0?\.\d+)\s*,\s*(?<green>0|1|0\.\d+)\s*,\s*(?<blue>0|1|0\.\d+)\s*\)$";

        private const string ScaledStringPattern =
            @"^rgb\(\s*(?<red>\d{1,3})\s*,\s*(?<green>\d{1,3})\s*,\s*(?<blue>\d{1,3})\s*\)$";

        /// <summary>
        /// Red on a scale of 0-1
        /// </summary>
        public double Red { get; }

        /// <summary>
        /// Green on a scale of 0-1
        /// </summary>
        public double Green { get; }

        /// <summary>
        /// Blue on a scale of 0-1
        /// </summary>
        public double Blue { get; }

        /// <summary>
        /// Red on a scale of 0-255
        /// </summary>
        public int ScaledRed => (int) Round(Red * 255, 0);

        /// <summary>
        /// Green on a scale of 0-255
        /// </summary>
        public int ScaledGreen => (int) Round(Green * 255, 0);

        /// <summary>
        /// Blue on a scale of 0-255
        /// </summary>
        public int ScaledBlue => (int) Round(Blue * 255, 0);

        /// <summary>
        /// Red on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedRed => Round(Red);

        /// <summary>
        /// Green on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedGreen => Round(Green);

        /// <summary>
        /// Blue on a scale of 0-1, rounded to 3 decimal places
        /// </summary>
        public double RoundedBlue => Round(Blue);

        /// <summary>
        /// Calculated perceived brightness. Value is 0 (black) – 255 (white)
        /// </summary>
        public int Brightness =>
            (int) Math.Sqrt(
                Red * Red * .241 +
                Green * Green * .691 +
                Blue * Blue * .068
            );

        /// <summary>
        /// The text color that is most likely to be visible against this color.
        /// </summary>
        public Rgb ContrastingTextColor()
        {
            return Brightness < 130 ? new Rgb(255, 255, 255) : new Rgb(0, 0, 0);
        }

        public Rgb(double r, double g, double b)
        {
            Red = ClampedComponent(r);
            Green = ClampedComponent(g);
            Blue = ClampedComponent(b);
        }

        public Rgb(int r, int g, int b)
        {
            Red = ClampedComponent(r) / 255.0;
            Green = ClampedComponent(g) / 255.0;
            Blue = ClampedComponent(b) / 255.0;
        }

        public static bool IsValidString(string theString)
        {
            return Regex.IsMatch(theString, StringPattern);
        }

        public static bool IsValidScaledString(string theString)
        {
            return Regex.IsMatch(theString, ScaledStringPattern);
        }

        public static bool IsValidComponent(string theString)
        {
            return Regex.IsMatch(theString, @"^0$|^1(\.0)?$|^0?\.\d+$");
        }

        public static bool IsValidComponent(double value)
        {
            return value >= 0.0 && value <= 1.0;
        }

        public static bool IsValidScaledComponent(string theString)
        {
            var isInt = int.TryParse(theString, out var value);
            if (!isInt) return false;
            return value >= 0 && value <= 255;
        }

        public static bool IsValidScaledComponent(int value)
        {
            return value >= 0 && value <= 255;
        }

        public static Rgb? FromString(string theString)
        {
            var regex = new Regex(StringPattern, RegexOptions.IgnoreCase);
            var match = regex.Match(theString);
            if (match.Success)
            {
                return new Rgb(
                    double.Parse(match.Groups["red"].Value),
                    double.Parse(match.Groups["green"].Value),
                    double.Parse(match.Groups["blue"].Value)
                );
            }

            return null;
        }

        public static Rgb? FromScaledString(string theString)
        {
            var regex = new Regex(ScaledStringPattern, RegexOptions.IgnoreCase);
            var match = regex.Match(theString);
            if (!match.Success) return null;
            var r = int.Parse(match.Groups["red"].Value);
            var g = int.Parse(match.Groups["green"].Value);
            var b = int.Parse(match.Groups["blue"].Value);
            if (!IsValidScaledComponent(r) || !IsValidScaledComponent(g) || !IsValidScaledComponent(b)) return null;
            return new Rgb(r, g, b);
        }

        public static double ClampedComponent(double c)
        {
            return c > 1.0 ? 1.0 : c < 0.0 ? 0.0 : c;
        }

        public static int ClampedComponent(int c)
        {
            return c > 255 ? 255 : c < 0 ? 0 : c;
        }

        public Rgb WithRed(double r)
        {
            return new Rgb(r, Green, Blue);
        }

        public Rgb WithGreen(double g)
        {
            return new Rgb(Red, g, Blue);
        }

        public Rgb WithBlue(double b)
        {
            return new Rgb(Red, Green, b);
        }

        public Rgb WithRed(int r)
        {
            return new Rgb(r / 255.0, Green, Blue);
        }

        public Rgb WithGreen(int g)
        {
            return new Rgb(Red, g / 255.0, Blue);
        }

        public Rgb WithBlue(int b)
        {
            return new Rgb(Red, Green, b / 255.0);
        }

        private static double Round(double d, int precision = 3)
        {
            return Math.Round(d, precision, MidpointRounding.AwayFromZero);
        }

        public override string ToString()
        {
            return $"rgb({RoundedRed}, {RoundedGreen}, {RoundedBlue})";
        }

        public string ToScaledString()
        {
            return $"rgb({ScaledRed}, {ScaledGreen}, {ScaledBlue})";
        }

        public bool Equals(Rgb other)
        {
            return Red == other.Red && Green == other.Green && Blue == other.Blue;
        }

        public override bool Equals(object obj)
        {
            return obj is Rgb other && Equals(other);
        }

        public static bool operator ==(Rgb item1, Rgb item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(Rgb item1, Rgb item2)
        {
            return !item1.Equals(item2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Red, Green, Blue);
        }

        public Hex ToHex()
        {
            return new Hex($"#{ScaledRed:x2}{ScaledGreen:x2}{ScaledBlue:x2}".ToUpper());
        }

        public Hsl ToHsl()
        {
            // Formula from https://www.rapidtables.com/convert/color/rgb-to-hsl.html

            var r = Red;
            var g = Green;
            var b = Blue;
            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));
            var delta = max - min;

            // Hue
            var h = 0.0;
            if (max == r)
            {
                h = 60 * ((g - b) / delta % 6);
            }
            else if (max == g)
            {
                h = 60 * ((b - r) / delta + 2);
            }
            else if (max == b)
            {
                h = 60 * ((r - g) / delta + 4);
            }

            // Fix potential div-by-zero
            if (double.IsNaN(h))
            {
                h = 0.0;
            }

            // Lightness
            var l = (max + min) / 2;

            // Saturation
            var s = 0.0;
            if (delta != 0)
            {
                s = delta / (1 - Math.Abs(2 * l - 1));
            }

            return new Hsl(h / 360.0, s, l);
        }

        public Hsv ToHsv()
        {
            // Formula from https://www.rapidtables.com/convert/color/rgb-to-hsv.html

            var r = Red;
            var g = Green;
            var b = Blue;
            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));
            var delta = max - min;

            // Hue
            var h = 0.0;
            if (max == r)
            {
                h = 60 * ((g - b) / delta % 6);
            }
            else if (max == g)
            {
                h = 60 * ((b - r) / delta + 2);
            }
            else if (max == b)
            {
                h = 60 * ((r - g) / delta + 4);
            }

            // Fix potential div-by-zero
            if (double.IsNaN(h))
            {
                h = 0.0;
            }

            // Saturation
            var s = 0.0;
            if (max != 0)
            {
                s = delta / max;
            }

            // Value
            var v = max;

            return new Hsv(h / 360.0, s, v);
        }

        /// <summary>
        /// Be aware, there may be some loss of color accuracy.
        /// CMYK should be tailored to a color profile anyway.
        /// </summary>
        public Cmyk ToCmyk()
        {
            // Formula from https://www.rapidtables.com/convert/color/rgb-to-cmyk.html

            var r1 = Red;
            var g1 = Green;
            var b1 = Blue;

            var key = 1 - Math.Max(r1, Math.Max(g1, b1));

            double cyan;
            double magenta;
            double yellow;

            if (1 - key == 0)
            {
                cyan = 0;
                magenta = 0;
                yellow = 0;
            }
            else
            {
                cyan = (1 - r1 - key) / (1 - key);
                magenta = (1 - g1 - key) / (1 - key);
                yellow = (1 - b1 - key) / (1 - key);
            }

            return new Cmyk(cyan * 100, magenta * 100, yellow * 100, key * 100);
        }

        /// <summary>
        /// XYZ for a D65/2° standard illuminant.
        /// </summary>
        public Xyz ToXyz()
        {
            // Formula from http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html

            var r1 = Red;
            var g1 = Green;
            var b1 = Blue;

            static double InverseCompand(double companded)
            {
                var sign = 1.0;
                if (companded < 0.0)
                {
                    sign = -1.0;
                    companded = -companded;
                }

                var linear = (companded < 0.04045) ? (companded / 12.92) : Math.Pow((companded + 0.055) / 1.055, 2.4);
                return linear * sign;
            }

            var linearR = InverseCompand(r1);
            var linearG = InverseCompand(g1);
            var linearB = InverseCompand(b1);

            var rgbMatrix = new[,] {{linearR}, {linearG}, {linearB}};
            var xyz = RgbMatrix * DenseMatrix.OfArray(rgbMatrix);

            return new Xyz(xyz.Values[0] * 100, xyz.Values[1] * 100, xyz.Values[2] * 100);
        }

        public Lab ToLab()
        {
            return ToXyz().ToLab();
        }

        public System.Windows.Media.Color ToMediaColor()
        {
            return System.Windows.Media.Color.FromRgb((byte) ScaledRed, (byte) ScaledGreen, (byte) ScaledBlue);
        }
    }
}