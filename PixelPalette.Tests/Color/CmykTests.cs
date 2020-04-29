using PixelPalette.Color;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace PixelPalette.Tests.Color
{
    [TestClass]
    public class CmykTests
    {
        [DataTestMethod]
        [DataRow("cmyk(50,40,30,10)", 50, 40, 30, 10)]
        [DataRow("cmyk(50, 40, 30, 10)", 50, 40, 30, 10)]
        [DataRow("cmyk(50a, 40, 30, 10)", 0, 0, 0, 0, true)]
        [DataRow("cmyk(a,b,c,d)", 0, 0, 0, 0, true)]
        public void FromString_ShouldReturnObject(
            string theString,
            int expectedC,
            int expectedM,
            int expectedY,
            int expectedK,
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
            var cmyk = new Cmyk(101, 0, 0, 0);
            cmyk.Cyan.ShouldBe(Cmyk.MaxComponentValue);

            cmyk = new Cmyk(-50, 0, 0, 0);
            cmyk.Cyan.ShouldBe(Cmyk.MinComponentValue);
        }

        [TestMethod]
        public void Magenta_ShouldBeClamped()
        {
            var cmyk = new Cmyk(0, 101, 0, 0);
            cmyk.Magenta.ShouldBe(Cmyk.MaxComponentValue);

            cmyk = new Cmyk(0, -50, 0, 0);
            cmyk.Magenta.ShouldBe(Cmyk.MinComponentValue);
        }

        [TestMethod]
        public void Yellow_ShouldBeClamped()
        {
            var cmyk = new Cmyk(0, 0, 101, 0);
            cmyk.Yellow.ShouldBe(Cmyk.MaxComponentValue);

            cmyk = new Cmyk(0, 0, -50, 0);
            cmyk.Yellow.ShouldBe(Cmyk.MinComponentValue);
        }

        [TestMethod]
        public void Key_ShouldBeClamped()
        {
            var cmyk = new Cmyk(0, 0, 0, 101);
            cmyk.Key.ShouldBe(Cmyk.MaxComponentValue);

            cmyk = new Cmyk(0, 0, 0, -50);
            cmyk.Key.ShouldBe(Cmyk.MinComponentValue);
        }
    }
}