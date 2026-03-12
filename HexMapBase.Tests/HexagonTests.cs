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

    [TestMethod]
    public void GenerateCircularPoints_CorrectCount()
    {
        int circles = 3;
        int pointsPerCircle = 6;
        var dict = Hexagon.GenerateCircularPoints(circles, pointsPerCircle);
        // 1 center (key 0) + 3 circle keys = 4 keys total
        Assert.HasCount(circles + 1, dict);
        // total points: 1 center + 3 circles * 6 points = 19
        int totalPoints = dict.Values.Sum(v => v.Count);
        Assert.AreEqual(1 + circles * pointsPerCircle, totalPoints);
    }

    [TestMethod]
    public void GenerateCircularPoints_CenterIsOrigin()
    {
        var dict = Hexagon.GenerateCircularPoints(2, 4);
        Assert.IsTrue(dict[0][0] == new Vec2D(0, 0));
    }

    [TestMethod]
    public void GenerateCircularPoints_PointsOnSameCircleEquidistantFromCenter()
    {
        int circles = 2;
        int pointsPerCircle = 5;
        float halfWidth = 1f;
        float halfHeight = 1f; // use equal scaling so circles stay circular
        var dict = Hexagon.GenerateCircularPoints(circles, pointsPerCircle, halfWidth, halfHeight);
        var center = dict[0][0];

        for (int c = 1; c <= circles; c++)
        {
            var circlePoints = dict[c];
            float firstDist = circlePoints[0].Distance(center);
            for (int p = 1; p < pointsPerCircle; p++)
            {
                float dist = circlePoints[p].Distance(center);
                Assert.IsLessThan(0.001f, Math.Abs(dist - firstDist), $"Circle {c}: point {p} distance {dist} differs from first {firstDist}");
            }
        }
    }

    [TestMethod]
    public void GenerateCircularPoints_PointsOnSameCircleEquallySpaced()
    {
        int circles = 1;
        int pointsPerCircle = 3;
        float halfWidth = 1f;
        float halfHeight = 1f;
        var dict = Hexagon.GenerateCircularPoints(circles, pointsPerCircle, halfWidth, halfHeight);
        var c1 = dict[1];

        // with equal scaling the 3 points form an equilateral triangle
        float d01 = c1[0].Distance(c1[1]);
        float d12 = c1[1].Distance(c1[2]);
        float d20 = c1[2].Distance(c1[0]);
        Assert.IsLessThan(0.001f, Math.Abs(d01 - d12));
        Assert.IsLessThan(0.001f, Math.Abs(d12 - d20));
    }

    [TestMethod]
    public void GenerateCircularPoints_AdjacentCirclesAreOffset()
    {
        int circles = 2;
        int pointsPerCircle = 4;
        float halfWidth = 1f;
        float halfHeight = 1f;
        var dict = Hexagon.GenerateCircularPoints(circles, pointsPerCircle, halfWidth, halfHeight);

        // circle 1 point 0 angle = 0 => (r1, 0)
        // circle 2 point 0 angle = pi/4 => rotated 45 degrees
        // verify first point of circle 2 is not at the same angle as circle 1
        var c1p0 = dict[1][0];
        var c2p0 = dict[2][0];
        // angles should differ — dot product of unit vectors should not be 1
        var c1dir = Vec2D.Normalize(c1p0);
        var c2dir = Vec2D.Normalize(c2p0);
        double dot = Vec2D.DotProduct(c1dir, c2dir);
        Assert.IsLessThan(0.99, Math.Abs(dot), $"Adjacent circle points should be offset, dot={dot}");
    }

    [TestMethod]
    public void GenerateCircularPoints_ZeroCirclesReturnsCenterOnly()
    {
        var dict = Hexagon.GenerateCircularPoints(0, 5);
        Assert.HasCount(1, dict);
        Assert.IsTrue(dict[0][0] == new Vec2D(0, 0));
    }
}
