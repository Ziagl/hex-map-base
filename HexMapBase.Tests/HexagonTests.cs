using com.hexagonsimulations.HexMapBase.Models;

namespace HexMapBase.Tests;

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
}
