using System;
using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra.Double;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace PixelPalette.Color
{
    public readonly struct Rgb
    {
        public static readonly Rgb Empty = new Rgb();

        public static readonly int MinComponentValue = 0;
        public static readonly int MaxComponentValue = 255;

        // sRGB to CIE-XYZ matrix (D65) – http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
        // Long values pulled from Bruce's color calculator JS
        private static readonly DenseMatrix RgbMatrix = DenseMatrix.OfArray(new[,]
        {
            {0.4124564390896922, 0.357576077643909, 0.18043748326639894},
            {0.21267285140562253, 0.715152155287818, 0.07217499330655958},
            {0.0193338955823293, 0.11919202588130297, 0.9503040785363679}
        });

        public int Red { get; }

        public int Green { get; }

        public int Blue { get; }

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

        public Rgb(int r, int g, int b)
        {
            Red = ClampedComponent(r);
            Green = ClampedComponent(g);
            Blue = ClampedComponent(b);
        }

        public static Rgb? FromString(string theString)
        {
            var regex = new Regex(@"rgb\(\s*(?<red>\d+)\s*,\s*(?<green>\d+)\s*,\s*(?<blue>\d+)\s*\)", RegexOptions.IgnoreCase);
            var match = regex.Match(theString);
            if (match.Success)
            {
                return new Rgb(
                    int.Parse(match.Groups["red"].Value), 
                    int.Parse(match.Groups["green"].Value), 
                    int.Parse(match.Groups["blue"].Value)
                );
            }

            return null;

        }

        public static int ClampedComponent(int c)
        {
            return c > MaxComponentValue ? MaxComponentValue : c < MinComponentValue ? MinComponentValue : c;
        }

        public Rgb WithRed(int r)
        {
            return new Rgb(r, Green, Blue);
        }

        public Rgb WithGreen(int g)
        {
            return new Rgb(Red, g, Blue);
        }

        public Rgb WithBlue(int b)
        {
            return new Rgb(Red, Green, b);
        }

        public override string ToString()
        {
            return $"rgb({Red}, {Green}, {Blue})";
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
            return new Hex($"#{Red:x2}{Green:x2}{Blue:x2}".ToUpper());
        }

        public Hsl ToHsl()
        {
            // Formula from https://www.rapidtables.com/convert/color/rgb-to-hsl.html

            var r = Red / 255.0;
            var g = Green / 255.0;
            var b = Blue / 255.0;
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

            return new Hsl(h, s * 100, l * 100);
        }

        public Hsv ToHsv()
        {
            // Formula from https://www.rapidtables.com/convert/color/rgb-to-hsv.html

            var r = Red / 255.0;
            var g = Green / 255.0;
            var b = Blue / 255.0;
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

            return new Hsv(h, s * 100, v * 100);
        }

        /// <summary>
        /// Be aware, there may be some loss of color accuracy.
        /// CMYK should be tailored to a color profile anyway.
        /// </summary>
        public Cmyk ToCmyk()
        {
            // Formula from https://www.rapidtables.com/convert/color/rgb-to-cmyk.html

            var r1 = Red / 255.0;
            var g1 = Green / 255.0;
            var b1 = Blue / 255.0;

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

            var r1 = Red / 255.0;
            var g1 = Green / 255.0;
            var b1 = Blue / 255.0;

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
            return System.Windows.Media.Color.FromRgb((byte) Red, (byte) Green, (byte) Blue);
        }
    }
}