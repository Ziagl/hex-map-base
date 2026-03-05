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
        int count = 10;
        float minDistance = 0.05f;
        var list = Hexagon.GeneratePoints(count, minDistance: minDistance);
        Assert.HasCount(count, list);
        foreach (var point in list)
        {
            Assert.IsTrue(point.x >= -1 && point.x <= 1);
            Assert.IsTrue(point.y >= -0.85 && point.y <= 0.85);
        }
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                Assert.IsGreaterThanOrEqualTo(minDistance, list[i].Distance(list[j]));
            }
        }

        int numberOfPoints = 1000;
        float halfWidth = 2.0f;
        float halfHeight = 1.5f;
        list = Hexagon.GeneratePoints(numberOfPoints, halfWidth, halfHeight, minDistance);
        Assert.HasCount(numberOfPoints, list);
        foreach (var point in list)
        {
            Assert.IsTrue(point.x >= -halfWidth && point.x <= halfWidth);
            Assert.IsTrue(point.y >= -halfHeight && point.y <= halfHeight);
        }
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                Assert.IsGreaterThanOrEqualTo(minDistance, list[i].Distance(list[j]));
            }
        }
    }

    [TestMethod]
    public void GenerateRandomPointsExtremeCase()
    {
        // hexagon diameter (~0.283) is smaller than minDistance (1.0),
        // so no two points can ever be 1.0 apart inside it — at most 1 point is placed
        int maxCount = 100;
        float halfWidth = 0.1f;
        float halfHeight = 0.1f;
        float minDistance = 1.0f;
        var list = Hexagon.GeneratePoints(maxCount, halfWidth, halfHeight, minDistance);
        Assert.IsLessThan(maxCount, list.Count);
        Assert.IsLessThanOrEqualTo(1, list.Count);
    }
}
