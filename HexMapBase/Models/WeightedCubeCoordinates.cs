namespace com.hexagonsimulations.HexMapBase.Geometry.Hex.Models;

/// <summary>
/// Weighted coordinates for path finding and movement.
/// </summary>
public record WeightedCubeCoordinates
{
    public CubeCoordinates Coordinates;
    public int Cost;
}
