using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using PixelPalette.Color;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: TestDataSourceDiscovery(TestDataSourceDiscoveryOption.DuringExecution)]
namespace PixelPalette.Tests.Color
{
    public class ColorData
    {
        public string Name { get; init; }
        public Rgb Rgb { get; init; }
        public Hsl Hsl { get; init; }
        public Hsv Hsv { get; init; }
        public Cmyk Cmyk { get; init; }
        public Xyz Xyz { get; init; }
        public Lab Lab { get; init; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ColorTestDataAttribute : Attribute, ITestDataSource
    {
        // Conversion tool for reference:
        // https://www.easyrgb.com/en/convert.php#inputFORM

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            yield return new object[]
            {
                new ColorData
                {
                    Name = "Black",
                    Rgb = new Rgb(0, 0, 0),
                    Hsl = new Hsl(0, 0, 0),
                    Hsv = new Hsv(0, 0, 0),
                    Cmyk = new Cmyk(0, 0, 0, 1),
                    Xyz = new Xyz(0, 0, 0),
                    Lab = new Lab(0, 0, 0)
                }
            };
            yield return new object[]
            {
                new ColorData
                {
                    Name = "White",
                    Rgb = new Rgb(1, 1, 1),
                    Hsl = new Hsl(0, 0, 1),
                    Hsv = new Hsv(0, 0, 1),
                    Cmyk = new Cmyk(0, 0, 0, 0),
                    Xyz = new Xyz(95.047, 100, 108.883),
                    Lab = new Lab(100, 0, 0)
                }
            };
            yield return new object[]
            {
                new ColorData
                {
                    Name = "Red",
                    Rgb = new Rgb(1, 0, 0),
                    Hsl = new Hsl(0, 1, 0.5),
                    Hsv = new Hsv(0, 1, 1),
                    Cmyk = new Cmyk(0, 1, 1, 0),
                    Xyz = new Xyz(41.2456, 21.2673, 1.9334),
                    Lab = new Lab(53.2408, 80.0925, 67.2032)
                }
            };
            yield return new object[]
            {
                new ColorData
                {
                    Name = "Green",
                    Rgb = new Rgb(0, 1, 0),
                    Hsl = Hsl.FromScaledValues(120, 100, 50),
                    Hsv = Hsv.FromScaledValues(120, 100, 100),
                    Cmyk = new Cmyk(1, 0, 1, 0),
                    Xyz = new Xyz(35.7576, 71.5152, 11.9192),
                    Lab = new Lab(87.7347, -86.1827, 83.1793)
                }
            };
            yield return new object[]
            {
                new ColorData
                {
                    Name = "Blue",
                    Rgb = new Rgb(0, 0, 1),
                    Hsl = Hsl.FromScaledValues(240, 100, 50),
                    Hsv = Hsv.FromScaledValues(240, 100, 100),
                    Cmyk = new Cmyk(1, 1, 0, 0),
                    Xyz = new Xyz(18.0437, 7.2175, 95.0304),
                    Lab = new Lab(32.297, 79.1875, -107.8602)
                }
            };
            yield return new object[]
            {
                new ColorData
                {
                    Name = "Material Blue",
                    Rgb = Rgb.FromScaledValues(33, 150, 243),
                    Hsl = Hsl.FromScaledValues(206.5716, 89.744, 54.118),
                    Hsv = Hsv.FromScaledValues(206.5716, 86.42, 95.294),
                    Cmyk = Cmyk.FromScaledValues(86.419753, 38.271604, 0, 4.705882),
                    Xyz = new Xyz(27.704960365063513, 28.60350077011483, 88.83745176406208),
                    Lab = new Lab(60.4301, 2.0799, -55.1094)
                }
            };
            yield return new object[]
            {
                new ColorData
                {
                    Name = "Purple HEX to HSL/HSV issue",
                    Rgb = Rgb.FromScaledValues(116, 58, 111),
                    Hsl = Hsl.FromScaledValues(305.172, 33.333, 34.118),
                    Hsv = Hsv.FromScaledValues(305.172, 50, 45.49),
                    Cmyk = Cmyk.FromScaledValues(0, 50, 4.310, 54.510),
                    Xyz = new Xyz(11.5846, 7.8875, 15.9481),
                    Lab = new Lab(33.7475, 33.4761, -19.6542)
                }
            };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data == null) return null;
            var colorData = (ColorData) data[0];
            return string.Format(CultureInfo.CurrentCulture, "{0} {1}", methodInfo.Name, colorData.Name);
        }
    }
}