using PixelPalette.Color;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace PixelPalette.Tests.Color
{
    [TestClass]
    public class RgbTests
    {
        [DataTestMethod]
        [DataRow("rgb(255,100,30)", 255, 100, 30)]
        [DataRow("rgb(255, 100, 30)", 255, 100, 30)]
        [DataRow("rgb(255 , 100, 30)", 255, 100, 30)]
        [DataRow("rgb(255 , 100 , 30)", 255, 100, 30)]
        [DataRow("rgb( 255 , 100 , 30 )", 255, 100, 30)]
        [DataRow("rgb(255, 100 , 30 )", 255, 100, 30)]
        [DataRow("rgb(-1, 100 , 30 )", 0, 0, 0, true)]
        [DataRow("rgb(-1, )", 0, 0, 0, true)]
        [DataRow("rgb(255, 255, x)", 0, 0, 0, true)]
        public void FromString_ShouldReturnObject(
            string theString,
            int expectedRed,
            int expectedGreen,
            int expectedBlue,
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
            rgb.Red.ShouldBe(Rgb.MinComponentValue);

            rgb = new Rgb(256, 0, 0);
            rgb.Red.ShouldBe(Rgb.MaxComponentValue);
        }

        [TestMethod]
        public void Green_ShouldBeClamped()
        {
            var rgb = new Rgb(0, -1, 0);
            rgb.Green.ShouldBe(Rgb.MinComponentValue);

            rgb = new Rgb(0, 256, 0);
            rgb.Green.ShouldBe(Rgb.MaxComponentValue);
        }

        [TestMethod]
        public void Blue_ShouldBeClamped()
        {
            var rgb = new Rgb(0, 0, -1);
            rgb.Blue.ShouldBe(Rgb.MinComponentValue);

            rgb = new Rgb(0, 0, 256);
            rgb.Blue.ShouldBe(Rgb.MaxComponentValue);
        }
    }
}