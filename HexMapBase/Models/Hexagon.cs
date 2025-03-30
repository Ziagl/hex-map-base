namespace com.hexagonsimulations.HexMapBase.Models;

/// <summary>
/// A model to perform geometric operations on hexagons. Use for example for hit test (is inside).
/// </summary>
public class Hexagon
{
    public float HalfSize;
    public Vec2D CenterPoint;
    public Vec2D UpperRightCorner;
    public Vec2D UpperLeftCorner;
    public Vec2D UpperCorner;
    public Vec2D LowerRightCorner;
    public Vec2D LowerLeftCorner;
    public Vec2D LowerCorner;

    public Hexagon(Vec2D centerPoint, float halfSize)
    {
        this.CenterPoint = centerPoint;
        this.HalfSize = halfSize;

        UpperCorner = centerPoint + new Vec2D(0, 1) * halfSize;
        LowerCorner = centerPoint + new Vec2D(0, -1) * halfSize;

        UpperRightCorner = centerPoint + new Vec2D(1, 0) * halfSize + new Vec2D(0, 1) * halfSize * 0.5f;
        UpperLeftCorner = centerPoint + new Vec2D(-1, 0) * halfSize + new Vec2D(0, 1) * halfSize * 0.5f;
        LowerRightCorner = centerPoint + new Vec2D(1, 0) * halfSize + new Vec2D(0, -1) * halfSize * 0.5f;
        LowerLeftCorner = centerPoint + new Vec2D(-1, 0) * halfSize + new Vec2D(0, -1) * halfSize * 0.5f;
    }

    /// <summary>
    /// Tests if a point is exactly inside the hexagon.
    /// </summary>
    /// <param name="point">Point to test.</param>
    /// <returns>true if point is inside, otherwise false.</returns>
    public bool IsInside(Vec2D point)
    {
        // basic bounding box check
        if(point.x <= UpperRightCorner.x &&
           point.x >= LowerLeftCorner.x)
        {
            if (point.y <= UpperCorner.y && 
                point.y >= LowerCorner.y)
            {
                // detailed test
                var dirFromUpperRightCornerToUpperCorner = UpperCorner - UpperRightCorner;
                var dotDirUpperRightCorner = Vec2D.Rotate(dirFromUpperRightCornerToUpperCorner, 90);
                var dirFromUpperRightCornerToTestPoint = point - UpperRightCorner;
                double dotUpperRightCorner = Vec2D.DotProduct(Vec2D.Normalize(dotDirUpperRightCorner), Vec2D.Normalize(dirFromUpperRightCornerToTestPoint));

                var dirFromUpperLeftCornerToUpperCorner = UpperCorner - UpperLeftCorner;
                var dotDirUpperLeftCorner = Vec2D.Rotate(dirFromUpperLeftCornerToUpperCorner, -90);
                var dirFromUpperLeftCornerToTestPoint = point - UpperLeftCorner;
                double dotUpperLeftCorner = Vec2D.DotProduct(Vec2D.Normalize(dotDirUpperLeftCorner), Vec2D.Normalize(dirFromUpperLeftCornerToTestPoint));

                var dirFromLowerRightCornerToLowerCorner = LowerCorner - LowerRightCorner;
                var dotDirLowerRightCorner = Vec2D.Rotate(dirFromLowerRightCornerToLowerCorner, -90);
                var dirFromLowerRightCornerToTestPoint = point - LowerRightCorner;
                double dotLowerRightCorner = Vec2D.DotProduct(Vec2D.Normalize(dotDirLowerRightCorner), Vec2D.Normalize(dirFromLowerRightCornerToTestPoint));

                var dirFromLowerLeftCornerToLowerCorner = LowerCorner - LowerLeftCorner;
                var dotDirLowerLeftCorner = Vec2D.Rotate(dirFromLowerLeftCornerToLowerCorner, 90);
                var dirFromLowerLeftCornerToTestPoint = point - LowerLeftCorner;
                double dotLowerLeftCorner = Vec2D.DotProduct(Vec2D.Normalize(dotDirLowerLeftCorner), Vec2D.Normalize(dirFromLowerLeftCornerToTestPoint));

                if (dotUpperRightCorner >= 0 &&
                    dotUpperLeftCorner >= 0 &&
                    dotLowerRightCorner >= 0 &&
                    dotLowerLeftCorner >= 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
