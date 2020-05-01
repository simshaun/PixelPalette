using PixelPalette.Color;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace PixelPalette.Tests.Color
{
    [TestClass]
    public class HslTests
    {
        [DataTestMethod]
        [DataRow("hsl(200,80%,10%)", 200, 80, 10)]
        [DataRow("hsl(200, 90%, 30%)", 200, 90, 30)]
        [DataRow("hsl(200, 75 , 45)", 200, 75, 45)]
        [DataRow("hsl(255, 100 , 30 )", 255, 100, 30)]
        [DataRow("hsl(100.5, 90.4, 80.3)", 100.5, 90.4, 80.3)]
        [DataRow("hsl(255a, 100, 30)", 0, 0, 0, true)]
        [DataRow("hsl(,,)", 0, 0, 0, true)]
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
            var hsl = new Hsl(360, 50, 50);
            hsl.ToRgb().ShouldBeEquivalentTo(new Rgb(191, 64, 64));
        }

        [TestMethod]
        public void ToHsv_ShouldBeCorrect_AtMaxHue()
        {
            var hsl = new Hsl(360, 50, 50);
            var hsv = hsl.ToHsv();
            hsv.Hue.ShouldBe(360);
            hsv.RoundedSaturation.ShouldBe(66.67);
            hsv.RoundedValue.ShouldBe(75);
        }

        [TestMethod]
        public void ToHsv_ShouldBeCorrect_AtDecimals()
        {
            var hsl = new Hsl(60.2, 53, 70.91);
            var hsv = hsl.ToHsv();
            hsv.Hue.ShouldBe(60.2);
            hsv.RoundedSaturation.ShouldBe(35.72);
            hsv.RoundedValue.ShouldBe(86.33);
        }

        [TestMethod]
        public void ToRgb_ShouldBeCorrect_AtDecimals()
        {
            var hsl = new Hsl(60.2, 53, 70.91);
            hsl.ToRgb().ShouldBeEquivalentTo(new Rgb(220, 220, 142));
        }

        [TestMethod]
        public void Lighter_ShouldIncreaseLightness()
        {
            var hsl = new Hsl(240, 100, 80);
            hsl.Lighter(5).ShouldBeEquivalentTo(hsl.WithLuminance(85));
            hsl.Lighter(11).ShouldBeEquivalentTo(hsl.WithLuminance(91));
            hsl.Lighter(30).ShouldBeEquivalentTo(hsl.WithLuminance(100));
        }

        [TestMethod]
        public void Darker_ShouldDecreaseLightness()
        {
            var hsl = new Hsl(240, 100, 20);
            hsl.Darker(5).ShouldBeEquivalentTo(hsl.WithLuminance(15));
            hsl.Darker(11).ShouldBeEquivalentTo(hsl.WithLuminance(9));
            hsl.Darker(30).ShouldBeEquivalentTo(hsl.WithLuminance(0));
        }

        [TestMethod]
        public void Hue_ShouldBeClamped()
        {
            var hsl = new Hsl(-0.01, 100, 100);
            hsl.Hue.ShouldBe(0);

            hsl = new Hsl(360.01, 100, 100);
            hsl.Hue.ShouldBe(360);
        }

        [TestMethod]
        public void Saturation_ShouldBeClamped()
        {
            var hsl = new Hsl(120, -1, 100);
            hsl.Saturation.ShouldBe(0);

            hsl = new Hsl(120, 101, 100);
            hsl.Saturation.ShouldBe(100);
        }

        [TestMethod]
        public void Luminance_ShouldBeClamped()
        {
            var hsl = new Hsl(120, 0, -1);
            hsl.Luminance.ShouldBe(0);

            hsl = new Hsl(120, 0, 101);
            hsl.Luminance.ShouldBe(100);
        }

        [TestMethod]
        public void RoundedHue_ShouldRound()
        {
            // Half-up
            var hsl = new Hsl(240.585, 1, 50);
            hsl.RoundedHue.ShouldBe(240.59);

            // Down
            hsl = new Hsl(240.584, 1, 50);
            hsl.RoundedHue.ShouldBe(240.58);
        }

        [TestMethod]
        public void RoundedSaturation100_ShouldRound()
        {
            // Half-up
            var hsl = new Hsl(240, 59.645, 100);
            hsl.RoundedSaturation.ShouldBe(59.65);

            // Down
            hsl = new Hsl(240, 59.644, 100);
            hsl.RoundedSaturation.ShouldBe(59.64);
        }

        [TestMethod]
        public void RoundedLuminance100_ShouldRound()
        {
            // Half-up
            var hsl = new Hsl(240, 100, 59.645);
            hsl.RoundedLuminance.ShouldBe(59.65);

            // Down
            hsl = new Hsl(240, 100, 59.644);
            hsl.RoundedLuminance.ShouldBe(59.64);
        }
    }
}