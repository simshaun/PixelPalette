using PixelPalette.Color;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace PixelPalette.Tests.Color
{
    [TestClass]
    public class HslTests
    {
        [DataTestMethod]
        [DataRow("hsl(0, 0, 0)", 0, 0, 0)]
        [DataRow("hsl(1, 1, 1)", 1, 1, 1)]
        [DataRow("hsl(0.0, 0.0, 0.0)", 0, 0, 0)]
        [DataRow("hsl(1.0, 1.0, 1.0)", 1, 1, 1)]
        [DataRow("hsl(0.5555, 0.4444, 0.3333)", 0.5555, 0.4444, 0.3333)]
        [DataRow("hsl(-1, -1 , -1)", -1, -1, -1, true)]
        [DataRow("hsl(1.1, 1.1 , 1.1)", -1, -1, -1, true)]
        public void FromString_ShouldReturnObject(
            string theString,
            double expectedHue,
            double expectedSaturation,
            double expectedLuminance,
            bool expectedNull = false
        )
        {
            var hsl = Hsl.FromString(theString);
            if (expectedNull)
            {
                hsl.ShouldBeNull();
            }
            else
            {
                hsl.ShouldBeOfType<Hsl>();
                hsl?.Hue.ShouldBe(expectedHue);
                hsl?.Saturation.ShouldBe(expectedSaturation);
                hsl?.Luminance.ShouldBe(expectedLuminance);
            }
        }

        [DataTestMethod]
        [DataRow("hsl(200,80%,10%)", 200, 80, 10)]
        [DataRow("hsl(200, 90%, 30%)", 200, 90, 30)]
        [DataRow("hsl(200, 75 , 45)", 200, 75, 45)]
        [DataRow("hsl(255, 100 , 30 )", 255, 100, 30)]
        [DataRow("hsl(100.5, 90.4, 80.3)", 100.5, 90.4, 80.3)]
        [DataRow("hsl(255a, 100, 30)", -1, -1, -1, true)]
        [DataRow("hsl(,,)", -1, -1, -1, true)]
        public void FromScaledString_ShouldReturnObject(
            string theString,
            double expectedHue,
            double expectedSaturation,
            double expectedLuminance,
            bool expectedNull = false
        )
        {
            var hsl = Hsl.FromScaledString(theString);
            if (expectedNull)
            {
                hsl.ShouldBeNull();
            }
            else
            {
                hsl.ShouldBeOfType<Hsl>();
                hsl?.ScaledHue.ShouldBe(expectedHue);
                hsl?.ScaledSaturation.ShouldBe(expectedSaturation);
                hsl?.ScaledLuminance.ShouldBe(expectedLuminance);
            }
        }

        [TestMethod, ColorTestData]
        public void ColorConversions_ShouldBeAccurate(ColorData color)
        {
            color.Hsl.ToRgb().ShouldBeEquivalentTo(color.Rgb);

            var hsv = color.Hsl.ToHsv();
            hsv.RoundedHue.ShouldBeEquivalentTo(color.Hsv.RoundedHue);
            hsv.RoundedSaturation.ShouldBeEquivalentTo(color.Hsv.RoundedSaturation);
            hsv.RoundedValue.ShouldBeEquivalentTo(color.Hsv.RoundedValue);
        }

        [TestMethod]
        public void ToRgb_ShouldBeCorrect_AtMaxHue()
        {
            var hsl = Hsl.FromScaledValues(360, 50, 50);
            hsl.ToRgb().ShouldBeEquivalentTo(new Rgb(191, 64, 64));
        }

        [TestMethod]
        public void ToHsv_ShouldBeCorrect_AtMaxHue()
        {
            var hsl = Hsl.FromScaledValues(360, 50, 50);
            var hsv = hsl.ToHsv();
            hsv.RoundedScaledHue.ShouldBe(360);
            hsv.RoundedScaledSaturation.ShouldBe(66.67);
            hsv.RoundedScaledValue.ShouldBe(75);
        }

        [TestMethod]
        public void ToHsv_ShouldBeCorrect_AtDecimals()
        {
            var hsl = Hsl.FromScaledValues(60.2, 53, 70.91);
            var hsv = hsl.ToHsv();
            hsv.RoundedScaledHue.ShouldBe(60.2);
            hsv.RoundedScaledSaturation.ShouldBe(35.72);
            hsv.RoundedScaledValue.ShouldBe(86.33);
        }

        [TestMethod]
        public void ToRgb_ShouldBeCorrect_AtDecimals()
        {
            var hsl = Hsl.FromScaledValues(60.2, 53, 70.91);
            hsl.ToRgb().ShouldBeEquivalentTo(new Rgb(220, 220, 142));
        }

        [TestMethod]
        public void Lighter_ShouldIncreaseLightness()
        {
            var hsl = Hsl.FromScaledValues(240, 100, 80);
            hsl.Lighter(5).ShouldBeEquivalentTo(hsl.WithScaledLuminance(85));
            hsl.Lighter(11).ShouldBeEquivalentTo(hsl.WithScaledLuminance(91));
            hsl.Lighter(30).ShouldBeEquivalentTo(hsl.WithScaledLuminance(100));
        }

        [TestMethod]
        public void Darker_ShouldDecreaseLightness()
        {
            var hsl = Hsl.FromScaledValues(240, 100, 20);
            hsl.Darker(5).ShouldBeEquivalentTo(hsl.WithScaledLuminance(15));
            hsl.Darker(11).ShouldBeEquivalentTo(hsl.WithScaledLuminance(9));
            hsl.Darker(30).ShouldBeEquivalentTo(hsl.WithScaledLuminance(0));
        }

        [TestMethod]
        public void Hue_ShouldBeClamped()
        {
            var hsl = Hsl.FromScaledValues(-0.01, 100, 100);
            hsl.ScaledHue.ShouldBe(0);

            hsl = Hsl.FromScaledValues(360.01, 100, 100);
            hsl.ScaledHue.ShouldBe(360);
        }

        [TestMethod]
        public void Saturation_ShouldBeClamped()
        {
            var hsl = Hsl.FromScaledValues(120, -1, 100);
            hsl.ScaledSaturation.ShouldBe(0);

            hsl = Hsl.FromScaledValues(120, 101, 100);
            hsl.ScaledSaturation.ShouldBe(100);
        }

        [TestMethod]
        public void Luminance_ShouldBeClamped()
        {
            var hsl = Hsl.FromScaledValues(120, 0, -1);
            hsl.ScaledLuminance.ShouldBe(0);

            hsl = Hsl.FromScaledValues(120, 0, 101);
            hsl.ScaledLuminance.ShouldBe(100);
        }

        [TestMethod]
        public void RoundedScaledHue_ShouldRound()
        {
            // Half-up
            var hsl = Hsl.FromScaledValues(240.585, 1, 50);
            hsl.RoundedScaledHue.ShouldBe(240.59);

            // Down
            hsl = Hsl.FromScaledValues(240.584, 1, 50);
            hsl.RoundedScaledHue.ShouldBe(240.58);
        }

        [TestMethod]
        public void RoundedScaledSaturation_ShouldRound()
        {
            // Half-up
            var hsl = Hsl.FromScaledValues(240, 59.645, 100);
            hsl.RoundedScaledSaturation.ShouldBe(59.65);

            // Down
            hsl = Hsl.FromScaledValues(240, 59.644, 100);
            hsl.RoundedScaledSaturation.ShouldBe(59.64);
        }

        [TestMethod]
        public void RoundedScaledLuminance_ShouldRound()
        {
            // Half-up
            var hsl = Hsl.FromScaledValues(240, 100, 59.645);
            hsl.RoundedScaledLuminance.ShouldBe(59.65);

            // Down
            hsl = Hsl.FromScaledValues(240, 100, 59.644);
            hsl.RoundedScaledLuminance.ShouldBe(59.64);
        }
    }
}