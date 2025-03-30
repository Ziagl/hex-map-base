using com.hexagonsimulations.HexMapBase.Models;

namespace com.hexagonsimulations.HexMapBase.Tests;

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
    public void OperatorMultiply()
    {
        Vec2D point = new Vec2D(1f, 1f) * 3f;
        Assert.IsTrue(point.x >= 3f - EPSILON && point.x <= 3f + EPSILON);
        Assert.IsTrue(point.y >= 3f - EPSILON && point.y <= 3f + EPSILON);
    }

    [TestMethod]
    public void OperatorCompare()
    {
        Vec2D point1 = new Vec2D(1f, 1f);
        Vec2D point2 = new Vec2D(1f, 1f);
        Assert.IsTrue(point1 == point2);
    }

    [TestMethod]
    public void OperatorNotEqual()
    {
        Vec2D point1 = new Vec2D(1f, 1f);
        Vec2D point2 = new Vec2D(2f, 2f);
        Assert.IsTrue(point1 != point2);
    }

    [TestMethod]
    public void Rotate()
    {
        Vec2D point = new Vec2D(1, 0);
        Vec2D rotated = Vec2D.Rotate(point, 90);
        Assert.IsTrue(rotated.x >= 0 - EPSILON && rotated.x <= 0 + EPSILON);
        Assert.IsTrue(rotated.y >= 1 - EPSILON && rotated.y <= 1 + EPSILON);
    
        rotated = Vec2D.Rotate(point, -90);
        Assert.IsTrue(rotated.x >= 0 - EPSILON && rotated.x <= 0 + EPSILON);
        Assert.IsTrue(rotated.y >= -1 - EPSILON && rotated.y <= -1 + EPSILON);
    }

    [TestMethod]
    public void Normalize()
    {
        Vec2D point = new Vec2D(256, 0);
        Vec2D normalized = Vec2D.Normalize(point);
        Assert.IsTrue(normalized.x >= 1 - EPSILON && normalized.x <= 1 + EPSILON);
        Assert.IsTrue(normalized.y >= 0 - EPSILON && normalized.y <= 0 + EPSILON);
    }

    [TestMethod]
    public void DotProduct()
    {
        Vec2D point1 = new Vec2D(1, 1);
        Vec2D point2 = new Vec2D(2, 2);
        double dotProduct = Vec2D.DotProduct(point1, point2);
        double expected = 4;
        Assert.IsTrue(dotProduct >= expected - EPSILON && dotProduct <= expected + EPSILON);
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
