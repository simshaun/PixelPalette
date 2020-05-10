using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PixelPalette.Color
{
    public struct Hex
    {
        public static readonly Hex Empty = new Hex();

        private int _red;
        private int _green;
        private int _blue;

        public int Red
        {
            get => _red;
            set => _red = (value > 255) ? 255 : ((value < 0) ? 0 : value);
        }

        public int Green
        {
            get => _green;
            set => _green = (value > 255) ? 255 : ((value < 0) ? 0 : value);
        }

        public int Blue
        {
            get => _blue;
            set => _blue = (value > 255) ? 255 : ((value < 0) ? 0 : value);
        }

        public string RedPart => $"{Red:x2}".ToUpper();
        public string GreenPart => $"{Green:x2}".ToUpper();
        public string BluePart => $"{Blue:x2}".ToUpper();

        public Hex(int r, int g, int b)
        {
            _red = (r > 255) ? 255 : ((r < 0) ? 0 : r);
            _green = (g > 255) ? 255 : ((g < 0) ? 0 : g);
            _blue = (b > 255) ? 255 : ((b < 0) ? 0 : b);
        }

        public Hex WithRed(int r)
        {
            return new Hex(r, Green, Blue);
        }

        public Hex WithRed(string r)
        {
            return new Hex(ClampedComponent(r), Green, Blue);
        }

        public Hex WithGreen(int g)
        {
            return new Hex(Red, g, Blue);
        }

        public Hex WithGreen(string g)
        {
            return new Hex(Red, ClampedComponent(g), Blue);
        }

        public Hex WithBlue(int b)
        {
            return new Hex(Red, Green, b);
        }

        public Hex WithBlue(string b)
        {
            return new Hex(Red, Green, ClampedComponent(b));
        }

        public Hex(string hex)
        {
            if (!IsValidHex(hex))
            {
                throw new ArgumentException($"{hex} is not a valid hex");
            }

            // Expand shorthand form (e.g. "03F") to full form (e.g. "0033FF")
            var shorthandRegex = new Regex("^#?([a-f\\d])([a-f\\d])([a-f\\d])$", RegexOptions.IgnoreCase);
            var expandedHex = shorthandRegex.Replace(hex, match =>
            {
                var r = match.Groups[1].Value;
                var g = match.Groups[2].Value;
                var b = match.Groups[3].Value;
                return r + r + g + g + b + b;
            });

            var regex = new Regex("^#?([a-f\\d]{2})([a-f\\d]{2})([a-f\\d]{2})$", RegexOptions.IgnoreCase);
            var result = regex.Match(expandedHex);

            _red = int.Parse(result.Groups[1].Value, NumberStyles.HexNumber);
            _green = int.Parse(result.Groups[2].Value, NumberStyles.HexNumber);
            _blue = int.Parse(result.Groups[3].Value, NumberStyles.HexNumber);
        }

        public static Hex? From6CharString(string theString)
        {
            if (!IsValid6CharHex(theString)) return null;
            return new Hex(theString);
        }

        public static Hex? FromString(string theString)
        {
            try
            {
                return new Hex(theString);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public override string ToString()
        {
            return $"#{RedPart}{GreenPart}{BluePart}".ToUpper();
        }

        public Rgb ToRgb()
        {
            return Rgb.FromScaledValues(_red, _green, _blue);
        }

        public static bool IsValidHex(string hex)
        {
            var regex = new Regex("^#?([0-9A-F]{3}){1,2}$", RegexOptions.IgnoreCase);
            return regex.IsMatch(hex);
        }

        public static bool IsValid6CharHex(string hex)
        {
            var regex = new Regex("^#?([0-9A-F]{6})$", RegexOptions.IgnoreCase);
            return regex.IsMatch(hex);
        }

        public static bool IsValidHexPart(string hexPart)
        {
            var isInt = int.TryParse(hexPart, NumberStyles.HexNumber, null, out var partAsInt);

            return isInt && partAsInt >= 0 && partAsInt <= 255;
        }

        public static int ClampedComponent(int c)
        {
            return c > 255 ? 255 : c < 0 ? 0 : c;
        }

        public static int ClampedComponent(string c)
        {
            var isInt = int.TryParse(c, NumberStyles.HexNumber, null, out var value);
            return !isInt ? 0 : ClampedComponent(value);
        }

        public bool Equals(Hex other)
        {
            return _red == other._red && _green == other._green && _blue == other._blue;
        }

        public override bool Equals(object obj)
        {
            return obj is Hex other && Equals(other);
        }

        public static bool operator ==(Hex item1, Hex item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(Hex item1, Hex item2)
        {
            return !item1.Equals(item2);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return HashCode.Combine(_red, _green, _blue);
        }
    }
}