using PixelPalette.Color;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace PixelPalette.Tests.Color
{
    [TestClass]
    public class RgbTests
    {
        [DataTestMethod]
        [DataRow("0", true)]
        [DataRow("1", true)]
        [DataRow("0.0", true)]
        [DataRow("0.9", true)]
        [DataRow("0.99999", true)]
        [DataRow("0.0", true)]
        [DataRow("1.0", true)]
        [DataRow("1.01", false)]
        [DataRow("1.1", false)]
        [DataRow("-0", false)]
        [DataRow("FF", false)]
        public void IsValidComponent_ShouldValidateProperly_WithStrings(string theString, bool expectedResult)
        {
            Rgb.IsValidComponent(theString).ShouldBe(expectedResult);
        }

        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(1, true)]
        [DataRow(0.5, true)]
        [DataRow(0.99999, true)]
        [DataRow(1.01, false)]
        public void IsValidComponent_ShouldValidateProperly_WithDoubles(double value, bool expectedResult)
        {
            Rgb.IsValidComponent(value).ShouldBe(expectedResult);
        }

        [DataTestMethod]
        [DataRow("0", true)]
        [DataRow("255", true)]
        [DataRow("0.0", false)]
        [DataRow("254", true)]
        [DataRow("-0", true)]
        [DataRow("-1", false)]
        [DataRow("256", false)]
        [DataRow("FF", false)]
        public void IsValidScaledComponent_ShouldValidateProperly_WithStrings(string theString, bool expectedResult)
        {
            Rgb.IsValidScaledComponent(theString).ShouldBe(expectedResult);
        }

        [DataTestMethod]
        [DataRow(0, true)]
        [DataRow(255, true)]
        [DataRow(-1, false)]
        [DataRow(256, false)]
        public void IsValidScaledComponent_ShouldValidateProperly_WithIntegers(int value, bool expectedResult)
        {
            Rgb.IsValidScaledComponent(value).ShouldBe(expectedResult);
        }

        [DataTestMethod]
        [DataRow("rgb(0, 0, 0)", 0, 0, 0)]
        [DataRow("rgb(1, 1, 1)", 1, 1, 1)]
        [DataRow("rgb(0.1, 0.2, 0.3)", 0.1, 0.2, 0.3)]
        [DataRow("rgb(1.5, 0, 0)", -1, -1, -1, true)]
        [DataRow("rgb(-1, 0, 0)", -1, -1, -1, true)]
        [DataRow("rgb(-1, 0, 0", -1, -1, -1, true)]
        public void FromString_ShouldReturnObject(
            string theString,
            double expectedRed,
            double expectedGreen,
            double expectedBlue,
            bool expectedNull = false
        )
        {
            var rgb = Rgb.FromString(theString);
            if (expectedNull)
            {
                rgb.ShouldBeNull();
            }
            else
            {
                rgb.ShouldBeOfType<Rgb>();
                rgb?.Red.ShouldBe(expectedRed);
                rgb?.Green.ShouldBe(expectedGreen);
                rgb?.Blue.ShouldBe(expectedBlue);
            }
        }

        [DataTestMethod]
        [DataRow("rgb(0, 0, 0)", 0, 0, 0)]
        [DataRow("rgb(255, 255, 255)", 255, 255, 255)]
        [DataRow("rgb(255, 255, 255)", 255, 255, 255)]
        [DataRow("rgb(-1, 0, 0)", -1, -1, -1, true)]
        [DataRow("rgb(300, 0, 0)", -1, -1, -1, true)]
        [DataRow("rgb(255, 255, x)", -1, -1, -1, true)]
        [DataRow("rgb(255, 255, 255", -1, -1, -1, true)]
        public void FromScaledString_ShouldReturnObject(
            string theString,
            int expectedRed,
            int expectedGreen,
            int expectedBlue,
            bool expectedNull = false
        )
        {
            var rgb = Rgb.FromScaledString(theString);
            if (expectedNull)
            {
                rgb.ShouldBeNull();
            }
            else
            {
                rgb.ShouldBeOfType<Rgb>();
                rgb?.ScaledRed.ShouldBe(expectedRed);
                rgb?.ScaledGreen.ShouldBe(expectedGreen);
                rgb?.ScaledBlue.ShouldBe(expectedBlue);
            }
        }

        [TestMethod, ColorTestData]
        public void ColorConversions_ShouldBeAccurate(ColorData color)
        {
            var hsl = color.Rgb.ToHsl();
            hsl.RoundedHue.ShouldBeEquivalentTo(color.Hsl.RoundedHue);
            hsl.RoundedSaturation.ShouldBeEquivalentTo(color.Hsl.RoundedSaturation);
            hsl.RoundedLuminance.ShouldBeEquivalentTo(color.Hsl.RoundedLuminance);

            var hsv = color.Rgb.ToHsv();
            hsv.RoundedHue.ShouldBeEquivalentTo(color.Hsv.RoundedHue);
            hsv.RoundedSaturation.ShouldBeEquivalentTo(color.Hsv.RoundedSaturation);
            hsv.RoundedValue.ShouldBeEquivalentTo(color.Hsv.RoundedValue);

            var cmyk = color.Rgb.ToCmyk();
            cmyk.RoundedCyan.ShouldBeEquivalentTo(color.Cmyk.RoundedCyan);
            cmyk.RoundedMagenta.ShouldBeEquivalentTo(color.Cmyk.RoundedMagenta);
            cmyk.RoundedYellow.ShouldBeEquivalentTo(color.Cmyk.RoundedYellow);
            cmyk.RoundedKey.ShouldBeEquivalentTo(color.Cmyk.RoundedKey);

            var xyz = color.Rgb.ToXyz();
            xyz.RoundedX.ShouldBeEquivalentTo(color.Xyz.RoundedX);
            xyz.RoundedY.ShouldBeEquivalentTo(color.Xyz.RoundedY);
            xyz.RoundedZ.ShouldBeEquivalentTo(color.Xyz.RoundedZ);
        }

        [TestMethod]
        public void Red_ShouldBeClamped()
        {
            var rgb = new Rgb(-1, 0, 0);
            rgb.ScaledRed.ShouldBe(0);
            rgb = new Rgb(256, 0, 0);
            rgb.ScaledRed.ShouldBe(255);

            rgb = new Rgb(-0.1, 0, 0);
            rgb.Red.ShouldBe(0.0);
            rgb = new Rgb(1.1, 0, 0);
            rgb.Red.ShouldBe(1.0);
        }

        [TestMethod]
        public void Green_ShouldBeClamped()
        {
            var rgb = new Rgb(0, -1, 0);
            rgb.ScaledGreen.ShouldBe(0);
            rgb = new Rgb(0, 256, 0);
            rgb.ScaledGreen.ShouldBe(255);

            rgb = new Rgb(0, -0.1, 0);
            rgb.Green.ShouldBe(0.0);
            rgb = new Rgb(0, 1.1, 0);
            rgb.Green.ShouldBe(1.0);
        }

        [TestMethod]
        public void Blue_ShouldBeClamped()
        {
            var rgb = new Rgb(0, 0, -1);
            rgb.ScaledBlue.ShouldBe(0);
            rgb = new Rgb(0, 0, 256);
            rgb.ScaledBlue.ShouldBe(255);
            
            rgb = new Rgb(0, 0, -0.1);
            rgb.Blue.ShouldBe(0.0);
            rgb = new Rgb(0, 0, 1.1);
            rgb.Blue.ShouldBe(1.0);
        }
    }
}