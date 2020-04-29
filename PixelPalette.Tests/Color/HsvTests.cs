using PixelPalette.Color;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace PixelPalette.Tests.Color
{
    [TestClass]
    public class HsvTests
    {
        [DataTestMethod]
        [DataRow("hsv(200,80%,10%)", 200, 80, 10)]
        [DataRow("hsv(200, 90%, 30%)", 200, 90, 30)]
        [DataRow("hsv(200, 75 , 45)", 200, 75, 45)]
        [DataRow("hsv(255, 100 , 30 )", 255, 100, 30)]
        [DataRow("hsv(255a, 100, 30)", 0, 0, 0, true)]
        [DataRow("hsv(,,)", 0, 0, 0, true)]
        public void FromString_ShouldReturnObject(
            string theString,
            int expectedHue,
            int expectedSaturation,
            int expectedValue,
            bool expectedNull = false
        )
        {
            var hsv = Hsv.FromString(theString);
            if (expectedNull)
            {
                hsv.ShouldBeNull();
            }
            else
            {
                hsv.ShouldBeOfType<Hsv>();
                hsv?.Hue.ShouldBe(expectedHue);
                hsv?.Saturation.ShouldBe(expectedSaturation);
                hsv?.Value.ShouldBe(expectedValue);
            }
        }

        [TestMethod, ColorTestData]
        public void ColorConversions_ShouldBeAccurate(ColorData color)
        {
            color.Hsv.ToRgb().ShouldBeEquivalentTo(color.Rgb);

            var hsl = color.Hsv.ToHsl();
            hsl.RoundedHue.ShouldBeEquivalentTo(color.Hsl.RoundedHue);
            hsl.RoundedSaturation.ShouldBeEquivalentTo(color.Hsl.RoundedSaturation);
            hsl.RoundedLuminance.ShouldBeEquivalentTo(color.Hsl.RoundedLuminance);
        }

        [TestMethod]
        public void ToRgb_ShouldBeCorrect_AtMaxHue()
        {
            var hsv = new Hsv(360, 50, 50);
            hsv.ToRgb().ShouldBeEquivalentTo(new Rgb(128, 64, 64));
        }

        [TestMethod]
        public void ToRgb_ShouldBeCorrect_AtDecimals()
        {
            var hsv = new Hsv(60.2, 53, 70.91);
            hsv.ToRgb().ShouldBeEquivalentTo(new Rgb(181, 181, 85));
        }

        [TestMethod]
        public void ToHsl_ShouldBeCorrect_AtMaxHue()
        {
            var hsv = new Hsv(360, 50, 50);
            var hsl = hsv.ToHsl();
            hsl.Hue.ShouldBe(360);
            hsl.RoundedSaturation.ShouldBe(33.33);
            hsl.RoundedLuminance.ShouldBe(37.5);
        }

        [TestMethod]
        public void ToHsl_ShouldBeCorrect_AtDecimals()
        {
            var hsv = new Hsv(60.2, 53, 70.91);
            var hsl = hsv.ToHsl();
            hsl.Hue.ShouldBe(60.2);
            hsl.RoundedSaturation.ShouldBe(39.25);
            hsl.RoundedLuminance.ShouldBe(52.12);
        }

        [TestMethod]
        public void Hue_ShouldBeClamped()
        {
            var hsv = new Hsv(-0.01, 100, 100);
            hsv.Hue.ShouldBe(0);

            hsv = new Hsv(360.01, 100, 100);
            hsv.Hue.ShouldBe(360);
        }

        [TestMethod]
        public void Saturation_ShouldBeClamped()
        {
            var hsv = new Hsv(120, -1, 100);
            hsv.Saturation.ShouldBe(0);

            hsv = new Hsv(120, 101, 100);
            hsv.Saturation.ShouldBe(100);
        }

        [TestMethod]
        public void Value_ShouldBeClamped()
        {
            var hsv = new Hsv(120, 0, -1);
            hsv.Value.ShouldBe(0);

            hsv = new Hsv(120, 0, 101);
            hsv.Value.ShouldBe(100);
        }

        [TestMethod]
        public void RoundedHue_ShouldRound()
        {
            // Half-up
            var hsv = new Hsv(240.585, 100, 50);
            hsv.RoundedHue.ShouldBe(240.59);

            // Down
            hsv = new Hsv(240.584, 100, 50);
            hsv.RoundedHue.ShouldBe(240.58);
        }

        [TestMethod]
        public void RoundedSaturation100_ShouldRound()
        {
            // Half-up
            var hsv = new Hsv(240, 59.645, 100);
            hsv.RoundedSaturation.ShouldBe(59.65);

            // Down
            hsv = new Hsv(240, 59.644, 100);
            hsv.RoundedSaturation.ShouldBe(59.64);
        }

        [TestMethod]
        public void RoundedValue100_ShouldRound()
        {
            // Half-up
            var hsv = new Hsv(240, 100, 59.645);
            hsv.RoundedValue.ShouldBe(59.65);

            // Down
            hsv = new Hsv(240, 100, 59.644);
            hsv.RoundedValue.ShouldBe(59.64);
        }
    }
}