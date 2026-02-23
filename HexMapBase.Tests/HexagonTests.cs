using com.hexagonsimulations.HexMapBase.Models;

namespace com.hexagonsimulations.HexMapBase.Tests;

[TestClass]
public sealed class HexagonTests
{
    [TestMethod]
    public void HexagonConstructor()
    {
        var hex = new Hexagon(new Vec2D(0, 0), 256);
        Assert.IsTrue(hex.UpperCorner == new Vec2D(0, 256));
        Assert.IsTrue(hex.LowerCorner == new Vec2D(0, -256));
        Assert.IsTrue(hex.UpperRightCorner == new Vec2D(256, 128));
        Assert.IsTrue(hex.UpperLeftCorner == new Vec2D(-256, 128));
        Assert.IsTrue(hex.LowerRightCorner == new Vec2D(256, -128));
        Assert.IsTrue(hex.LowerLeftCorner == new Vec2D(-256, -128));
    }

    [TestMethod]
    public void HexagonIsInside()
    {
        var hex = new Hexagon(new Vec2D(0, 0), 256);
        Assert.IsTrue(hex.IsInside(new Vec2D(0, 0)));
        Assert.IsTrue(hex.IsInside(new Vec2D(0, 128)));
        Assert.IsTrue(hex.IsInside(new Vec2D(0, -128)));
        Assert.IsTrue(hex.IsInside(new Vec2D(256, 0)));
        Assert.IsTrue(hex.IsInside(new Vec2D(-256, 0)));
        Assert.IsFalse(hex.IsInside(new Vec2D(0, 257)));
        Assert.IsFalse(hex.IsInside(new Vec2D(0, -257)));
        Assert.IsFalse(hex.IsInside(new Vec2D(257, 0)));
        Assert.IsFalse(hex.IsInside(new Vec2D(-257, 0)));
    }

    [TestMethod]
    public void GenerateRandomPoints()
    {
        var list = Hexagon.GeneratePoints(10);
        Assert.HasCount(10, list);
        foreach (var point in list)
        {
            Assert.IsTrue(point.x >= -1 && point.x <= 1);
            Assert.IsTrue(point.y >= -0.85 && point.y <= 0.85);
        }

        int numberOfPoints = 1000;
        float halfWidth = 2.0f;
        float halfHeight = 1.5f;
        list = Hexagon.GeneratePoints(numberOfPoints, halfWidth, halfHeight);
        Assert.HasCount(numberOfPoints, list);
        foreach (var point in list)
        {
            Assert.IsTrue(point.x >= -halfWidth && point.x <= halfWidth);
            Assert.IsTrue(point.y >= -halfHeight && point.y <= halfHeight);
        }
    }
}
