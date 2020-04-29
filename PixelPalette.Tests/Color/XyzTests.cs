using PixelPalette.Color;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace PixelPalette.Tests.Color
{
    [TestClass]
    public class XyzTests
    {
        [TestMethod, ColorTestData]
        public void ColorConversions_ShouldBeAccurate(ColorData color)
        {
            color.Xyz.ToRgb().ShouldBeEquivalentTo(color.Rgb);

            var lab = color.Xyz.ToLab();
            lab.RoundedL.ShouldBeEquivalentTo(color.Lab.RoundedL);
            lab.RoundedA.ShouldBeEquivalentTo(color.Lab.RoundedA);
            lab.RoundedB.ShouldBeEquivalentTo(color.Lab.RoundedB);
        }

        [TestMethod]
        public void X_ShouldBeClamped()
        {
            var xyz = new Xyz(-1, 0, 0);
            xyz.X.ShouldBe(Xyz.MinX);

            xyz = new Xyz(100, 0, 0);
            xyz.X.ShouldBe(Xyz.MaxX);
        }

        [TestMethod]
        public void Y_ShouldBeClamped()
        {
            var xyz = new Xyz(0, -1, 0);
            xyz.Y.ShouldBe(Xyz.MinY);

            xyz = new Xyz(0, 120, 0);
            xyz.Y.ShouldBe(Xyz.MaxY);
        }

        [TestMethod]
        public void Z_ShouldBeClamped()
        {
            var xyz = new Xyz(0, 0, -1);
            xyz.Z.ShouldBe(Xyz.MinZ);

            xyz = new Xyz(0, 0, 120);
            xyz.Z.ShouldBe(Xyz.MaxZ);
        }
    }
}