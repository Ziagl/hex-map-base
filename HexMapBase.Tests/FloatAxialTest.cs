using com.hexagonsimulations.Geometry.Hex;

namespace com.hexagonsimulations.Geometry.HexGridTest
{
    [TestFixture]
    public class FloatAxialTest
    {
        // The range in which floating-point numbers are consider equal.
        public const float EPSILON = 0.000001f;

        [Test]
        public void ConstructorAxial()
        {
            AxialCoordinates axial = new AxialCoordinates(1, 2);
            FloatAxial floatAxial = new FloatAxial(axial);

            Assert.That(floatAxial.q, Is.InRange(1f - EPSILON, 1f + EPSILON));
            Assert.That(floatAxial.r, Is.InRange(2f - EPSILON, 2f + EPSILON));
        }

        [Test]
        public void ConstructorQR()
        {
            FloatAxial floatAxial = new FloatAxial(1f, 2f);

            Assert.That(floatAxial.q, Is.EqualTo(1f));
            Assert.That(floatAxial.r, Is.EqualTo(2f));
        }

        [Test]
        public void ConstructorParameterless()
        {
            FloatAxial floatAxial = new FloatAxial();

            Assert.That(floatAxial.q, Is.EqualTo(0f));
            Assert.That(floatAxial.r, Is.EqualTo(0f));
        }

        [Test]
        public void ToFloatCubic()
        {
            FloatCubic floatCubic = new FloatAxial(1f, 2f).ToFloatCubic();

            Assert.That(floatCubic.q, Is.InRange(1f - EPSILON, 1f + EPSILON));
            Assert.That(floatCubic.r, Is.InRange(-3f - EPSILON, -3f + EPSILON));
            Assert.That(floatCubic.s, Is.InRange(2f - EPSILON, 2f + EPSILON));
        }
    }
}
