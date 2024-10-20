using System.Collections.Generic;

namespace com.hexagonsimulations.Geometry.Hex;

/// <summary>
/// This is the base class for all hex tiles used in other libraries.
/// It contains at leased its coordinates in a hex grid as defined
/// here and one can add further properties for the use case of the 
/// library.
/// </summary>
public class HexTile
{
    public CubeCoordinates Coordinates { get; set; }

    /// <summary>
    /// Check if this HexTile is equal to an arbitrary object.
    /// </summary>
    /// <returns>Whether or not this HexTile and the given object are equal.</returns>
    public sealed override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        HexTile other = (HexTile)obj;

        return this.Coordinates == other.Coordinates;
    }

    /// <summary>
    /// Get a hash reflecting the contents of the CubeCoordinates.
    /// </summary>
    /// <returns>An integer hash code reflecting the contents of the CubeCoordinates.</returns>
    public sealed override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Coordinates.q.GetHashCode();
        hash = hash * 23 + Coordinates.r.GetHashCode();
        hash = hash * 23 + Coordinates.s.GetHashCode();
        return hash;
    }

    /// <summary>
    /// Get all neighbor tiles of this tile. This function also respects
    /// grid boundaries.
    /// </summary>
    /// <param name="grid">Grid of all tiles of map.</param>
    /// <param name="rows">Number of tile rows.</param>
    /// <param name="columns">Number of tile columns.</param>
    /// <returns>A list of neighbor tiles.</returns>
    public List<HexTile> Neighbors(List<HexTile> grid, int rows, int columns)
    {
        List<HexTile> neighbors = new();

        foreach (var neighborCoordinate in Coordinates.Neighbors())
        {
            var coord = neighborCoordinate.ToOffset();
            if (coord.x < 0 || coord.x >= columns || coord.y < 0 || coord.y >= rows)
            {
                continue;
            }
            neighbors.Add(grid[coord.y * columns + coord.x]);
        }

        return neighbors;
    }
}
