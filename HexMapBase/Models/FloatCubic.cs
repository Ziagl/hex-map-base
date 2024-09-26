using System;

namespace com.hexagonsimulations.Geometry.Hex
{
    /// <summary>
    /// FloatCubic represents a pseudo-position on the hex grid. It does not directly represent
    /// the position of a hex, but instead is used as a means to compute a hex position by rounding
    /// a FloatCubic using CubeCoordinate.Round(), which returns a CubeCoordinate.
    /// </summary>
    public struct FloatCubic
    {
        public float q;
        public float r;
        public float s;

        /// <summary>
        /// Create a new FloatCubic given a CubeCoordinateIndex.
        /// </summary>
        /// <param name="cubic">Any CubeCoordinate representing a hex.</param>
        public FloatCubic(CubeCoordinates cubic)
        {
            this.q = (float)cubic.q;
            this.r = (float)cubic.r;
            this.s = (float)cubic.s;
        }

        /// <summary>
        /// Create a new FloatCubic given the coordinates q, r and s.
        /// </summary>
        /// <param name="q">The position on this point on the q-axis.</param>
        /// <param name="r">The position on this point on the r-axis.</param>
        /// <param name="s">The position on this point on the s-axis.</param>
        public FloatCubic(float q, float r, float s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        /// <summary>
        /// Return this FloatCubic as a FloatAxial.
        /// </summary>
        /// <returns>A FloatAxial representing this FloatCubic.</returns>
        public FloatAxial ToFloatAxial()
        {
            return new FloatAxial(this.q, this.s);
        }

        /// <summary>
        /// Returns a new CubeCoordinate representing the nearest hex to this FloatCubic.
        /// </summary>
        /// <returns>A new CubeCoordinate representing the nearest hex to this FloatCubic.</returns>
        public CubeCoordinates Round()
        {
            int rx = (int)Math.Round(this.q);
            int ry = (int)Math.Round(this.r);
            int rz = (int)Math.Round(this.s);

            float xDiff = Math.Abs(rx - this.q);
            float yDiff = Math.Abs(ry - this.r);
            float zDiff = Math.Abs(rz - this.s);

            if (xDiff > yDiff && xDiff > zDiff)
            {
                rx = -ry - rz;
            }
            else if (yDiff > zDiff)
            {
                ry = -rx - rz;
            }
            else
            {
                rz = -rx - ry;
            }

            return new CubeCoordinates(rx, ry, rz);
        }

        /// <summary>
        /// Scale the world space by the given factor, causing q and r to change proportionally
        /// to factor.
        /// </summary>
        /// <param name="factor">The multiplicative factor by which the world space is being
        /// scaled.</param>
        /// <returns>A new FloatCubic representing the new floating hex position.</returns>
        public FloatCubic Scale(float factor)
        {
            return new FloatCubic(this.q * factor, this.r * factor, this.s * factor);
        }
    }
}
