using com.hexagonsimulations.HexMapBase.Models;

namespace com.hexagonsimulations.HexMapBase.Tests;

[TestClass]
public sealed class FloatAxialTest
{
    // The range in which floating-point numbers are consider equal.
    public const float EPSILON = 0.000001f;

    [TestMethod]
    public void ConstructorAxial()
    {
        AxialCoordinates axial = new AxialCoordinates(1, 2);
        FloatAxial floatAxial = new FloatAxial(axial);

        Assert.IsTrue(floatAxial.q >= 1f - EPSILON && floatAxial.q <= 1f + EPSILON);
        Assert.IsTrue(floatAxial.r >= 2f - EPSILON && floatAxial.r <= 2f + EPSILON);
    }

    [TestMethod]
    public void ConstructorQR()
    {
        FloatAxial floatAxial = new FloatAxial(1f, 2f);

        Assert.AreEqual(1f, floatAxial.q);
        Assert.AreEqual(2f, floatAxial.r);
    }

    [TestMethod]
    public void ConstructorParameterless()
    {
        FloatAxial floatAxial = new FloatAxial();

        Assert.AreEqual(0f, floatAxial.q);
        Assert.AreEqual(0f, floatAxial.r);
    }

    [TestMethod]
    public void ToFloatCubic()
    {
        FloatCubic floatCubic = new FloatAxial(1f, 2f).ToFloatCubic();

        Assert.IsTrue(floatCubic.q >= 1f - EPSILON && floatCubic.q <= 1f + EPSILON);
        Assert.IsTrue(floatCubic.r >= -3f - EPSILON && floatCubic.r <= -3f + EPSILON);
        Assert.IsTrue(floatCubic.s >= 2f - EPSILON && floatCubic.s <= 2f + EPSILON);
    }
}
