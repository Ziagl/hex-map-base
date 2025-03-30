using System;

namespace com.hexagonsimulations.HexMapBase.Models;

/// <summary>
/// A simple 2D vector implementation to store coordinates on the x-y cartesian plane.
/// This doesn't really do anything important. It's just a container used by HexGrid to
/// return world position values pack to the caller.
/// </summary>
public struct Vec2D
{
    public float x;
    public float y;

    /// <summary>
    /// Create a new Vec2D given the coordinates x and z (on the world x-y plane).
    /// </summary>
    /// <param name="x">The position of this point on the x-axis.</param>
    /// <param name="y">The position of this point on the y-axis.</param>
    public Vec2D(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Add 2 Vec2Ds together and return the result.
    /// </summary>
    /// <param name="lhs">The Vec2D on the left-hand side of the + sign.</param>
    /// <param name="rhs">The Vec2D on the right-hand side of the + sign.</param>
    /// <returns>A new Vec2D representing the sum of the inputs.</returns>
    public static Vec2D operator +(Vec2D lhs, Vec2D rhs)
    {
        float x = lhs.x + rhs.x;
        float y = lhs.y + rhs.y;

        return new Vec2D(x, y);
    }

    /// <summary>
    /// Subtract 1 Vec2D from another and return the result.
    /// </summary>
    /// <param name="lhs">The Vec2D on the left-hand side of the - sign.</param>
    /// <param name="rhs">The Vec2D on the right-hand side of the - sign.</param>
    /// <returns>A new Vec2D representing the difference of the inputs.</returns>
    public static Vec2D operator -(Vec2D lhs, Vec2D rhs)
    {
        float x = lhs.x - rhs.x;
        float y = lhs.y - rhs.y;

        return new Vec2D(x, y);
    }

    /// <summary>
    /// Multiply a Vec2D by a scalar value.
    /// </summary>
    /// <param name="lhs">The Vec2D to be multiplied.</param>
    /// <param name="scalar">The scalar value to multiply the Vec2D by.</param>
    /// <returns>A new Vec2D.</returns>
    public static Vec2D operator *(Vec2D lhs, float scalar)
    {
        return new Vec2D(lhs.x * scalar, lhs.y * scalar);
    }

    /// <summary>
    /// Compare 2 Vec2D objects if they are equal.
    /// </summary>
    /// <param name="lhs">The first Vec2D object.</param>
    /// <param name="rhs">The second Vec2D object.</param>
    /// <returns>True if all values are equal otherwise false.</returns>
    public static bool operator ==(Vec2D lhs, Vec2D rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    /// <summary>
    /// Compare 2 Vec2D objects if they are not equl
    /// </summary>
    /// <param name="lhs">The first Vec2D object.</param>
    /// <param name="rhs">The second Vec2D object.</param>
    /// <returns>True if any value is different, otherwise false.</returns>
    public static bool operator !=(Vec2D lhs, Vec2D rhs)
    {
        return !(lhs == rhs);
    }

    /// <summary>
    /// Rotate a Vec2D by a given angle.
    /// </summary>
    /// <param name="lhs">A Vec2D object to rotate.</param>
    /// <param name="angle">An angle to rotate clockwise (in degrees).</param>
    /// <returns></returns>
    public static Vec2D Rotate(Vec2D lhs, float angle)
    {
        double angleInRadians = angle * Math.PI / 180.0;

        float x = lhs.x * (float)Math.Cos(angleInRadians) - lhs.y * (float)Math.Sin(angleInRadians);
        float y = lhs.x * (float)Math.Sin(angleInRadians) + lhs.y * (float)Math.Cos(angleInRadians);
        return new Vec2D(x, y);
    }

    /// <summary>
    /// Normalize a Vec2D.
    /// </summary>
    /// <param name="lhs">Vec2D object to be normalized.</param>
    /// <returns>A normalized Vec2D or the same Vec2D if length <= 0.</returns>
    public static Vec2D Normalize(Vec2D lhs)
    {
        double length = Math.Sqrt(lhs.x * lhs.x + lhs.y * lhs.y);
        if(length > 0.0)
        {
            double normalizedX = lhs.x / length;
            double normalizedY = lhs.y / length;

            return new Vec2D((float)normalizedX, (float)normalizedY);
        }

        return lhs;
    }

    /// <summary>
    /// Returns the dot product of two Vec2Ds.
    /// </summary>
    /// <param name="lhs">First Vec2D object.</param>
    /// <param name="rhs">Second Vec2D object.</param>
    /// <returns>Vec2D of the dot product.</returns>
    public static double DotProduct(Vec2D lhs, Vec2D rhs)
    {
        return (lhs.x * rhs.x) + (lhs.y * rhs.y);
    }

    /// <summary>
    /// Returns the distance from this Vec2D to the given other.
    /// </summary>
    /// <param name="other">Any other Vec2D.</param>
    /// <returns>A float representing the distance from this Vec2D to the given other.
    /// </returns>
    public float Distance(Vec2D other)
    {
        return (other - this).Length();
    }

    /// <summary>
    /// Returns float representing the length of this Vec2D.
    /// </summary>
    /// <returns>A float representing the length of this Vec2D.</returns>
    public float Length()
    {
        return (float)Math.Sqrt(x * x + y * y);
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Vec2D))
        {
            return false;
        }

        Vec2D other = (Vec2D)obj;
        return this == other;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode();
    }
}
