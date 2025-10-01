using com.hexagonsimulations.HexMapBase.Enums;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.hexagonsimulations.HexMapBase.Models;

/// <summary>
/// Represents the position of a hex within a hex grid using the Odd-Row offset grid layout
/// scheme. This means that all hexes are pointy-topped and each odd row is offset in the
/// positive r direction by half of a hex width. Offset coordinates are a natural fit for
/// storage in a rectangular array of memory, and can be an ideal storage format for hexes.
/// </summary>
/// <remarks>This type is the least computationally efficient type to use for hex grid
/// computations, as all of the work has to be done by the CubeCoordinate type and converting
/// between OffsetCoordinate and CubeCoordinate is the most computationally expensive of the
/// type conversions provided by this library.</remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(OffsetCoordinatesJsonConverter))]
public struct OffsetCoordinates : IEquatable<OffsetCoordinates>
{
    [DataMember(Order = 1)]
    [JsonInclude]
    public int x;

    [DataMember(Order = 2)]
    [JsonInclude]
    public int y;

    /// <summary>
    /// Size in bytes when written as two little-endian 32-bit integers.
    /// </summary>
    public const int ByteSize = sizeof(int) * 2;

    /// <summary>
    /// Default JSON options (IncludeFields = true to serialize public fields).
    /// </summary>
    public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        IncludeFields = true,
        WriteIndented = false
    };

    /// <summary>
    /// Return whether or not this hex belongs to an odd-numbered row.
    /// </summary>
    public bool IsOddRow => (RowParity == Parity.Odd);

    /// <summary>
    /// Return the row parity of the hex (whether its row number is even or odd).
    /// </summary>
    public Parity RowParity => (Parity)(this.y & 1);

    /// <summary>
    /// Create a new OffsetHexCoord given the coordinates x and y.
    /// </summary>
    public OffsetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Return this hex as a CubeCoordinate.
    /// </summary>
    public CubeCoordinates ToCubic()
    {
        int q = this.x - (this.y - (int)RowParity) / 2;
        int r = this.y;
        int s = -q - r;
        return new CubeCoordinates(q, r, s);
    }

    public string ToJson(JsonSerializerOptions options = null) =>
        JsonSerializer.Serialize(this, options ?? JsonOptions);

    public static OffsetCoordinates FromJson(string json, JsonSerializerOptions options = null) =>
        JsonSerializer.Deserialize<OffsetCoordinates>(json, options ?? JsonOptions);

    public void Write(System.IO.BinaryWriter writer)
    {
        writer.Write(x);
        writer.Write(y);
    }

    public static OffsetCoordinates Read(System.IO.BinaryReader reader) =>
        new OffsetCoordinates(reader.ReadInt32(), reader.ReadInt32());

    public bool TryWriteBytes(Span<byte> destination)
    {
        if (destination.Length < ByteSize) return false;
        System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(destination.Slice(0, 4), x);
        System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(destination.Slice(4, 4), y);
        return true;
    }

    public static bool TryRead(ReadOnlySpan<byte> source, out OffsetCoordinates value)
    {
        value = default;
        if (source.Length < ByteSize) return false;
        int x = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(source.Slice(0, 4));
        int y = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(source.Slice(4, 4));
        value = new OffsetCoordinates(x, y);
        return true;
    }

    /// <summary>
    /// Add 2 OffsetCoordinates together and return the result.
    /// </summary>
    /// <param name="lhs">The OffsetCoordinate on the left-hand side of the + sign.</param>
    /// <param name="rhs">The OffsetCoordinate on the right-hand side of the + sign.</param>
    /// <returns>A new OffsetCoordinate representing the sum of the inputs.</returns>
    public static OffsetCoordinates operator +(OffsetCoordinates lhs, OffsetCoordinates rhs)
    {
        int x = lhs.x + rhs.x;
        int y = lhs.y + rhs.y;

        return new OffsetCoordinates(x, y);
    }

    /// <summary>
    /// Subtract 1 OffsetCoordinate from another and return the result.
    /// </summary>
    /// <param name="lhs">The OffsetCoordinate on the left-hand side of the - sign.</param>
    /// <param name="rhs">The OffsetCoordinate on the right-hand side of the - sign.</param>
    /// <returns>A new OffsetCoordinate representing the difference of the inputs.</returns>
    public static OffsetCoordinates operator -(OffsetCoordinates lhs, OffsetCoordinates rhs)
    {
        int x = lhs.x - rhs.x;
        int y = lhs.y - rhs.y;

        return new OffsetCoordinates(x, y);
    }

    /// <summary>
    /// Check if 2 OffsetCoordinates represent the same hex on the grid.
    /// </summary>
    /// <param name="lhs">The OffsetCoordinate on the left-hand side of the == sign.</param>
    /// <param name="rhs">The OffsetCoordinate on the right-hand side of the == sign.</param>
    /// <returns>A bool representing whether or not the OffsetCoordinates are equal.</returns>
    public static bool operator ==(OffsetCoordinates lhs, OffsetCoordinates rhs)
    {
        return (lhs.x == rhs.x) && (lhs.y == rhs.y);
    }

    /// <summary>
    /// Check if 2 OffsetCoordinates represent the different hexes on the grid.
    /// </summary>
    /// <param name="lhs">The OffsetCoordinate on the left-hand side of the != sign.</param>
    /// <param name="rhs">The OffsetCoordinate on the right-hand side of the != sign.</param>
    /// <returns>A bool representing whether or not the OffsetCoordinates are unequal.</returns>
    public static bool operator !=(OffsetCoordinates lhs, OffsetCoordinates rhs)
    {
        return (lhs.x != rhs.x) || (lhs.y != rhs.y);
    }

    public bool Equals(OffsetCoordinates other) => (x == other.x) && (y == other.y);

    public override bool Equals(object obj) => obj is OffsetCoordinates other && Equals(other);

    /// <summary>
    /// Get a hash reflecting the contents of the OffsetCoordinate.
    /// </summary>
    /// <returns>An integer hash code reflecting the contents of the OffsetCoordinate.</returns>
    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + x.GetHashCode();
        hash = hash * 23 + y.GetHashCode();
        return hash;
    }

    /// <summary>
    /// Returns a string representation of the offset coordinates.
    /// </summary>
    /// <returns>A string in the format "OffsetCoordinates(x, y)" where x and y are the coordinate values.</returns>
    public override string ToString() => $"OffsetCoordinates({x}, {y})";

    internal string ToKeyString() => $"{x},{y}";

    public static bool TryParseKey(string key, out OffsetCoordinates value)
    {
        value = default;
        if (string.IsNullOrWhiteSpace(key)) return false;

        var parts = key.Split(',');
        if (parts.Length != 2) return false;

        if (int.TryParse(parts[0], out int x) &&
            int.TryParse(parts[1], out int y))
        {
            value = new OffsetCoordinates(x, y);
            return true;
        }

        return false;
    }
}

internal sealed class OffsetCoordinatesJsonConverter : JsonConverter<OffsetCoordinates>
{
    public override OffsetCoordinates Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string key = reader.GetString();
            if (OffsetCoordinates.TryParseKey(key, out var value))
                return value;

            throw new JsonException($"Invalid OffsetCoordinates key: {key}");
        }

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject for OffsetCoordinates.");

        int x = 0, y = 0;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return new OffsetCoordinates(x, y);

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "x": x = reader.GetInt32(); break;
                    case "y": y = reader.GetInt32(); break;
                }
            }
        }

        throw new JsonException("Incomplete OffsetCoordinates JSON object.");
    }

    public override void Write(Utf8JsonWriter writer, OffsetCoordinates value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("x", value.x);
        writer.WriteNumber("y", value.y);
        writer.WriteEndObject();
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, OffsetCoordinates value, JsonSerializerOptions options)
    {
        writer.WritePropertyName(value.ToKeyString());
    }

    public override OffsetCoordinates ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string key = reader.GetString();
        if (OffsetCoordinates.TryParseKey(key, out var value))
            return value;

        throw new JsonException($"Invalid OffsetCoordinates dictionary key: {key}");
    }
}
