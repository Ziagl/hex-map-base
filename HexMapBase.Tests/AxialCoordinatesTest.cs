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

    [TestMethod]
    public void ToString()
    {
        AxialCoordinates axial = new AxialCoordinates(1, 2);
        string result = axial.ToString();
        Assert.AreEqual("AxialCoordinates(1, 2)", result);
    }

    [TestMethod]
    public void SerializationDeserialization()
    {
        var original = new AxialCoordinates(5, -3);

        // JSON
        string json = original.ToJson();
        var fromJson = AxialCoordinates.FromJson(json);
        Assert.AreEqual(original, fromJson, "JSON serialization/deserialization failed");

        // Binary
        using var ms = new MemoryStream();
        using (var bw = new BinaryWriter(ms, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            original.Write(bw);
        }
        ms.Position = 0;
        AxialCoordinates fromBinary;
        using (var br = new BinaryReader(ms, System.Text.Encoding.UTF8, leaveOpen: false))
        {
            fromBinary = AxialCoordinates.Read(br);
        }
        Assert.AreEqual(original, fromBinary, "Binary serialization/deserialization failed");

        // Span
        Span<byte> buffer = stackalloc byte[AxialCoordinates.ByteSize];
        Assert.IsTrue(original.TryWriteBytes(buffer), "Span write failed");
        bool ok = AxialCoordinates.TryRead(buffer, out var fromSpan);
        Assert.IsTrue(ok, "Span read failed");
        Assert.AreEqual(original, fromSpan, "Span serialization/deserialization failed");
    }

    [TestMethod]
    public void SerializationDeserializationKey()
    {
        var directory = new Dictionary<AxialCoordinates, string>()
        {
            { new AxialCoordinates(0, 0), "Test1" },
            { new AxialCoordinates(1, -1), "Test2" },
            { new AxialCoordinates(-1, 1), "Test3" },
        };

        string json = System.Text.Json.JsonSerializer.Serialize(directory);
        var fromJson = System.Text.Json.JsonSerializer.Deserialize<Dictionary<AxialCoordinates, string>>(json);

        Assert.IsNotNull(fromJson);
        Assert.AreEqual(directory.Count, fromJson.Count);
    }
}
