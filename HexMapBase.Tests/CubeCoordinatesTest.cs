using com.hexagonsimulations.HexMapBase.Enums;
using com.hexagonsimulations.HexMapBase.Models;

namespace com.hexagonsimulations.HexMapBase.Tests;

[TestClass]
public sealed class CubeCoordinatesTest
{
    [TestMethod]
    public void ConstructorQRS()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 2, 3);

        Assert.AreEqual(1, cubic.q);
        Assert.AreEqual(2, cubic.r);
        Assert.AreEqual(3, cubic.s);
    }

    [TestMethod]
    public void ConstructorParameterless()
    {
        CubeCoordinates cubic = new CubeCoordinates();

        Assert.AreEqual(0, cubic.q);
        Assert.AreEqual(0, cubic.r);
        Assert.AreEqual(0, cubic.s);
    }

    [TestMethod]
    public void ToAxial()
    {
        AxialCoordinates axial = new CubeCoordinates(1, 2, 3).ToAxial();

        Assert.AreEqual(1, axial.q);
        Assert.AreEqual(3, axial.r);
    }

    [TestMethod]
    public void ToOffset()
    {
        OffsetCoordinates offset = new CubeCoordinates(0, 2, -2).ToOffset();

        Assert.AreEqual(1, offset.x);
        Assert.AreEqual(2, offset.y);
    }

    [TestMethod]
    public void OperatorOverloadPlus()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 2, 3) + new CubeCoordinates(4, 5, 6);

        Assert.AreEqual(5, cubic.q);
        Assert.AreEqual(7, cubic.r);
        Assert.AreEqual(9, cubic.s);
    }

    [TestMethod]
    public void OperatorOverloadMinus()
    {
        CubeCoordinates cubic = new CubeCoordinates(6, 5, 4) - new CubeCoordinates(1, 2, 3);

        Assert.AreEqual(5, cubic.q);
        Assert.AreEqual(3, cubic.r);
        Assert.AreEqual(1, cubic.s);
    }

    [TestMethod]
    public void OperatorOverloadEquals()
    {
        Assert.IsTrue(new CubeCoordinates(1, 2, 3) == new CubeCoordinates(1, 2, 3));
        Assert.IsFalse(new CubeCoordinates(1, 2, 3) == new CubeCoordinates(4, 5, 6));
    }

    [TestMethod]
    public void OperatorOverloadNotEquals()
    {
        Assert.IsTrue(new CubeCoordinates(1, 2, 3) != new CubeCoordinates(4, 5, 6));
        Assert.IsFalse(new CubeCoordinates(1, 2, 3) != new CubeCoordinates(1, 2, 3));
    }

    [TestMethod]
    public void AreaAround()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 0, -1);
        CubeCoordinates[] area = CubeCoordinates.Area(cubic, 2);

        // Center
        CollectionAssert.Contains(area, new CubeCoordinates(1, 0, -1));
        // Distance 1
        CollectionAssert.Contains(area, new CubeCoordinates(1, 1, -2));
        CollectionAssert.Contains(area, new CubeCoordinates(2, 0, -2));
        CollectionAssert.Contains(area, new CubeCoordinates(0, 1, -1));
        CollectionAssert.Contains(area, new CubeCoordinates(2, -1, -1));
        CollectionAssert.Contains(area, new CubeCoordinates(1, -1, 0));
        CollectionAssert.Contains(area, new CubeCoordinates(0, 0, 0));
        // Distance 2
        CollectionAssert.Contains(area, new CubeCoordinates(-1, 2, -1));
        CollectionAssert.Contains(area, new CubeCoordinates(0, 2, -2));
        CollectionAssert.Contains(area, new CubeCoordinates(1, 2, -3));
        CollectionAssert.Contains(area, new CubeCoordinates(2, 1, -3));
        CollectionAssert.Contains(area, new CubeCoordinates(3, 0, -3));
        CollectionAssert.Contains(area, new CubeCoordinates(3, -1, -2));
        CollectionAssert.Contains(area, new CubeCoordinates(3, -2, -1));
        CollectionAssert.Contains(area, new CubeCoordinates(2, -2, 0));
        CollectionAssert.Contains(area, new CubeCoordinates(1, -2, 1));
        CollectionAssert.Contains(area, new CubeCoordinates(0, -1, 1));
        CollectionAssert.Contains(area, new CubeCoordinates(-1, 0, 1));
        CollectionAssert.Contains(area, new CubeCoordinates(-1, 1, 0));
    }

    [TestMethod]
    public void Diagonal()
    {
        CubeCoordinates cubic = new CubeCoordinates(0, 1, -1).Diagonal(Enums.Diagonal.ESE);

        Assert.AreEqual(1, cubic.q);
        Assert.AreEqual(2, cubic.r);
        Assert.AreEqual(-3, cubic.s);
    }

    [TestMethod]
    public void Diagonals()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 2, 3);
        CubeCoordinates[] diagonals = cubic.Diagonals();

        CollectionAssert.AreEquivalent(
            diagonals,
            new CubeCoordinates[6]
            {
                cubic.Diagonal(Enums.Diagonal.ESE),
                cubic.Diagonal(Enums.Diagonal.S),
                cubic.Diagonal(Enums.Diagonal.WSW),
                cubic.Diagonal(Enums.Diagonal.WNW),
                cubic.Diagonal(Enums.Diagonal.N),
                cubic.Diagonal(Enums.Diagonal.ENE),
            }
        );
    }

    [TestMethod]
    public void DistanceTo()
    {
        CubeCoordinates cubic1 = new CubeCoordinates(0, 0, 0);
        CubeCoordinates cubic2 = cubic1.Neighbor(Direction.E).Neighbor(Direction.SE);
        int distance = cubic1.DistanceTo(cubic2);

        Assert.AreEqual(2, distance);
    }

    [TestMethod]
    public void LineTo()
    {
        CubeCoordinates startCubic = new CubeCoordinates(1, 0, -1);
        CubeCoordinates endCubic = new CubeCoordinates(-2, 2, 0);
        CubeCoordinates[] line = startCubic.LineTo(endCubic);

        CollectionAssert.AreEquivalent(
            line,
            new CubeCoordinates[4]
            {
                new CubeCoordinates(1, 0, -1),
                new CubeCoordinates(0, 1, -1),
                new CubeCoordinates(-1, 1, 0),
                new CubeCoordinates(-2, 2, 0),
            }
        );
    }

    [TestMethod]
    public void Neighbor()
    {
        CubeCoordinates cubic = new CubeCoordinates(0, 1, -1).Neighbor(Direction.E);

        Assert.AreEqual(1, cubic.q);
        Assert.AreEqual(1, cubic.r);
        Assert.AreEqual(-2, cubic.s);

        cubic = new CubeCoordinates(0, 0, 0).Neighbor(Direction.NE);

        Assert.AreEqual(1, cubic.q);
        Assert.AreEqual(-1, cubic.r);
        Assert.AreEqual(0, cubic.s);
    }

    [TestMethod]
    public void Neighbors()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 2, 3);
        CubeCoordinates[] neighbors = cubic.Neighbors();

        CollectionAssert.AreEquivalent(
            neighbors,
            new CubeCoordinates[6]
            {
                cubic.Neighbor(Direction.E),
                cubic.Neighbor(Direction.SE),
                cubic.Neighbor(Direction.SW),
                cubic.Neighbor(Direction.W),
                cubic.Neighbor(Direction.NW),
                cubic.Neighbor(Direction.NE),
            }
        );
    }

    [TestMethod]
    public void RingAround()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 0, -1);
        CubeCoordinates[] ring = cubic.RingAround(2, Direction.W);

        CollectionAssert.AreEquivalent(
            ring,
            new CubeCoordinates[12]
            {
                new CubeCoordinates(-1, 2, -1),
                new CubeCoordinates(0, 2, -2),
                new CubeCoordinates(1, 2, -3),
                new CubeCoordinates(2, 1, -3),
                new CubeCoordinates(3, 0, -3),
                new CubeCoordinates(3, -1, -2),
                new CubeCoordinates(3, -2, -1),
                new CubeCoordinates(2, -2, 0),
                new CubeCoordinates(1, -2, 1),
                new CubeCoordinates(0, -1, 1),
                new CubeCoordinates(-1, 0, 1),
                new CubeCoordinates(-1, 1, 0),
            }
        );
    }

    [TestMethod]
    public void RotateAroundOther()
    {
        // Not yet implemented
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void RotateOtherAround()
    {
        // Not yet implemented
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void Scale()
    {
        CubeCoordinates cubic = CubeCoordinates.DirectionDiff(Direction.SE);
        CubeCoordinates scaled = cubic.Scale(3);

        Assert.AreEqual(3 * cubic.q, scaled.q);
        Assert.AreEqual(3 * cubic.r, scaled.r);
        Assert.AreEqual(3 * cubic.s, scaled.s);
    }

    [TestMethod]
    public void SpiralAroundInward()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 0, -1);
        CubeCoordinates[] spiral = cubic.SpiralAroundInward(2, Direction.W);

        CollectionAssert.AreEquivalent(
            spiral,
            new CubeCoordinates[19]
            {
                // Distance 2
                new CubeCoordinates(-1, 2, -1),
                new CubeCoordinates(0, 2, -2),
                new CubeCoordinates(1, 2, -3),
                new CubeCoordinates(2, 1, -3),
                new CubeCoordinates(3, 0, -3),
                new CubeCoordinates(3, -1, -2),
                new CubeCoordinates(3, -2, -1),
                new CubeCoordinates(2, -2, 0),
                new CubeCoordinates(1, -2, 1),
                new CubeCoordinates(0, -1, 1),
                new CubeCoordinates(-1, 0, 1),
                new CubeCoordinates(-1, 1, 0),
                // Distance 1
                new CubeCoordinates(0, 1, -1),
                new CubeCoordinates(1, 1, -2),
                new CubeCoordinates(2, 0, -2),
                new CubeCoordinates(2, -1, -1),
                new CubeCoordinates(1, -1, 0),
                new CubeCoordinates(0, 0, 0),
                // Center
                new CubeCoordinates(1, 0, -1),
            }
        );
    }

    [TestMethod]
    public void SpiralAroundOutward()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 0, -1);
        CubeCoordinates[] spiral = cubic.SpiralAroundOutward(2, Direction.W);

        CollectionAssert.AreEquivalent(
            spiral,
            new CubeCoordinates[19]
            {
                // Center
                new CubeCoordinates(1, 0, -1),
                // Distance 1
                new CubeCoordinates(0, 1, -1),
                new CubeCoordinates(1, 1, -2),
                new CubeCoordinates(2, 0, -2),
                new CubeCoordinates(2, -1, -1),
                new CubeCoordinates(1, -1, 0),
                new CubeCoordinates(0, 0, 0),
                // Distance 2
                new CubeCoordinates(-1, 2, -1),
                new CubeCoordinates(0, 2, -2),
                new CubeCoordinates(1, 2, -3),
                new CubeCoordinates(2, 1, -3),
                new CubeCoordinates(3, 0, -3),
                new CubeCoordinates(3, -1, -2),
                new CubeCoordinates(3, -2, -1),
                new CubeCoordinates(2, -2, 0),
                new CubeCoordinates(1, -2, 1),
                new CubeCoordinates(0, -1, 1),
                new CubeCoordinates(-1, 0, 1),
                new CubeCoordinates(-1, 1, 0),
            }
        );
    }

    [TestMethod]
    public void StaticArea()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 0, -1);
        CubeCoordinates[] area = CubeCoordinates.Area(cubic, 2);

        // Center
        CollectionAssert.Contains(area, new CubeCoordinates(1, 0, -1));
        // Distance 1
        CollectionAssert.Contains(area, new CubeCoordinates(1, 1, -2));
        CollectionAssert.Contains(area, new CubeCoordinates(2, 0, -2));
        CollectionAssert.Contains(area, new CubeCoordinates(0, 1, -1));
        CollectionAssert.Contains(area, new CubeCoordinates(2, -1, -1));
        CollectionAssert.Contains(area, new CubeCoordinates(1, -1, 0));
        CollectionAssert.Contains(area, new CubeCoordinates(0, 0, 0));
        // Distance 2
        CollectionAssert.Contains(area, new CubeCoordinates(-1, 2, -1));
        CollectionAssert.Contains(area, new CubeCoordinates(0, 2, -2));
        CollectionAssert.Contains(area, new CubeCoordinates(1, 2, -3));
        CollectionAssert.Contains(area, new CubeCoordinates(2, 1, -3));
        CollectionAssert.Contains(area, new CubeCoordinates(3, 0, -3));
        CollectionAssert.Contains(area, new CubeCoordinates(3, -1, -2));
        CollectionAssert.Contains(area, new CubeCoordinates(3, -2, -1));
        CollectionAssert.Contains(area, new CubeCoordinates(2, -2, 0));
        CollectionAssert.Contains(area, new CubeCoordinates(1, -2, 1));
        CollectionAssert.Contains(area, new CubeCoordinates(0, -1, 1));
        CollectionAssert.Contains(area, new CubeCoordinates(-1, 0, 1));
        CollectionAssert.Contains(area, new CubeCoordinates(-1, 1, 0));
    }

    [TestMethod]
    public void StaticDiagonalDiff()
    {
        CubeCoordinates cubic = CubeCoordinates.DiagonalDiff(Enums.Diagonal.ESE);

        Assert.AreEqual(1, cubic.q);
        Assert.AreEqual(1, cubic.r);
        Assert.AreEqual(-2, cubic.s);
    }

    [TestMethod]
    public void StaticDirectionDiff()
    {
        CubeCoordinates cubic = CubeCoordinates.DirectionDiff(Direction.E);

        Assert.AreEqual(1, cubic.q);
        Assert.AreEqual(0, cubic.r);
        Assert.AreEqual(-1, cubic.s);
    }

    [TestMethod]
    public void StaticIntersectRanges()
    {
        // Not yet implemented
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void StaticLine()
    {
        CubeCoordinates startCubic = new CubeCoordinates(1, 0, -1);
        CubeCoordinates endCubic = new CubeCoordinates(-2, 2, 0);
        CubeCoordinates[] line = CubeCoordinates.Line(startCubic, endCubic);

        CollectionAssert.AreEquivalent(
            line,
            new CubeCoordinates[4]
            {
                new CubeCoordinates(1, 0, -1),
                new CubeCoordinates(0, 1, -1),
                new CubeCoordinates(-1, 1, 0),
                new CubeCoordinates(-2, 2, 0),
            }
        );
    }

    [TestMethod]
    public void StaticRing()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 0, -1);
        CubeCoordinates[] ring = CubeCoordinates.Ring(cubic, 2, Direction.W);

        CollectionAssert.AreEquivalent(
            ring,
            new CubeCoordinates[12]
            {
                new CubeCoordinates(-1, 2, -1),
                new CubeCoordinates(0, 2, -2),
                new CubeCoordinates(1, 2, -3),
                new CubeCoordinates(2, 1, -3),
                new CubeCoordinates(3, 0, -3),
                new CubeCoordinates(3, -1, -2),
                new CubeCoordinates(3, -2, -1),
                new CubeCoordinates(2, -2, 0),
                new CubeCoordinates(1, -2, 1),
                new CubeCoordinates(0, -1, 1),
                new CubeCoordinates(-1, 0, 1),
                new CubeCoordinates(-1, 1, 0),
            }
        );
    }

    [TestMethod]
    public void StaticRotate()
    {
        // Not yet implemented
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void StaticSpiralInward()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 0, -1);
        CubeCoordinates[] spiral = CubeCoordinates.SpiralOutward(cubic, 2, Direction.W);

        CollectionAssert.AreEquivalent(
            spiral,
            new CubeCoordinates[19]
            {
                // Distance 2
                new CubeCoordinates(-1, 2, -1),
                new CubeCoordinates(0, 2, -2),
                new CubeCoordinates(1, 2, -3),
                new CubeCoordinates(2, 1, -3),
                new CubeCoordinates(3, 0, -3),
                new CubeCoordinates(3, -1, -2),
                new CubeCoordinates(3, -2, -1),
                new CubeCoordinates(2, -2, 0),
                new CubeCoordinates(1, -2, 1),
                new CubeCoordinates(0, -1, 1),
                new CubeCoordinates(-1, 0, 1),
                new CubeCoordinates(-1, 1, 0),
                // Distance 1
                new CubeCoordinates(0, 1, -1),
                new CubeCoordinates(1, 1, -2),
                new CubeCoordinates(2, 0, -2),
                new CubeCoordinates(2, -1, -1),
                new CubeCoordinates(1, -1, 0),
                new CubeCoordinates(0, 0, 0),
                // Center
                new CubeCoordinates(1, 0, -1),
            }
        );
    }

    [TestMethod]
    public void StaticSpiralOutward()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 0, -1);
        CubeCoordinates[] spiral = CubeCoordinates.SpiralOutward(cubic, 2, Direction.W);

        CollectionAssert.AreEquivalent(
            spiral,
            new CubeCoordinates[19]
            {
                // Center
                new CubeCoordinates(1, 0, -1),
                // Distance 1
                new CubeCoordinates(0, 1, -1),
                new CubeCoordinates(1, 1, -2),
                new CubeCoordinates(2, 0, -2),
                new CubeCoordinates(2, -1, -1),
                new CubeCoordinates(1, -1, 0),
                new CubeCoordinates(0, 0, 0),
                // Distance 2
                new CubeCoordinates(-1, 2, -1),
                new CubeCoordinates(0, 2, -2),
                new CubeCoordinates(1, 2, -3),
                new CubeCoordinates(2, 1, -3),
                new CubeCoordinates(3, 0, -3),
                new CubeCoordinates(3, -1, -2),
                new CubeCoordinates(3, -2, -1),
                new CubeCoordinates(2, -2, 0),
                new CubeCoordinates(1, -2, 1),
                new CubeCoordinates(0, -1, 1),
                new CubeCoordinates(-1, 0, 1),
                new CubeCoordinates(-1, 1, 0),
            }
        );
    }

    [TestMethod]
    public void ToString()
    {
        CubeCoordinates cubic = new CubeCoordinates(1, 2, 3);
        string result = cubic.ToString();
        Assert.AreEqual("CubeCoordinates(1, 2, 3)", result);
    }

    [TestMethod]
    public void SerializationDeserialization()
    {
        var original = new CubeCoordinates(3, -2, -1); // q + r + s = 0 invariant

        // JSON
        string json = original.ToJson();
        var fromJson = CubeCoordinates.FromJson(json);
        Assert.AreEqual(original, fromJson, "JSON serialization/deserialization failed");

        // Binary
        using var ms = new MemoryStream();
        using (var bw = new BinaryWriter(ms, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            original.Write(bw);
        }
        ms.Position = 0;
        CubeCoordinates fromBinary;
        using (var br = new BinaryReader(ms, System.Text.Encoding.UTF8, leaveOpen: false))
        {
            fromBinary = CubeCoordinates.Read(br);
        }
        Assert.AreEqual(original, fromBinary, "Binary serialization/deserialization failed");

        // Span-based
        Span<byte> buffer = stackalloc byte[CubeCoordinates.ByteSize];
        Assert.IsTrue(original.TryWriteBytes(buffer), "Span write failed");
        bool readOk = CubeCoordinates.TryRead(buffer, out var fromSpan);
        Assert.IsTrue(readOk, "Span read failed");
        Assert.AreEqual(original, fromSpan, "Span serialization/deserialization failed");
    }

    [TestMethod]
    public void SerializationDeserializationKey()
    {
        var directory = new Dictionary<CubeCoordinates, string>()
        {
            { new CubeCoordinates(0, 0, 0), "Test1" },
            { new CubeCoordinates(1, -1, 0), "Test2" }, 
            { new CubeCoordinates(-1, 1, 0), "Test3" },
        };

        string json = System.Text.Json.JsonSerializer.Serialize(directory);
        var fromJson = System.Text.Json.JsonSerializer.Deserialize<Dictionary<CubeCoordinates, string>>(json);

        Assert.IsNotNull(fromJson);
        Assert.HasCount(directory.Count, fromJson);
    }
}