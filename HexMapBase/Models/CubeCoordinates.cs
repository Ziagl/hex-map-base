using com.hexagonsimulations.HexMapBase.Enums;
using System;

namespace com.hexagonsimulations.HexMapBase.Models;

/// <summary>
/// Represents the position of a hex within a hex grid using cube-space coordinates. This is a
/// bit weird, but it is comparable to representing 3D rotations with quaternions (in 4-space).
/// This coordinate system is a bit less memory-efficient but enables simple algorithms for
/// most hex grid operations. Because of this, the CubeCoordinate type contains basically all of
/// the methods for operating on hexes. That being said, this data format is non-ideal for
/// storage or end-use, so it is advised that the user transform their data into another
/// coordinate system for use in most applications or storage in memory/to disk.
/// </summary>
/// <remarks>It is advisable to keep your hex position data structures as CubeCoordinate type
/// whenever possible. Since most of the computational work is done in cube-space, this type
/// is the most efficient to use, but is the least practical for end-user grid
/// implementations. Simply keep a CubeCoordinate to do the work and allow it to return results
/// that you can convert to other types.</remarks>
public struct CubeCoordinates
{
    public int q;
    public int r;
    public int s;

    private static CubeCoordinates GetDirection(Direction direction)
    {
        switch(direction)
        {
            case Direction.E:
                return new CubeCoordinates(1, 0, -1);
            case Direction.SE:
                return new CubeCoordinates(0, 1, -1);
            case Direction.SW:
                return new CubeCoordinates(-1, 1, 0);
            case Direction.W:
                return new CubeCoordinates(-1, 0, 1);
            case Direction.NW:
                return new CubeCoordinates(0, -1, 1);
            case Direction.NE:
                return new CubeCoordinates(1, -1, 0);
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }

    private static CubeCoordinates GetDiagonal(Diagonal diagonal)
    {
        switch(diagonal)
        {
            case Enums.Diagonal.ESE:
                return new CubeCoordinates(1, 1, -2);
            case Enums.Diagonal.S:
                return new CubeCoordinates(-1, 2, -1);
            case Enums.Diagonal.WSW:
                return new CubeCoordinates(-2, 1, 1);
            case Enums.Diagonal.WNW:
                return new CubeCoordinates(-1, -1, 2);
            case Enums.Diagonal.N:
                return new CubeCoordinates(1, -2, 1);
            case Enums.Diagonal.ENE:
                return new CubeCoordinates(2, -1, -1);
            default:
                throw new ArgumentOutOfRangeException("diagonal", diagonal, null);
        }
    }

    /// <summary>
    /// Creates new CubeCoordinaes given the coordinates q, r and s.
    /// </summary>
    /// <param name="q">The position on the q-axis in cube-space.</param>
    /// <param name="r">The position on the r-axis in cube-space.</param>
    /// <param name="s">The position on the s-axis in cube-space.</param>
    public CubeCoordinates(int q, int r, int s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
    }

    /// <summary>
    /// Creates new CubeCoordinates given the coordinates q, and r.
    /// </summary>
    /// <param name="q">The position on the q-axis in cube-space.</param>
    /// <param name="r">The position on the r-axis in cube-space.</param>
    public CubeCoordinates(int q, int r)
    {
        this.q = q;
        this.r = r;
        this.s = q - r;
    }

    /// <summary>
    /// Return this hex as an AxialCoordinates.
    /// </summary>
    /// <returns>An AxialCoordinates representing the hex.</returns>
    public AxialCoordinates ToAxial()
    {
        return new AxialCoordinates(this.q, this.s);
    }

    /// <summary>
    /// Return this hex as an OffsetCoordinates.
    /// </summary>
    /// <returns>An OffsetCoordinates representing the hex.</returns>
    public OffsetCoordinates ToOffset()
    {
        int x = this.q + (this.r - (this.r & 1)) / 2;
        int y = this.r;

        return new OffsetCoordinates(x, y);
    }

    /// <summary>
    /// Add 2 CubeCoordinates together and return the result.
    /// </summary>
    /// <param name="lhs">The CubeCoordinates on the left-hand side of the + sign.</param>
    /// <param name="rhs">The CubeCoordinates on the right-hand side of the + sign.</param>
    /// <returns>A new CubeCoordinates representing the sum of the inputs.</returns>
    public static CubeCoordinates operator +(CubeCoordinates lhs, CubeCoordinates rhs)
    {
        int q = lhs.q + rhs.q;
        int r = lhs.r + rhs.r;
        int s = lhs.s + rhs.s;

        return new CubeCoordinates(q, r, s);
    }

    /// <summary>
    /// Subtract 1 CubeCoordinates from another and return the result.
    /// </summary>
    /// <param name="lhs">The CubeCoordinates on the left-hand side of the - sign.</param>
    /// <param name="rhs">The CubeCoordinates on the right-hand side of the - sign.</param>
    /// <returns>A new CubeCoordinates representing the difference of the inputs.</returns>
    public static CubeCoordinates operator -(CubeCoordinates lhs, CubeCoordinates rhs)
    {
        int q = lhs.q - rhs.q;
        int r = lhs.r - rhs.r;
        int s = lhs.s - rhs.s;

        return new CubeCoordinates(q, r, s);
    }

    /// <summary>
    /// Check if 2 CubeCoordinates represent the same hex on the grid.
    /// </summary>
    /// <param name="lhs">The CubeCoordinates on the left-hand side of the == sign.</param>
    /// <param name="rhs">The CubeCoordinates on the right-hand side of the == sign.</param>
    /// <returns>A bool representing whether or not the CubeCoordinates are equal.</returns>
    public static bool operator ==(CubeCoordinates lhs, CubeCoordinates rhs)
    {
        return (lhs.q == rhs.q) && (lhs.r == rhs.r) && (lhs.s == rhs.s);
    }

    /// <summary>
    /// Check if 2 CubeCoordinates represent the different hexes on the grid.
    /// </summary>
    /// <param name="lhs">The CubeCoordinate on the left-hand side of the != sign.</param>
    /// <param name="rhs">The CubeCoordinate on the right-hand side of the != sign.</param>
    /// <returns>A bool representing whether or not the CubeCoordinate are unequal.</returns>
    public static bool operator !=(CubeCoordinates lhs, CubeCoordinates rhs)
    {
        return (lhs.q != rhs.q) || (lhs.r != rhs.r) || (lhs.s != rhs.s);
    }

    /// <summary>
    /// Get a hash reflecting the contents of the CubeCoordinates.
    /// </summary>
    /// <returns>An integer hash code reflecting the contents of the CubeCoordinates.</returns>
    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + q.GetHashCode();
        hash = hash * 23 + r.GetHashCode();
        hash = hash * 23 + s.GetHashCode();
        return hash;
    }

    /// <summary>
    /// Check if this CubeCoordinate is equal to an arbitrary object.
    /// </summary>
    /// <returns>Whether or not this CubeCoordinate and the given object are equal.</returns>
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        CubeCoordinates other = (CubeCoordinates)obj;

        return (this.q == other.q) && (this.r == other.r) && (this.s == other.s);
    }

    /// <summary>
    /// Returns an array of CubeCoordinates within the given range from this hex (including
    /// this hex) in no particular order.
    /// </summary>
    /// <param name="range">The maximum number of grid steps away from this hex that
    /// CubeCoordinate will be returned for.</param>
    /// <returns>An array of CubeCoordinates within the given range from this hex (including
    /// this hex) in no particular order.</returns>
    public CubeCoordinates[] AreaAround(int range)
    {
        return CubeCoordinates.Area(this, range);
    }

    /// <summary>
    /// Returns a CubeCoordinates representing the diagonal of this hex in the given diagonal
    /// direction.
    /// </summary>
    /// <param name="diagonal">The diagonal direction of the requested neighbor.</param>
    /// <returns>A CubeCoordinate representing the diagonal of this hex in the given diagonal
    /// direction.</returns>
    public CubeCoordinates Diagonal(Diagonal diagonal)
    {
        return this + GetDiagonal(diagonal);
    }

    /// <summary>
    /// Returns an array of CubeCoordinates representing this hex's diagonals (in clockwise
    /// order).
    /// </summary>
    /// <returns>An array of CubeCoordinates representing this hex's diagonals (in clockwise
    /// order).</returns>
    public CubeCoordinates[] Diagonals()
    {
        return new CubeCoordinates[6]
        {
            this + GetDiagonal(Enums.Diagonal.ESE),
            this + GetDiagonal(Enums.Diagonal.S),
            this + GetDiagonal(Enums.Diagonal.WSW),
            this + GetDiagonal(Enums.Diagonal.WNW),
            this + GetDiagonal(Enums.Diagonal.N),
            this + GetDiagonal(Enums.Diagonal.ENE),
        };
    }

    /// <summary>
    /// Returns the minimum number of grid steps to get from this hex to the given hex.
    /// </summary>
    /// <param name="other">Any CubeCoordinate.</param>
    /// <returns>An integer number of grid steps from this hex to the given hex.</returns>
    public int DistanceTo(CubeCoordinates other)
    {
        return CubeCoordinates.Distance(this, other);
    }

    /// <summary>
    /// Returns an array of CubeCoordinates that form the straightest path from this hex to the
    /// given end. The hexes in the line are determined by forming a straight line from start
    /// to end and linearly interpolating and rounding each of the interpolated points to
    /// the nearest hex position.
    /// </summary>
    /// <param name="other">The CubeCoordinate representing the last hex in the line.</param>
    /// <returns>An array of CubeCoordinates ordered as a line from start to end.</returns>
    public CubeCoordinates[] LineTo(CubeCoordinates other)
    {
        return CubeCoordinates.Line(this, other);
    }

    /// <summary>
    /// Returns a CubeCoordinate representing the neighbor of this hex in the given direction.
    /// </summary>
    /// <param name="direction">The direction of the requested neighbor.</param>
    /// <returns>A CubeCoordinate representing the neighbor of this hex in the given direction.
    /// </returns>
    public CubeCoordinates Neighbor(Direction direction)
    {
        return this + GetDirection(direction);
    }

    /// <summary>
    /// Returns an array of CubeCoordinates representing this hex's neighbors (in clockwise
    /// order).
    /// </summary>
    /// <returns>An array of CubeCoordinates representing this hex's neighbors (in clockwise
    /// order).</returns>
    public CubeCoordinates[] Neighbors()
    {
        return new CubeCoordinates[6]
        {
            this + GetDirection(Direction.E),
            this + GetDirection(Direction.SE),
            this + GetDirection(Direction.SW),
            this + GetDirection(Direction.W),
            this + GetDirection(Direction.NW),
            this + GetDirection(Direction.NE),
        };
    }

    /// <summary>
    /// Returns an array of CubeCoordinates that appear at the given range around this hex.
    /// The ring begins from the CubeCoordinate range grid steps away from the center, heading
    /// in the given direction, and encircling the center in clockwise order.
    /// </summary>
    /// <param name="range">The number of grid steps distance away from the center that the
    /// ring will be.</param>
    /// <param name="startDirection">The direction in which the first CubeCoordinate of the
    /// ring will appear in.</param>
    /// <returns>An array of CubeCoordinates ordered as a ring.</returns>
    public CubeCoordinates[] RingAround(int range, Direction startDirection = Direction.E)
    {
        return CubeCoordinates.Ring(this, range, startDirection);
    }

    /// <remarks>THIS IS NOT YET IMPLEMENTED!</remarks>
    /// <summary>
    /// Rotate this CubeCoordinate around the given center by the given rotation (constrained to
    /// exact 60 degree rotation steps) and return the CubeCoordinate that represents the
    /// rotated position in the grid.
    /// </summary>
    /// <param name="center">The CubeCoordinate to be rotated around this CubeCoordinate.</param>
    /// <param name="rotation">The direction and magnitude of rotation to be applied (in exact
    /// 60 degree rotation steps.</param>
    /// <returns>A CubeCoordinate representing the position of the rotated hex on the grid.
    /// </returns>
    public CubeCoordinates RotateAroundOther(CubeCoordinates center, Rotation rotation)
    {
        return CubeCoordinates.Rotate(center, this, rotation);
    }

    /// <remarks>THIS IS NOT YET IMPLEMENTED!</remarks>
    /// <summary>
    /// Rotate a CubeCoordinate around this CubeCoordinate by the given rotation (constrained to
    /// exact 60 degree rotation steps) and return the CubeCoordinate that represents the
    /// rotated position in the grid.
    /// </summary>
    /// <param name="toRotate">The CubeCoordinate to be rotated around this CubeCoordinate.</param>
    /// <param name="rotation">The direction and magnitude of rotation to be applied (in exact
    /// 60 degree rotation steps.</param>
    /// <returns>A CubeCoordinate representing the position of the rotated hex on the grid.
    /// </returns>
    public CubeCoordinates RotateOtherAround(CubeCoordinates toRotate, Rotation rotation)
    {
        return CubeCoordinates.Rotate(this, toRotate, rotation);
    }

    /// <summary>
    /// Scale this CubeCoordinate by the given factor, such that the x, y and z values of
    /// the CubeCoordinate change proprtionally to the factor provided.
    /// </summary>
    /// <param name="factor">A multiplicative factor to scale by.</param>
    /// <returns>A new scaled CubeCoordinate.</returns>
    public CubeCoordinates Scale(float factor)
    {
        return new FloatCubic(this).Scale(factor).Round();
    }

    /// <summary>
    /// Returns an array of CubeCoordinates of a area centering around ths hex and extending in
    /// every direction up to the given range. The hexes are ordered starting from the center
    /// and then spiraling outward clockwise beginning from the neighbor in the given
    /// direction, until the outside ring is complete.
    /// </summary>
    /// <param name="range">The number of grid steps distance away from the center that the
    /// edge of the spiral will be.</param>
    /// <param name="startDirection">The direction in which the first CubeCoordinate of the
    /// spiral will appear in.</param>
    /// <returns>An array of CubeCoordinates ordered as a spiral, beginning from the center
    /// and proceeding clockwise until it reaches the outside of the spiral.</returns>
    public CubeCoordinates[] SpiralAroundInward(
        int range,
        Direction startDirection = Direction.E
    )
    {
        return CubeCoordinates.SpiralInward(this, range, startDirection);
    }

    /// <summary>
    /// Returns an array of CubeCoordinates of a area centering around this hex and extending in
    /// every direction up to the given range. The hexes are ordered starting from the maximum
    /// range, in the given direction, spiraling inward in a clockwise direction until the
    /// center is reached (and is the last element in the array).
    /// </summary>
    /// <param name="range">The number of grid steps distance away from the center that the
    /// edge of the spiral will be.</param>
    /// <param name="startDirection">The direction in which the first CubeCoordinate of the
    /// spiral will appear in.</param>
    /// <returns>An array of CubeCoordinates ordered as a spiral, beginning from the outside
    /// and proceeding clockwise until it reaches the center of the spiral.</returns>
    /// <returns></returns>
    public CubeCoordinates[] SpiralAroundOutward(
        int range,
        Direction startDirection = Direction.E
    )
    {
        return CubeCoordinates.SpiralOutward(this, range, startDirection);
    }

    /// <summary>
    /// Returns an array of CubeCoordinates within the given range from the given center
    /// (including the center itself) in no particular order.
    /// </summary>
    /// <param name="center">The CubeCoordinate around which the area is formed.</param>
    /// <param name="range">The maximum number of grid steps away from the center that
    /// CubeCoordinates will be returned for.</param>
    /// <returns>An array of CubeCoordinates within the given range from the given center
    /// (including the center itself) in no particular order.</returns>
    public static CubeCoordinates[] Area(CubeCoordinates center, int range)
    {
        if (range < 0)
        {
            throw new ArgumentOutOfRangeException(
                "range must be a non-negative integer value."
            );
        }
        else if (range == 0)
        {
            return new CubeCoordinates[1] { center };
        }

        int arraySize = 1;
        for (int i = range; i > 0; i--)
        {
            arraySize += 6 * i;
        }

        CubeCoordinates[] result = new CubeCoordinates[arraySize];

        for (int i = 0, dx = -range; dx <= range; dx++)
        {
            int dyMinBound = Math.Max(-range, -dx - range);
            int dyMaxBound = Math.Min(range, -dx + range);

            for (int dy = dyMinBound; dy <= dyMaxBound; dy++)
            {
                int dz = -dx - dy;
                result[i++] = center + new CubeCoordinates(dx, dy, dz);
            }
        }

        return result;
    }

    /// <summary>
    /// Returns a CubeCoordinate representing the diff between some hex and its diagonal in
    /// the given diagonal direction.
    /// </summary>
    /// <param name="direction">The diagonal direction to return a diff for.</param>
    /// <returns>A CubeCoordinate representing the diff between some hex and its diagonal in
    /// the given diagonal direction.</returns>
    public static CubeCoordinates DiagonalDiff(Diagonal diagonal)
    {
        return GetDiagonal(diagonal);
    }

    /// <summary>
    /// Returns a CubeCoordinate representing the diff between some hex and its neighbor in
    /// the given direction.
    /// </summary>
    /// <param name="direction">The direction to return a diff for.</param>
    /// <returns>A CubeCoordinate representing the diff between some hex and its neighbor in
    /// the given direction.</returns>
    public static CubeCoordinates DirectionDiff(Direction direction)
    {
        return GetDirection(direction);
    }

    /// <remarks>THIS IS NOT YET IMPLEMENTED!</remarks>
    /// <see cref="http://www.redblobgames.com/grids/hexagons/"/>
    /// <summary>
    /// Returns an array of CubeCoordinates that represents the hexes belonging to both a and b.
    /// </summary>
    /// <param name="a">An array of CubeCoordinates representing an area or spiral of hexes.</param>
    /// <param name="b">An array of CubeCoordinates representing an area or spiral of hexes.</param>
    /// <returns>An array of CubeCoordinates that represents the hexes belonging to both a and b.</returns>
    public static CubeCoordinates[] IntersectRanges(CubeCoordinates[] a, CubeCoordinates[] b)
    {
        throw new NotImplementedException("Feature not suppored yet!");
    }

    /// <summary>
    /// Returns the minimum number of grid steps to get from a to b.
    /// </summary>
    /// <param name="a">Any CubeCoordinate.</param>
    /// <param name="b">Any CubeCoordinate.</param>
    /// <returns>An integer number of grid steps from a to b.</returns>
    public static int Distance(CubeCoordinates a, CubeCoordinates b)
    {
        int dx = Math.Abs(a.q - b.q);
        int dy = Math.Abs(a.r - b.r);
        int dz = Math.Abs(a.s - b.s);

        return Math.Max(Math.Max(dx, dy), dz);
    }

    /// <summary>
    /// Returns an array of CubeCoordinates that form the straightest path from the given start
    /// to the given end. The hexes in the line are determined by forming a straight line from
    /// start to end and linearly interpolating and rounding each of the interpolated points to
    /// the nearest hex position.
    /// </summary>
    /// <param name="start">The CubeCoordinate representing the first hex in the line.</param>
    /// <param name="end">The CubeCoordinate representing the last hex in the line.</param>
    /// <returns>An array of CubeCoordinates ordered as a line from start to end.</returns>
    public static CubeCoordinates[] Line(CubeCoordinates start, CubeCoordinates end)
    {
        int distance = CubeCoordinates.Distance(start, end);

        CubeCoordinates[] result = new CubeCoordinates[distance + 1];

        for (int i = 0; i <= distance; i++)
        {
            float xLerp = start.q + (end.q - start.q) * 1f / distance * i;
            float yLerp = start.r + (end.r - start.r) * 1f / distance * i;
            float zLerp = start.s + (end.s - start.s) * 1f / distance * i;

            result[i] = new FloatCubic(xLerp, yLerp, zLerp).Round();
        }

        return result;
    }

    /// <summary>
    /// Returns an array of CubeCoordinates that appear at the given range around the given
    /// center hex. The ring begins from the CubeCoordinate range grid steps away from the
    /// center, heading in the given direction, and encircling the center in clockwise order.
    /// </summary>
    /// <param name="center">The CubeCoordinate around which the ring is formed.</param>
    /// <param name="range">The number of grid steps distance away from the center that the
    /// ring will be.</param>
    /// <param name="startDirection">The direction in which the first CubeCoordinate of the
    /// ring will appear in.</param>
    /// <returns>An array of CubeCoordinates ordered as a ring.</returns>
    public static CubeCoordinates[] Ring(
        CubeCoordinates center,
        int range,
        Direction startDirection = Direction.E
    )
    {
        if (range <= 0)
        {
            throw new ArgumentOutOfRangeException("range must be a positive integer value.");
        }

        CubeCoordinates[] result = new CubeCoordinates[6 * range];

        CubeCoordinates cube = center + GetDirection(startDirection).Scale(range);

        int[] directions = new int[6];
        for (int i = 0; i < 6; i++)
        {
            directions[i] = ((int)startDirection + i) % 6;
        }

        int index = 0;
        for (int i = 0; i < 6; i++)
        {
            int neighborDirection = (directions[i] + 2) % 6;
            for (int j = 0; j < range; j++)
            {
                result[index++] = cube;
                cube = cube.Neighbor((Direction)neighborDirection);
            }
        }

        return result;
    }

    /// <remarks>THIS IS NOT YET IMPLEMENTED!</remarks>
    /// <see cref="http://www.redblobgames.com/grids/hexagons/"/>
    /// <summary>
    /// Rotate a CubeCoordinate around the given center by the given rotation (constrained to
    /// exact 60 degree rotation steps) and return the CubeCoordinate that represents the
    /// rotated position in the grid.
    /// </summary>
    /// <param name="center">The CubeCoordinate representing the rotation's pivot.</param>
    /// <param name="toRotate">The CubeCoordinate to be rotated around the pivot.</param>
    /// <param name="rotation">The direction and magnitude of rotation to be applied (in exact
    /// 60 degree rotation steps.</param>
    /// <returns>A CubeCoordinate representing the position of the rotated hex on the grid.</returns>
    public static CubeCoordinates Rotate(
        CubeCoordinates center,
        CubeCoordinates toRotate,
        Rotation rotation
    )
    {
        throw new NotImplementedException("Feature not suppored yet!");
    }

    /// <summary>
    /// Returns an array of CubeCoordinates of a area centering around the given center hex and
    /// extending in every direction up to the given range. The hexes are ordered starting from
    /// the center and then spiraling outward clockwise beginning from the neighbor in the
    /// given direction, until the outside ring is complete.
    /// </summary>
    /// <param name="center">The CubeCoordinate around which the spiral is formed.</param>
    /// <param name="range">The number of grid steps distance away from the center that the
    /// edge of the spiral will be.</param>
    /// <param name="startDirection">The direction in which the first CubeCoordinate of the
    /// spiral will appear in.</param>
    /// <returns>An array of CubeCoordinates ordered as a spiral, beginning from the center
    /// and proceeding clockwise until it reaches the outside of the spiral.</returns>
    public static CubeCoordinates[] SpiralInward(
        CubeCoordinates center,
        int range,
        Direction startDirection = Direction.E
    )
    {
        if (range <= 0)
        {
            throw new ArgumentOutOfRangeException("range must be a positive integer value.");
        }
        else if (range == 0)
        {
            return new CubeCoordinates[1] { center };
        }

        int arraySize = 1;
        for (int i = range; i > 0; i--)
        {
            arraySize += 6 * i;
        }

        CubeCoordinates[] result = new CubeCoordinates[arraySize];

        result[result.Length - 1] = center;

        int arrayIndex = result.Length - 1;
        for (int i = range; i >= 1; i--)
        {
            CubeCoordinates[] ring = CubeCoordinates.Ring(center, i, startDirection);
            arrayIndex -= ring.Length;
            ring.CopyTo(result, arrayIndex);
        }

        return result;
    }

    /// <summary>
    /// Returns an array of CubeCoordinates of a area centering around the given center hex and
    /// extending in every direction up to the given range. The hexes are ordered starting from
    /// the maximum range, in the given direction, spiraling inward in a clockwise direction
    /// until the center is reached (and is the last element in the array).
    /// </summary>
    /// <param name="center">The CubeCoordinate around which the spiral is formed.</param>
    /// <param name="range">The number of grid steps distance away from the center that the
    /// edge of the spiral will be.</param>
    /// <param name="startDirection">The direction in which the first CubeCoordinate of the
    /// spiral will appear in.</param>
    /// <returns>An array of CubeCoordinates ordered as a spiral, beginning from the outside
    /// and proceeding clockwise until it reaches the center of the spiral.</returns>
    public static CubeCoordinates[] SpiralOutward(
        CubeCoordinates center,
        int range,
        Direction startDirection = Direction.E
    )
    {
        if (range <= 0)
        {
            throw new ArgumentOutOfRangeException("range must be a positive integer value.");
        }
        else if (range == 0)
        {
            return new CubeCoordinates[1] { center };
        }

        int arraySize = 1;
        for (int i = range; i > 0; i--)
        {
            arraySize += 6 * i;
        }

        CubeCoordinates[] result = new CubeCoordinates[arraySize];

        result[0] = center;

        int arrayIndex = 1;
        for (int i = 1; i <= range; i++)
        {
            CubeCoordinates[] ring = CubeCoordinates.Ring(center, i, startDirection);
            ring.CopyTo(result, arrayIndex);
            arrayIndex += ring.Length;
        }

        return result;
    }

    /// <summary>
    /// Returns a string representation of the cube coordinates.
    /// </summary>
    /// <returns>A string in the format "CubeCoordinates(q, r, s)" where q, r, and s are the coordinate values.</returns>
    public override string ToString()
    {
        return $"CubeCoordinates({q}, {r}, {s})";
    }
}
