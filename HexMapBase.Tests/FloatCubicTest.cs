using com.hexagonsimulations.HexMapBase.Models;

namespace com.hexagonsimulations.HexMapBase.Tests;

[TestClass]
public sealed class FloatCubicTest
{
    // The range in which floating-point numbers are consider equal.
    public const float EPSILON = 0.000001f;

    [TestMethod]
    public void ConstructorCubic()
    {
        FloatCubic floatCubic = new FloatCubic(new CubeCoordinates(1, 2, 3));

        Assert.IsTrue(floatCubic.q >= 1f - EPSILON && floatCubic.q <= 1f + EPSILON);
        Assert.IsTrue(floatCubic.r >= 2f - EPSILON && floatCubic.r <= 2f + EPSILON);
        Assert.IsTrue(floatCubic.s >= 3f - EPSILON && floatCubic.s <= 3f + EPSILON);
    }

    [TestMethod]
    public void ConstructorXYZ()
    {
        FloatCubic floatCubic = new FloatCubic(1f, 2f, 3f);

        Assert.AreEqual(1f, floatCubic.q);
        Assert.AreEqual(2f, floatCubic.r);
        Assert.AreEqual(3f, floatCubic.s);
    }

    [TestMethod]
    public void ConstructorParameterless()
    {
        FloatCubic floatCubic = new FloatCubic();

        Assert.AreEqual(0f, floatCubic.q);
        Assert.AreEqual(0f, floatCubic.r);
        Assert.AreEqual(0f, floatCubic.s);
    }

    [TestMethod]
    public void ToFloatAxial()
    {
        FloatAxial floatAxial = new FloatCubic(1f, 2f, 3f).ToFloatAxial();

        Assert.IsTrue(floatAxial.q >= 1f - EPSILON && floatAxial.q <= 1f + EPSILON);
        Assert.IsTrue(floatAxial.r >= 3f - EPSILON && floatAxial.r <= 3f + EPSILON);
    }

    [TestMethod]
    public void Round()
    {
        FloatCubic floatCubic = new FloatAxial(1.2f, 2.2f).ToFloatCubic();
        CubeCoordinates rounded = floatCubic.Round();
        AxialCoordinates axial = rounded.ToAxial();

        Assert.AreEqual(1, axial.q);
        Assert.AreEqual(2, axial.r);
    }

    [TestMethod]
    public void Scale()
    {
        FloatCubic floatCubic = new FloatCubic(1f, 2f, -3f).Scale(3f);

        Assert.IsTrue(floatCubic.q >= 3f - EPSILON && floatCubic.q <= 3f + EPSILON);
        Assert.IsTrue(floatCubic.r >= 6f - EPSILON && floatCubic.q <= 6f + EPSILON);
        Assert.IsTrue(floatCubic.s >= -9f - EPSILON && floatCubic.s <= -9f + EPSILON);
    }
}
