using com.hexagonsimulations.Geometry.Hex;

namespace com.hexagonsimulations.Geometry.Test
{
    [TestFixture]
    public class FloatCubicTest
    {
        // The range in which floating-point numbers are consider equal.
        public const float EPSILON = 0.000001f;

        [Test]
        public void ConstructorCubic()
        {
            FloatCubic floatCubic = new FloatCubic(new CubeCoordinates(1, 2, 3));

            Assert.That(floatCubic.q, Is.InRange(1f - EPSILON, 1f + EPSILON));
            Assert.That(floatCubic.r, Is.InRange(2f - EPSILON, 2f + EPSILON));
            Assert.That(floatCubic.s, Is.InRange(3f - EPSILON, 3f + EPSILON));
        }

        [Test]
        public void ConstructorXYZ()
        {
            FloatCubic floatCubic = new FloatCubic(1f, 2f, 3f);

            Assert.That(floatCubic.q, Is.EqualTo(1f));
            Assert.That(floatCubic.r, Is.EqualTo(2f));
            Assert.That(floatCubic.s, Is.EqualTo(3f));
        }

        [Test]
        public void ConstructorParameterless()
        {
            FloatCubic floatCubic = new FloatCubic();

            Assert.That(floatCubic.q, Is.EqualTo(0f));
            Assert.That(floatCubic.r, Is.EqualTo(0f));
            Assert.That(floatCubic.s, Is.EqualTo(0f));
        }

        [Test]
        public void ToFloatAxial()
        {
            FloatAxial floatAxial = new FloatCubic(1f, 2f, 3f).ToFloatAxial();

            Assert.That(floatAxial.q, Is.InRange(1f - EPSILON, 1f + EPSILON));
            Assert.That(floatAxial.r, Is.InRange(3f - EPSILON, 3f + EPSILON));
        }

        [Test]
        public void Round()
        {
            FloatCubic floatCubic = new FloatAxial(1.2f, 2.2f).ToFloatCubic();
            CubeCoordinates rounded = floatCubic.Round();
            AxialCoordinates axial = rounded.ToAxial();

            Assert.That(axial.q, Is.EqualTo(1));
            Assert.That(axial.r, Is.EqualTo(2));
        }

        [Test]
        public void Scale()
        {
            FloatCubic floatCubic = new FloatCubic(1f, 2f, -3f).Scale(3f);

            Assert.That(floatCubic.q, Is.InRange(3f - EPSILON, 3f + EPSILON));
            Assert.That(floatCubic.r, Is.InRange(6f - EPSILON, 6f + EPSILON));
            Assert.That(floatCubic.s, Is.InRange(-9f - EPSILON, -9f + EPSILON));
        }
    }
}
