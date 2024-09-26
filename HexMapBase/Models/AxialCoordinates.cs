namespace com.hexagonsimulations.Geometry.Hex
{
    /// <summary>
    /// Represents the position of a hex within a hex grid using axial coordinates (q = East,
    /// r = Southeast). This results in a grid with a naturally parallelgraphic form, but can
    /// still be stored in a regtangular array resulting in a small number of wasted cells.
    /// </summary>
    /// <remarks>This type is the less computationally efficient type to use for hex grid
    /// computations than CubeCoordinate, as all of the work has to be done by the CubeCoordinate
    /// type and converting between AxialCoordinale and CubeCoordinate is a small cost every time
    /// a result must be obtained.</remarks>
    public struct AxialCoordinates
    {
        public int q;
        public int r;

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

        /// <summary>
        /// Check if this AxialCoordinate is equal to an arbitrary object.
        /// </summary>
        /// <returns>Whether or not this AxialCoordinate and the given object are equal.</returns>
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

            AxialCoordinates other = (AxialCoordinates)obj;

            return (this.q == other.q) && (this.r == other.r);
        }
    }
}
