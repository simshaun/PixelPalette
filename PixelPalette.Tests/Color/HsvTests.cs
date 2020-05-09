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
        [DataRow("hsv(100.5, 90.4, 80.3)", 100.5, 90.4, 80.3)]
        [DataRow("hsv(255a, 100, 30)", -1, -1, -1, true)]
        [DataRow("hsv(,,)", -1, -1, -1, true)]
        public void FromScaledString_ShouldReturnObject(
            string theString,
            double expectedHue,
            double expectedSaturation,
            double expectedValue,
            bool expectedNull = false
        )
        {
            var hsv = Hsv.FromScaledString(theString);
            if (expectedNull)
            {
                hsv.ShouldBeNull();
            }
            else
            {
                hsv.ShouldBeOfType<Hsv>();
                hsv?.ScaledHue.ShouldBe(expectedHue);
                hsv?.ScaledSaturation.ShouldBe(expectedSaturation);
                hsv?.ScaledValue.ShouldBe(expectedValue);
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
            var hsv = Hsv.FromScaledValues(360, 50, 50);
            hsv.ToRgb().ShouldBeEquivalentTo(new Rgb(128, 64, 64));
        }

        [TestMethod]
        public void ToRgb_ShouldBeCorrect_AtDecimals()
        {
            var hsv = Hsv.FromScaledValues(60.2, 53, 70.91);
            hsv.ToRgb().ShouldBeEquivalentTo(new Rgb(181, 181, 85));
        }

        [TestMethod]
        public void ToHsl_ShouldBeCorrect_AtMaxHue()
        {
            var hsv = Hsv.FromScaledValues(360, 50, 50);
            var hsl = hsv.ToHsl();
            hsl.RoundedScaledHue.ShouldBe(360);
            hsl.RoundedScaledSaturation.ShouldBe(33.33);
            hsl.RoundedScaledLuminance.ShouldBe(37.5);
        }

        [TestMethod]
        public void ToHsl_ShouldBeCorrect_AtDecimals()
        {
            var hsv = Hsv.FromScaledValues(60.2, 53, 70.91);
            var hsl = hsv.ToHsl();
            hsl.RoundedScaledHue.ShouldBe(60.2);
            hsl.RoundedScaledSaturation.ShouldBe(39.25);
            hsl.RoundedScaledLuminance.ShouldBe(52.12);
        }

        [TestMethod]
        public void Hue_ShouldBeClamped()
        {
            var hsv = Hsv.FromScaledValues(-0.01, 100, 100);
            hsv.ScaledHue.ShouldBe(0);

            hsv = new Hsv(360.01, 100, 100);
            hsv.ScaledHue.ShouldBe(360);
        }

        [TestMethod]
        public void Saturation_ShouldBeClamped()
        {
            var hsv = Hsv.FromScaledValues(120, -1, 100);
            hsv.ScaledSaturation.ShouldBe(0);

            hsv = Hsv.FromScaledValues(120, 101, 100);
            hsv.ScaledSaturation.ShouldBe(100);
        }

        [TestMethod]
        public void Value_ShouldBeClamped()
        {
            var hsv = Hsv.FromScaledValues(120, 0, -1);
            hsv.ScaledValue.ShouldBe(0);

            hsv = Hsv.FromScaledValues(120, 0, 101);
            hsv.ScaledValue.ShouldBe(100);
        }

        [TestMethod]
        public void RoundedHue_ShouldRound()
        {
            // Half-up
            var hsv = Hsv.FromScaledValues(240.585, 100, 50);
            hsv.RoundedScaledHue.ShouldBe(240.59);

            // Down
            hsv = Hsv.FromScaledValues(240.584, 100, 50);
            hsv.RoundedScaledHue.ShouldBe(240.58);
        }

        [TestMethod]
        public void RoundedSaturation100_ShouldRound()
        {
            // Half-up
            var hsv = Hsv.FromScaledValues(240, 59.645, 100);
            hsv.RoundedScaledSaturation.ShouldBe(59.65);

            // Down
            hsv = Hsv.FromScaledValues(240, 59.644, 100);
            hsv.RoundedScaledSaturation.ShouldBe(59.64);
        }

        [TestMethod]
        public void RoundedValue100_ShouldRound()
        {
            // Half-up
            var hsv = Hsv.FromScaledValues(240, 100, 59.645);
            hsv.RoundedScaledValue.ShouldBe(59.65);

            // Down
            hsv = Hsv.FromScaledValues(240, 100, 59.644);
            hsv.RoundedScaledValue.ShouldBe(59.64);
        }
    }
}