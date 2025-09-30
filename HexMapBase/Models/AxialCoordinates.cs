using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.hexagonsimulations.HexMapBase.Models;

/// <summary>
/// Represents the position of a hex within a hex grid using axial coordinates (q = East,
/// r = Southeast). This results in a grid with a naturally parallelgraphic form, but can
/// still be stored in a regtangular array resulting in a small number of wasted cells.
/// </summary>
/// <remarks>This type is the less computationally efficient type to use for hex grid
/// computations than CubeCoordinate, as all of the work has to be done by the CubeCoordinate
/// type and converting between AxialCoordinale and CubeCoordinate is a small cost every time
/// a result must be obtained.</remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct AxialCoordinates : IEquatable<AxialCoordinates>
{
    // Public fields preserved for backward compatibility with existing code/tests.
    // System.Text.Json does not serialize fields unless IncludeFields = true (handled in JsonOptions).
    [DataMember(Order = 1)]
    [JsonInclude]
    public int q;

    [DataMember(Order = 2)]
    [JsonInclude]
    public int r;

    /// <summary>
    /// Size in bytes when serialized as two little-endian 32-bit integers.
    /// </summary>
    public const int ByteSize = sizeof(int) * 2;

    /// <summary>
    /// Default JSON serializer options (enables field inclusion).
    /// </summary>
    public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        IncludeFields = true,
        WriteIndented = false
    };

    /// <summary>
    /// Create a new AxialCoordinate given the coordinates q and r.
    /// </summary>
    /// <param name="q">The column position of the hex within the grid.</param>
    /// <param name="r">The row position of the hex within the grid.</param>
    public AxialCoordinates(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    /// <summary>
    /// Return this hex as a CubeCoordinate.
    /// </summary>
    /// <returns>A CubeCoordinate representing the hex.</returns>
    public CubeCoordinates ToCubic()
    {
        int x = this.q;
        int z = this.r;
        int y = -x - z;
        return new CubeCoordinates(x, y, z);
    }

    /// <summary>
    /// Serialize this instance to JSON.
    /// </summary>
    public string ToJson(JsonSerializerOptions options = null) =>
        JsonSerializer.Serialize(this, options ?? JsonOptions);

    /// <summary>
    /// Deserialize from JSON into an AxialCoordinates instance.
    /// </summary>
    public static AxialCoordinates FromJson(string json, JsonSerializerOptions options = null) =>
        JsonSerializer.Deserialize<AxialCoordinates>(json, options ?? JsonOptions);

    /// <summary>
    /// Write this instance as two 32-bit ints (q, r) to a BinaryWriter.
    /// </summary>
    public void Write(System.IO.BinaryWriter writer)
    {
        writer.Write(q);
        writer.Write(r);
    }

    /// <summary>
    /// Read an AxialCoordinates written by Write().
    /// </summary>
    public static AxialCoordinates Read(System.IO.BinaryReader reader) =>
        new AxialCoordinates(reader.ReadInt32(), reader.ReadInt32());

    /// <summary>
    /// Write to a byte span (little-endian). Returns false if destination too small.
    /// </summary>
    public bool TryWriteBytes(Span<byte> destination)
    {
        if (destination.Length < ByteSize) return false;
        System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(destination.Slice(0, 4), q);
        System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(destination.Slice(4, 4), r);
        return true;
    }

    /// <summary>
    /// Read from a span previously written by TryWriteBytes.
    /// </summary>
    public static bool TryRead(ReadOnlySpan<byte> source, out AxialCoordinates value)
    {
        value = default;
        if (source.Length < ByteSize) return false;
        int q = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(source.Slice(0, 4));
        int r = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(source.Slice(4, 4));
        value = new AxialCoordinates(q, r);
        return true;
    }

    /// <summary>
    /// Add 2 AxialCoordinates together and return the result.
    /// </summary>
    /// <param name="lhs">The AxialCoordinate on the left-hand side of the + sign.</param>
    /// <param name="rhs">The AxialCoordinate on the right-hand side of the + sign.</param>
    /// <returns>A new AxialCoordinate representing the sum of the inputs.</returns>
    public static AxialCoordinates operator +(AxialCoordinates lhs, AxialCoordinates rhs)
    {
        int q = lhs.q + rhs.q;
        int r = lhs.r + rhs.r;

        return new AxialCoordinates(q, r);
    }

    /// <summary>
    /// Subtract 1 AxialCoordinate from another and return the result.
    /// </summary>
    /// <param name="lhs">The AxialCoordinate on the left-hand side of the - sign.</param>
    /// <param name="rhs">The AxialCoordinate on the right-hand side of the - sign.</param>
    /// <returns>A new AxialCoordinate representing the difference of the inputs.</returns>
    public static AxialCoordinates operator -(AxialCoordinates lhs, AxialCoordinates rhs)
    {
        int q = lhs.q - rhs.q;
        int r = lhs.r - rhs.r;

        return new AxialCoordinates(q, r);
    }

    /// <summary>
    /// Check if 2 AxialCoordinates represent the same hex on the grid.
    /// </summary>
    /// <param name="lhs">The AxialCoordinate on the left-hand side of the == sign.</param>
    /// <param name="rhs">The AxialCoordinate on the right-hand side of the == sign.</param>
    /// <returns>A bool representing whether or not the AxialCoordinates are equal.</returns>
    public static bool operator ==(AxialCoordinates lhs, AxialCoordinates rhs)
    {
        return (lhs.q == rhs.q) && (lhs.r == rhs.r);
    }

    /// <summary>
    /// Check if 2 AxialCoordinates represent the different hexes on the grid.
    /// </summary>
    /// <param name="lhs">The AxialCoordinate on the left-hand side of the != sign.</param>
    /// <param name="rhs">The AxialCoordinate on the right-hand side of the != sign.</param>
    /// <returns>A bool representing whether or not the AxialCoordinates are unequal.</returns>
    public static bool operator !=(AxialCoordinates lhs, AxialCoordinates rhs)
    {
        return (lhs.q != rhs.q) || (lhs.r != rhs.r);
    }

    /// <summary>
    /// Get a hash reflecting the contents of the AxialCoordinate.
    /// </summary>
    /// <returns>An integer hash code reflecting the contents of the AxialCoordinate.</returns>
    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + q.GetHashCode();
        hash = hash * 23 + r.GetHashCode();
        return hash;
    }

    public bool Equals(AxialCoordinates other) => (q == other.q) && (r == other.r);

    public override bool Equals(object obj) => obj is AxialCoordinates other && Equals(other);

    /// <summary>
    /// Returns a string representation of the axial coordinates.
    /// </summary>
    /// <returns>A string in the format "AxialCoordinates(q, r)" where q and r are the coordinate values.</returns>
    public override string ToString()
    {
        return $"AxialCoordinates({q}, {r})";
    }
}
