using PixelPalette.Color;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace PixelPalette.Tests.Color
{
    [TestClass]
    public class LabTests
    {
        [DataTestMethod]
        [DataRow("lab(100, -80, 10)", 100, -80, 10)]
        [DataRow("lab(100, 20, 50)", 100, 20, 50)]
        [DataRow("lab(100a, 20, 50)", 0, 0, 0, true)]
        [DataRow("lab(,,)", 0, 0, 0, true)]
        public void FromString_ShouldReturnObject(
            string theString,
            int expectedL,
            int expectedA,
            int expectedB,
            bool expectedNull = false
        )
        {
            var lab = Lab.FromString(theString);
            if (expectedNull)
            {
                lab.ShouldBeNull();
            }
            else
            {
                lab.ShouldBeOfType<Lab>();
                lab?.L.ShouldBe(expectedL);
                lab?.A.ShouldBe(expectedA);
                lab?.B.ShouldBe(expectedB);
            }
        }

        [TestMethod, ColorTestData]
        public void ColorConversions_ShouldBeAccurate(ColorData color)
        {
            var xyz = color.Lab.ToXyz();
            xyz.RoundedX.ShouldBeEquivalentTo(color.Xyz.RoundedX);
            xyz.RoundedY.ShouldBeEquivalentTo(color.Xyz.RoundedY);
            xyz.RoundedZ.ShouldBeEquivalentTo(color.Xyz.RoundedZ);
        }

        [TestMethod]
        public void L_ShouldBeClamped()
        {
            var lab = new Lab(-1, 0, 0);
            lab.L.ShouldBe(Lab.MinL);

            lab = new Lab(101, 0, 0);
            lab.L.ShouldBe(Lab.MaxL);
        }

        [TestMethod]
        public void A_ShouldBeClamped()
        {
            var lab = new Lab(0, -129, 0);
            lab.A.ShouldBe(Lab.MinA);

            lab = new Lab(0, 128, 0);
            lab.A.ShouldBe(Lab.MaxA);
        }

        [TestMethod]
        public void B_ShouldBeClamped()
        {
            var lab = new Lab(0, 0, -129);
            lab.B.ShouldBe(Lab.MinB);

            lab = new Lab(0, 0, 128);
            lab.B.ShouldBe(Lab.MaxB);
        }
    }
}