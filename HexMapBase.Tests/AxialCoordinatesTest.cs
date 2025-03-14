using com.hexagonsimulations.HexMapBase.Models;

namespace com.hexagonsimulations.HexMapBase.Tests;

[TestClass]
public sealed class AxialCoordinatesTest
{
    [TestMethod]
    public void ConstructorQR()
    {
        AxialCoordinates axial = new AxialCoordinates(1, 2);

        Assert.AreEqual(1, axial.q);
        Assert.AreEqual(2, axial.r);
    }

    [TestMethod]
    public void ConstructorParameterless()
    {
        AxialCoordinates axial = new AxialCoordinates();

        Assert.AreEqual(0, axial.q);
        Assert.AreEqual(0, axial.r);
    }

    [TestMethod]
    public void ToCubic()
    {
        CubeCoordinates cubic = new AxialCoordinates(1, 2).ToCubic();

        Assert.AreEqual(1, cubic.q);
        Assert.AreEqual(-3, cubic.r);
        Assert.AreEqual(2, cubic.s);
    }

    [TestMethod]
    public void OperatorOverloadPlus()
    {
        AxialCoordinates axial = new AxialCoordinates(1, 2) + new AxialCoordinates(3, 4);

        Assert.AreEqual(4, axial.q);
        Assert.AreEqual(6, axial.r);
    }

    [TestMethod]
    public void OperatorOverloadMinus()
    {
        AxialCoordinates axial = new AxialCoordinates(4, 3) - new AxialCoordinates(1, 2);

        Assert.AreEqual(3, axial.q);
        Assert.AreEqual(1, axial.r);
    }

    [TestMethod]
    public void OperatorOverloadEquals()
    {
        Assert.IsTrue(new AxialCoordinates(1, 2) == new AxialCoordinates(1, 2));
        Assert.IsFalse(new AxialCoordinates(1, 2) == new AxialCoordinates(3, 4));
    }

    [TestMethod]
    public void OperatorOverloadNotEquals()
    {
        Assert.IsTrue(new AxialCoordinates(1, 2) != new AxialCoordinates(3, 4));
        Assert.IsFalse(new AxialCoordinates(1, 2) != new AxialCoordinates(1, 2));
    }
}
