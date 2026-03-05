using System;
using System.Collections.Generic;
using System.Numerics;

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

    // generates up to maxCount uniformly distributed random points inside a stretched hexagon,
    // each at least minDistance apart from all previously placed points
    public static List<Vec2D> GeneratePoints(int maxCount, float halfWidth = 1f, float halfHeight = 0.85f, float minDistance = 0.05f)
    {
        var points = new List<Vec2D>(maxCount);
        var rand = new Random();
        const int maxAttemptsPerPoint = 30;

        for (int i = 0; i < maxCount; i++)
        {
            bool placed = false;
            for (int attempt = 0; attempt < maxAttemptsPerPoint; attempt++)
            {
                Vec2D p = RandomPointInUnitHex(rand);
                p.x *= halfWidth;
                p.y *= halfHeight;

                if (IsFarEnough(p, points, minDistance))
                {
                    points.Add(p);
                    placed = true;
                    break;
                }
            }

            if (!placed)
            {
                break;
            }
        }

        return points;
    }

    private static bool IsFarEnough(Vec2D point, List<Vec2D> points, float minDistance)
    {
        foreach (var p in points)
        {
            if (point.Distance(p) < minDistance)
                return false;
        }
        return true;
    }

    // generates a uniformly distributed point inside a regular hexagon of radius 1
    private static Vec2D RandomPointInUnitHex(Random rand)
    {
        while (true)
        {
            // sample inside bounding box [-1,1] x [-sqrt(3)/2, sqrt(3)/2]
            float x = (float)(rand.NextDouble() * 2 - 1);
            float y = (float)(rand.NextDouble() * Math.Sqrt(3) - Math.Sqrt(3) / 2);

            if (IsInsideUnitHex(x, y))
                return new Vec2D(x, y);
        }
    }

    // check if point is inside a regular hexagon of radius 1
    private static bool IsInsideUnitHex(float x, float y)
    {
        // hexagon inequality
        return Math.Abs(x) <= 1 &&
               Math.Abs(y) <= Math.Sqrt(3) / 2 &&
               Math.Abs(x) + Math.Abs(y) / Math.Sqrt(3) <= 1;
    }


}
