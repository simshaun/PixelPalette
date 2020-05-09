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
            var rgb = color.Hsv.ToRgb();
            rgb.RoundedRed.ShouldBeEquivalentTo(color.Rgb.RoundedRed);
            rgb.RoundedGreen.ShouldBeEquivalentTo(color.Rgb.RoundedGreen);
            rgb.RoundedBlue.ShouldBeEquivalentTo(color.Rgb.RoundedBlue);

            var hsl = color.Hsv.ToHsl();
            hsl.RoundedHue.ShouldBeEquivalentTo(color.Hsl.RoundedHue);
            hsl.RoundedSaturation.ShouldBeEquivalentTo(color.Hsl.RoundedSaturation);
            hsl.RoundedLuminance.ShouldBeEquivalentTo(color.Hsl.RoundedLuminance);
        }

        [TestMethod]
        public void ToRgb_ShouldBeCorrect_AtMaxHue()
        {
            var rgb = Hsv.FromScaledValues(360, 50, 50).ToRgb();
            rgb.ScaledRed.ShouldBeEquivalentTo(128);
            rgb.ScaledGreen.ShouldBeEquivalentTo(64);
            rgb.ScaledBlue.ShouldBeEquivalentTo(64);
        }

        [TestMethod]
        public void ToHsl_ShouldBeCorrect_AtMaxHue()
        {
            var hsl = Hsv.FromScaledValues(360, 50, 50).ToHsl();
            hsl.RoundedScaledHue.ShouldBe(360);
            hsl.RoundedScaledSaturation.ShouldBe(33.33);
            hsl.RoundedScaledLuminance.ShouldBe(37.5);
        }

        [TestMethod]
        public void ToRgb_ShouldBeCorrect_AtDecimals()
        {
            var rgb = Hsv.FromScaledValues(60.2, 53, 70.91).ToRgb();
            rgb.ScaledRed.ShouldBeEquivalentTo(181);
            rgb.ScaledGreen.ShouldBeEquivalentTo(181);
            rgb.ScaledBlue.ShouldBeEquivalentTo(85);

            rgb = Hsv.FromScaledValues(60.3, 53, 70.91).ToRgb();
            rgb.ScaledRed.ShouldBeEquivalentTo(180);
            rgb.ScaledGreen.ShouldBeEquivalentTo(181);
            rgb.ScaledBlue.ShouldBeEquivalentTo(85);
        }

        [TestMethod]
        public void ToHsl_ShouldBeCorrect_AtDecimals()
        {
            var hsl = Hsv.FromScaledValues(60.2, 53, 70.91).ToHsl();
            hsl.RoundedScaledHue.ShouldBe(60.2);
            hsl.RoundedScaledSaturation.ShouldBe(39.25);
            hsl.RoundedScaledLuminance.ShouldBe(52.12);
        }

        [TestMethod]
        public void Hue_ShouldBeClamped()
        {
            Hsv.FromScaledValues(-0.01, 100, 100).ScaledHue.ShouldBe(0);
            new Hsv(360.01, 100, 100).ScaledHue.ShouldBe(360);
        }

        [TestMethod]
        public void Saturation_ShouldBeClamped()
        {
            Hsv.FromScaledValues(120, -1, 100).ScaledSaturation.ShouldBe(0);
            Hsv.FromScaledValues(120, 101, 100).ScaledSaturation.ShouldBe(100);
        }

        [TestMethod]
        public void Value_ShouldBeClamped()
        {
            Hsv.FromScaledValues(120, 0, -1).ScaledValue.ShouldBe(0);
            Hsv.FromScaledValues(120, 0, 101).ScaledValue.ShouldBe(100);
        }

        [TestMethod]
        public void RoundedHue_ShouldRound()
        {
            // Half-up
            Hsv.FromScaledValues(240.585, 100, 50).RoundedScaledHue.ShouldBe(240.59);

            // Down
            Hsv.FromScaledValues(240.584, 100, 50).RoundedScaledHue.ShouldBe(240.58);
        }

        [TestMethod]
        public void RoundedSaturation100_ShouldRound()
        {
            // Half-up
            Hsv.FromScaledValues(240, 59.645, 100).RoundedScaledSaturation.ShouldBe(59.65);

            // Down
            Hsv.FromScaledValues(240, 59.644, 100).RoundedScaledSaturation.ShouldBe(59.64);
        }

        [TestMethod]
        public void RoundedValue100_ShouldRound()
        {
            // Half-up
            Hsv.FromScaledValues(240, 100, 59.645).RoundedScaledValue.ShouldBe(59.65);

            // Down
            Hsv.FromScaledValues(240, 100, 59.644).RoundedScaledValue.ShouldBe(59.64);
        }
    }
}