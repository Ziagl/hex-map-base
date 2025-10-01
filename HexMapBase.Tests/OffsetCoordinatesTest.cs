using com.hexagonsimulations.HexMapBase.Enums;
using com.hexagonsimulations.HexMapBase.Models;
using System.Text;

namespace com.hexagonsimulations.HexMapBase.Tests;

[TestClass]
public sealed class OffsetCoordinatesTest
{
    [TestMethod]
    public void PropertyIsOddRow()
    {
        Assert.IsTrue(new OffsetCoordinates(2, 3).IsOddRow);
        Assert.IsFalse(new OffsetCoordinates(1, 2).IsOddRow);
    }

    [TestMethod]
    public void PropertyRowParity()
    {
        Parity odd = new OffsetCoordinates(2, 3).RowParity;
        Parity even = new OffsetCoordinates(1, 2).RowParity;

        Assert.AreEqual(Parity.Odd, odd);
        Assert.AreEqual(Parity.Even, even);
    }

    [TestMethod]
    public void ConstructorQR()
    {
        OffsetCoordinates offset = new OffsetCoordinates(1, 2);

        Assert.AreEqual(1, offset.x);
        Assert.AreEqual(2, offset.y);
    }

    [TestMethod]
    public void ConstructorParameterless()
    {
        OffsetCoordinates offset = new OffsetCoordinates();

        Assert.AreEqual(0, offset.x);
        Assert.AreEqual(0, offset.y);
    }

    [TestMethod]
    public void ToCubic()
    {
        // Odd row
        CubeCoordinates cubic = new OffsetCoordinates(1, 2).ToCubic();

        Assert.AreEqual(0, cubic.q);
        Assert.AreEqual(2, cubic.r);
        Assert.AreEqual(-2, cubic.s);

        // Even row
        cubic = new OffsetCoordinates(2, 3).ToCubic();

        Assert.AreEqual(1, cubic.q);
        Assert.AreEqual(3, cubic.r);
        Assert.AreEqual(-4, cubic.s);
    }

    [TestMethod]
    public void OperatorOverloadPlus()
    {
        OffsetCoordinates offset = new OffsetCoordinates(1, 2) + new OffsetCoordinates(3, 4);

        Assert.AreEqual(4, offset.x);
        Assert.AreEqual(6, offset.y);
    }

    [TestMethod]
    public void OperatorOverloadMinus()
    {
        OffsetCoordinates offset = new OffsetCoordinates(4, 3) - new OffsetCoordinates(1, 2);

        Assert.AreEqual(3, offset.x);
        Assert.AreEqual(1, offset.y);
    }

    [TestMethod]
    public void OperatorOverloadEquals()
    {
        Assert.IsTrue(new OffsetCoordinates(1, 2) == new OffsetCoordinates(1, 2));
        Assert.IsFalse(new OffsetCoordinates(1, 2) == new OffsetCoordinates(3, 4));
    }

    [TestMethod]
    public void OperatorOverloadNotEquals()
    {
        Assert.IsTrue(new OffsetCoordinates(1, 2) != new OffsetCoordinates(3, 4));
        Assert.IsFalse(new OffsetCoordinates(1, 2) != new OffsetCoordinates(1, 2));
    }

    [TestMethod]
    public void ToString()
    {
        OffsetCoordinates offset = new OffsetCoordinates(1, 2);
        string result = offset.ToString();
        Assert.AreEqual("OffsetCoordinates(1, 2)", result);
    }

    [TestMethod]
    public void SerializationDeserialization()
    {
        var original = new OffsetCoordinates(7, -3);

        // JSON round-trip
        string json = original.ToJson();
        var fromJson = OffsetCoordinates.FromJson(json);
        Assert.AreEqual(original, fromJson, "JSON serialization/deserialization failed");

        // Binary round-trip
        using var ms = new MemoryStream();
        using (var bw = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true))
        {
            original.Write(bw);
        }
        ms.Position = 0;
        OffsetCoordinates fromBinary;
        using (var br = new BinaryReader(ms, Encoding.UTF8, leaveOpen: false))
        {
            fromBinary = OffsetCoordinates.Read(br);
        }
        Assert.AreEqual(original, fromBinary, "Binary serialization/deserialization failed");

        // Span-based round-trip
        Span<byte> buffer = stackalloc byte[OffsetCoordinates.ByteSize];
        Assert.IsTrue(original.TryWriteBytes(buffer), "Span write failed");
        bool ok = OffsetCoordinates.TryRead(buffer, out var fromSpan);
        Assert.IsTrue(ok, "Span read failed");
        Assert.AreEqual(original, fromSpan, "Span serialization/deserialization failed");
    }

    [TestMethod]
    public void SerializationDeserializationKey()
    {
        var directory = new Dictionary<OffsetCoordinates, string>()
        {
            { new OffsetCoordinates(0, 0), "Test1" },
            { new OffsetCoordinates(1, -1), "Test2" },
            { new OffsetCoordinates(-1, 1), "Test3" },
        };

        string json = System.Text.Json.JsonSerializer.Serialize(directory);
        var fromJson = System.Text.Json.JsonSerializer.Deserialize<Dictionary<OffsetCoordinates, string>>(json);

        Assert.IsNotNull(fromJson);
        Assert.AreEqual(directory.Count, fromJson.Count);
    }
}
