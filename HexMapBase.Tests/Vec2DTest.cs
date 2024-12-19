using com.hexagonsimulations.HexMapBase.Geometry.Hex;

namespace com.hexagonsimulations.HexMapBase.Tests
{
    [TestClass]
    public sealed class Vec2DTest
    {
        // The range in which floating-point numbers are consider equal.
        public const float EPSILON = 0.000001f;

        [TestMethod]
        public void ConstructorXZ()
        {
            Vec2D point = new Vec2D(1f, 2f);

            Assert.AreEqual(1f, point.x);
            Assert.AreEqual(2, point.y);
        }

        [TestMethod]
        public void ConstructorParameterless()
        {
            Vec2D point = new Vec2D();

            Assert.AreEqual(0f, point.x);
            Assert.AreEqual(0f, point.y);
        }

        [TestMethod]
        public void OperatorOverloadPlus()
        {
            Vec2D point = new Vec2D(1f, 2f) + new Vec2D(3f, 4f);

            Assert.IsTrue(point.x >= 4f - EPSILON && point.x <= 4f + EPSILON);
            Assert.IsTrue(point.y >= 6f - EPSILON && point.y <= 6f + EPSILON);
        }

        [TestMethod]
        public void OperatorOverloadMinus()
        {
            Vec2D point = new Vec2D(4f, 3f) - new Vec2D(1f, 2f);

            Assert.IsTrue(point.x >= 3f - EPSILON && point.x <= 3f + EPSILON);
            Assert.IsTrue(point.y >= 1f - EPSILON && point.y <= 1f + EPSILON);
        }

        [TestMethod]
        public void Distance()
        {
            Vec2D point1 = new Vec2D(1, 1);
            Vec2D point2 = new Vec2D(3, 3);

            float length = point1.Distance(point2);
            float expected = (float)(2 * Math.Sqrt(2));

            Assert.IsTrue(length >= expected - EPSILON && length <= expected + EPSILON);
        }

        [TestMethod]
        public void Length()
        {
            Vec2D point = new Vec2D(2, 2);

            float length = point.Length();
            float expected = (float)(2 * Math.Sqrt(2));

            Assert.IsTrue(length >= expected - EPSILON && length <= expected + EPSILON);
        }
    }
}
