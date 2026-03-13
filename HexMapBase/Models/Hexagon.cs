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

    /// <summary>
    /// Generates up to maxCount uniformly distributed random points inside a stretched hexagon,
    /// each at least minDistance apart from all previously placed points.
    /// </summary>
    /// <param name="maxCount">Maximum number of points to generate.</param>
    /// <param name="halfWidth">Half width of hexagon geometry.</param>
    /// <param name="halfHeight">Half height of hexagon geometry.</param>
    /// <param name="minDistance">Minimal distance number should have to all other ppoints.</param>
    /// <returns>A list containing the random generated points.</returns>
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

    /// <summary>
    /// Generates deterministic points arranged in concentric circles inside a stretched hexagon.
    /// The result is grouped by circle number: key 0 contains the center point, keys 1..numberOfCircles
    /// each contain the evenly spaced points on that circle. Adjacent circles are rotated by half the
    /// angular spacing to maximize distances between circles.
    /// </summary>
    /// <param name="numberOfCircles">Number of concentric circles around the center.</param>
    /// <param name="pointsPerCircle">Number of evenly spaced points on each circle.</param>
    /// <param name="halfWidth">Half-width of the stretched hexagon (default 1).</param>
    /// <param name="halfHeight">Half-height of the stretched hexagon (default 0.85).</param>
    /// <returns>A dictionary keyed by circle number (0 = center) mapping to the points on that circle.</returns>
    public static Dictionary<int, List<Vec2D>> GenerateCircularPoints(int numberOfCircles, int pointsPerCircle, float halfWidth = 1f, float halfHeight = 0.85f)
    {
        var result = new Dictionary<int, List<Vec2D>>(1 + numberOfCircles);

        // center point
        result[0] = [new Vec2D(0, 0)];

        // inradius of the unit hexagon — largest circle that fits entirely inside
        float maxRadius = (float)(Math.Sqrt(3) / 2.0);

        for (int circle = 1; circle <= numberOfCircles; circle++)
        {
            // evenly space circle radii from center to the inradius
            float radius = circle * maxRadius / numberOfCircles;

            // alternate circles are offset by half the angular spacing
            // so that points on adjacent circles are maximally staggered
            float angleOffset = (circle % 2 == 0)
                ? (float)(Math.PI / pointsPerCircle)
                : 0f;

            var circlePoints = new List<Vec2D>(pointsPerCircle);
            for (int p = 0; p < pointsPerCircle; p++)
            {
                float angle = angleOffset + p * 2f * (float)Math.PI / pointsPerCircle;
                float x = radius * (float)Math.Cos(angle) * halfWidth;
                float y = radius * (float)Math.Sin(angle) * halfHeight;
                circlePoints.Add(new Vec2D(x, y));
            }
            result[circle] = circlePoints;
        }

        return result;
    }

    /// <summary>
    /// Generates dynamically distributed points arranged in concentric circle bands inside a stretched hexagon.
    /// The result is grouped by circle number: key 0 contains the center point, keys 1..numberOfCircles
    /// contain randomly distributed points in the corresponding ring band.
    ///
    /// Distances between all generated points are globally constrained to be at least <paramref name="minDistance"/>.
    /// The outermost ring is limited to 80% of the maximum inscribed radius so points keep a 20% margin to the border.
    /// If constraints are too strict, the method returns a best-effort result with fewer points in affected rings.
    /// </summary>
    /// <param name="numberOfCircles">Number of concentric circles around the center (excluding center key 0).</param>
    /// <param name="minDistance">Minimal Euclidean distance all generated points must keep to each other.</param>
    /// <param name="halfWidth">Half-width of the stretched hexagon (default 1).</param>
    /// <param name="halfHeight">Half-height of the stretched hexagon (default 0.85).</param>
    /// <param name="seed">Optional random seed for reproducible point generation.</param>
    /// <returns>A dictionary keyed by circle number (0 = center) mapping to generated points for each circle.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if input values are outside valid ranges.</exception>
    public static Dictionary<int, List<Vec2D>> GenerateCircularPoints(int numberOfCircles, float minDistance, float halfWidth = 1f, float halfHeight = 0.85f, int? seed = null)
    {
        if (numberOfCircles < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfCircles), "numberOfCircles must be greater than or equal to 1.");
        }

        if (minDistance <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(minDistance), "minDistance must be greater than 0.");
        }

        if (halfWidth <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(halfWidth), "halfWidth must be greater than 0.");
        }

        if (halfHeight <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(halfHeight), "halfHeight must be greater than 0.");
        }
        
        var result = new Dictionary<int, List<Vec2D>>(1 + numberOfCircles);
        var allPoints = new List<Vec2D>(64);
        var rand = seed.HasValue ? new Random(seed.Value) : new Random();

        var center = new Vec2D(0, 0);
        result[0] = [center];
        allPoints.Add(center);

        float maxRadius = (float)(Math.Sqrt(3) / 2.0);
        float maxUsableRadius = maxRadius * 0.8f;
        float ringStep = maxUsableRadius / numberOfCircles;

        for (int circle = 1; circle <= numberOfCircles; circle++)
        {
            float centerRadius = circle * ringStep;
            float innerRadius = circle == 1 ? 0f : centerRadius - ringStep * 0.5f;
            float outerRadius = circle == numberOfCircles ? maxUsableRadius : centerRadius + ringStep * 0.5f;

            float averageScale = (halfWidth + halfHeight) * 0.5f;
            float effectiveRadius = centerRadius * averageScale;
            int estimatedCount = (int)MathF.Ceiling((2f * MathF.PI * MathF.Max(effectiveRadius, minDistance)) / minDistance);
            int targetCount = Math.Max(1, estimatedCount);

            int maxAttempts = Math.Max(200, targetCount * 80);
            int attempts = 0;

            var circlePoints = new List<Vec2D>(targetCount);

            while (circlePoints.Count < targetCount && attempts < maxAttempts)
            {
                attempts++;

                float angle = (float)(rand.NextDouble() * 2.0 * Math.PI);
                float radius = SampleRadiusInAnnulus(innerRadius, outerRadius, rand);

                float x = radius * (float)Math.Cos(angle) * halfWidth;
                float y = radius * (float)Math.Sin(angle) * halfHeight;
                var candidate = new Vec2D(x, y);

                if (IsFarEnough(candidate, allPoints, minDistance))
                {
                    circlePoints.Add(candidate);
                    allPoints.Add(candidate);
                }
            }

            result[circle] = circlePoints;
        }

        return result;
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

    private static float SampleRadiusInAnnulus(float innerRadius, float outerRadius, Random rand)
    {
        if (outerRadius <= innerRadius)
            return innerRadius;

        float innerSquared = innerRadius * innerRadius;
        float outerSquared = outerRadius * outerRadius;
        float t = (float)rand.NextDouble();
        return MathF.Sqrt(innerSquared + t * (outerSquared - innerSquared));
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
