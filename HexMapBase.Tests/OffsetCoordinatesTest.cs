using com.hexagonsimulations.HexMapBase.Geometry.Hex.Enums;
using com.hexagonsimulations.HexMapBase.Geometry.Hex;

namespace com.hexagonsimulations.HexMapBase.Tests
{
    [TestClass]
    public sealed class OffsetCoordinatesTest
    {
        [TestMethod]
        public void PropertyIsOddRow()
        {
            Assert.IsTrue(new OffsetCoordinates(2, 3).IsOddRow);
            Assert.IsFalse(new OffsetCoordinates(1, 2).IsOddRow);
        }

        [TestMethod]
        public void PropertyRowParity()
        {
            Parity odd = new OffsetCoordinates(2, 3).RowParity;
            Parity even = new OffsetCoordinates(1, 2).RowParity;

            Assert.AreEqual(Parity.Odd, odd);
            Assert.AreEqual(Parity.Even, even);
        }

        [TestMethod]
        public void ConstructorQR()
        {
            OffsetCoordinates offset = new OffsetCoordinates(1, 2);

            Assert.AreEqual(1, offset.x);
            Assert.AreEqual(2, offset.y);
        }

        [TestMethod]
        public void ConstructorParameterless()
        {
            OffsetCoordinates offset = new OffsetCoordinates();

            Assert.AreEqual(0, offset.x);
            Assert.AreEqual(0, offset.y);
        }

        [TestMethod]
        public void ToCubic()
        {
            // Odd row
            CubeCoordinates cubic = new OffsetCoordinates(1, 2).ToCubic();

            Assert.AreEqual(0, cubic.q);
            Assert.AreEqual(2, cubic.r);
            Assert.AreEqual(-2, cubic.s);

            // Even row
            cubic = new OffsetCoordinates(2, 3).ToCubic();

            Assert.AreEqual(1, cubic.q);
            Assert.AreEqual(3, cubic.r);
            Assert.AreEqual(-4, cubic.s);
        }

        [TestMethod]
        public void OperatorOverloadPlus()
        {
            OffsetCoordinates offset = new OffsetCoordinates(1, 2) + new OffsetCoordinates(3, 4);

            Assert.AreEqual(4, offset.x);
            Assert.AreEqual(6, offset.y);
        }

        [TestMethod]
        public void OperatorOverloadMinus()
        {
            OffsetCoordinates offset = new OffsetCoordinates(4, 3) - new OffsetCoordinates(1, 2);

            Assert.AreEqual(3, offset.x);
            Assert.AreEqual(1, offset.y);
        }

        [TestMethod]
        public void OperatorOverloadEquals()
        {
            Assert.IsTrue(new OffsetCoordinates(1, 2) == new OffsetCoordinates(1, 2));
            Assert.IsFalse(new OffsetCoordinates(1, 2) == new OffsetCoordinates(3, 4));
        }

        [TestMethod]
        public void OperatorOverloadNotEquals()
        {
            Assert.IsTrue(new OffsetCoordinates(1, 2) != new OffsetCoordinates(3, 4));
            Assert.IsFalse(new OffsetCoordinates(1, 2) != new OffsetCoordinates(1, 2));
        }
    }
}
