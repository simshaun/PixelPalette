using PixelPalette.Color;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace PixelPalette.Tests.Color
{
    [TestClass]
    public class CmykTests
    {
        [DataTestMethod]
        [DataRow("cmyk(.5, .4, .3, .1)", 0.5, 0.4, 0.3, 0.1)]
        [DataRow("cmyk(0.5, 0.4, 0.3, 0.1)", 0.5, 0.4, 0.3, 0.1)]
        [DataRow("cmyk(0.504, 0.403, 0.302, 0.101)", 0.504, 0.403, 0.302, 0.101)]
        [DataRow("cmyk(1.1, 0, 0, 0)", -1, -1, -1, -1, true)]
        public void FromString_ShouldReturnObject(
            string theString,
            double expectedC,
            double expectedM,
            double expectedY,
            double expectedK,
            bool expectedNull = false
        )
        {
            var cmyk = Cmyk.FromString(theString);
            if (expectedNull)
            {
                cmyk.ShouldBeNull();
            }
            else
            {
                cmyk.ShouldBeOfType<Cmyk>();
                cmyk?.Cyan.ShouldBe(expectedC);
                cmyk?.Magenta.ShouldBe(expectedM);
                cmyk?.Yellow.ShouldBe(expectedY);
                cmyk?.Key.ShouldBe(expectedK);
            }
        }

        [DataTestMethod]
        [DataRow("cmyk(0, 0, 0, 0)", 0, 0, 0, 0)]
        [DataRow("cmyk(100, 100, 100, 100)", 100, 100, 100, 100)]
        [DataRow("cmyk(10%, 20%, 30%, 40%)", 10, 20, 30, 40)]
        [DataRow("cmyk(10,20,30,40)", 10, 20, 30, 40)]
        [DataRow("cmyk(10 , 20 , 30 , 40)", 10, 20, 30, 40)]
        [DataRow("cmyk(10.5, 20.5, 30.5, 40.5)", 10.5, 20.5, 30.5, 40.5)]
        [DataRow("cmyk(50a, 40, 30, 10)", -1, -1, -1, -1, true)]
        [DataRow("cmyk(10, 20, 30, 40", -1, -1, -1, -1, true)]
        public void FromScaledString_ShouldReturnObject(
            string theString,
            double expectedC,
            double expectedM,
            double expectedY,
            double expectedK,
            bool expectedNull = false
        )
        {
            var cmyk = Cmyk.FromScaledString(theString);
            if (expectedNull)
            {
                cmyk.ShouldBeNull();
            }
            else
            {
                cmyk.ShouldBeOfType<Cmyk>();
                cmyk?.ScaledCyan.ShouldBe(expectedC);
                cmyk?.ScaledMagenta.ShouldBe(expectedM);
                cmyk?.ScaledYellow.ShouldBe(expectedY);
                cmyk?.ScaledKey.ShouldBe(expectedK);
            }
        }

        [TestMethod, ColorTestData]
        public void ColorConversions_ShouldBeAccurate(ColorData color)
        {
            var cmyk = color.Rgb.ToCmyk();
            cmyk.RoundedCyan.ShouldBeEquivalentTo(color.Cmyk.RoundedCyan);
            cmyk.RoundedMagenta.ShouldBeEquivalentTo(color.Cmyk.RoundedMagenta);
            cmyk.RoundedYellow.ShouldBeEquivalentTo(color.Cmyk.RoundedYellow);
            cmyk.RoundedKey.ShouldBeEquivalentTo(color.Cmyk.RoundedKey);
        }

        [TestMethod]
        public void Cyan_ShouldBeClamped()
        {
            var cmyk = new Cmyk(1.01, 0, 0, 0);
            cmyk.Cyan.ShouldBe(1.0);

            cmyk = new Cmyk(-1.0, 0, 0, 0);
            cmyk.Cyan.ShouldBe(0.0);
        }

        [TestMethod]
        public void Magenta_ShouldBeClamped()
        {
            var cmyk = new Cmyk(0, 1.01, 0, 0);
            cmyk.Magenta.ShouldBe(1.0);

            cmyk = new Cmyk(0, -1.0, 0, 0);
            cmyk.Magenta.ShouldBe(0.0);
        }

        [TestMethod]
        public void Yellow_ShouldBeClamped()
        {
            var cmyk = new Cmyk(0, 0, 1.01, 0);
            cmyk.Yellow.ShouldBe(1.0);

            cmyk = new Cmyk(0, 0, -1.0, 0);
            cmyk.Yellow.ShouldBe(0.0);
        }

        [TestMethod]
        public void Key_ShouldBeClamped()
        {
            var cmyk = new Cmyk(0, 0, 0, 1.01);
            cmyk.Key.ShouldBe(1.0);

            cmyk = new Cmyk(0, 0, 0, -1.0);
            cmyk.Key.ShouldBe(0.0);
        }
    }
}