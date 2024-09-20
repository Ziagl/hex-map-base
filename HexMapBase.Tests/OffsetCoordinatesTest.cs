using com.hexagonsimulations.Geometry.Hex;

namespace com.hexagonsimulations.Geometry.HexGridTest
{
    [TestFixture]
    public class OffsetCoordinatesTest
    {
        [Test]
        public void PropertyIsOddRow()
        {
            bool isTrue = new OffsetCoordinates(2, 3).IsOddRow;
            bool isFalse = new OffsetCoordinates(1, 2).IsOddRow;

            Assert.That(isTrue, Is.True);
            Assert.That(isFalse, Is.False);
        }

        [Test]
        public void PropertyRowParity()
        {
            Parity odd = new OffsetCoordinates(2, 3).RowParity;
            Parity even = new OffsetCoordinates(1, 2).RowParity;

            Assert.That(odd, Is.EqualTo(Parity.Odd));
            Assert.That(even, Is.EqualTo(Parity.Even));
        }

        [Test]
        public void ConstructorQR()
        {
            OffsetCoordinates offset = new OffsetCoordinates(1, 2);

            Assert.That(offset.x, Is.EqualTo(1));
            Assert.That(offset.y, Is.EqualTo(2));
        }

        [Test]
        public void ConstructorParameterless()
        {
            OffsetCoordinates offset = new OffsetCoordinates();

            Assert.That(offset.x, Is.EqualTo(0));
            Assert.That(offset.y, Is.EqualTo(0));
        }

        [Test]
        public void ToCubic()
        {
            // Odd row
            CubeCoordinates cubic = new OffsetCoordinates(1, 2).ToCubic();

            Assert.That(cubic.q, Is.EqualTo(0));
            Assert.That(cubic.r, Is.EqualTo(2));
            Assert.That(cubic.s, Is.EqualTo(-2));

            // Even row
            cubic = new OffsetCoordinates(2, 3).ToCubic();

            Assert.That(cubic.q, Is.EqualTo(1));
            Assert.That(cubic.r, Is.EqualTo(3));
            Assert.That(cubic.s, Is.EqualTo(-4));
        }

        [Test]
        public void OperatorOverloadPlus()
        {
            OffsetCoordinates offset = new OffsetCoordinates(1, 2) + new OffsetCoordinates(3, 4);

            Assert.That(offset.x, Is.EqualTo(4));
            Assert.That(offset.y, Is.EqualTo(6));
        }

        [Test]
        public void OperatorOverloadMinus()
        {
            OffsetCoordinates offset = new OffsetCoordinates(4, 3) - new OffsetCoordinates(1, 2);

            Assert.That(offset.x, Is.EqualTo(3));
            Assert.That(offset.y, Is.EqualTo(1));
        }

        [Test]
        public void OperatorOverloadEquals()
        {
            bool isTrue = new OffsetCoordinates(1, 2) == new OffsetCoordinates(1, 2);
            bool isFalse = new OffsetCoordinates(1, 2) == new OffsetCoordinates(3, 4);

            Assert.That(isTrue, Is.True);
            Assert.That(isFalse, Is.False);
        }

        [Test]
        public void OperatorOverloadNotEquals()
        {
            bool isTrue = new OffsetCoordinates(1, 2) != new OffsetCoordinates(3, 4);
            bool isFalse = new OffsetCoordinates(1, 2) != new OffsetCoordinates(1, 2);

            Assert.That(isTrue, Is.True);
            Assert.That(isFalse, Is.False);
        }
    }
}
