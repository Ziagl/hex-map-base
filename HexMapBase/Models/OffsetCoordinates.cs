using com.hexagonsimulations.Geometry.Hex.Enums;

namespace com.hexagonsimulations.Geometry.Hex
{
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
    public struct OffsetCoordinates
    {
        public int x;
        public int y;

        /// <summary>
        /// Return whether or not this hex belongs to an odd-numbered row.
        /// </summary>
        public bool IsOddRow
        {
            get { return (this.RowParity == Parity.Odd); }
        }

        /// <summary>
        /// Return the row parity of the hex (whether its row number is even or odd).
        /// </summary>
        public Parity RowParity
        {
            get { return (Parity)(this.y & 1); }
        }

        /// <summary>
        /// Create a new OffsetHexCoord given the coordinates q and r.
        /// </summary>
        /// <param name="x">The column position of the hex within the grid.</param>
        /// <param name="y">The row position of the hex within the grid.</param>
        public OffsetCoordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Return this hex as a CubeCoordinate.
        /// </summary>
        /// <returns>A CubeCoordinate representing the hex.</returns>
        public CubeCoordinates ToCubic()
        {
            // Made a scary change here. Expect this to break!
            int q = this.x - (this.y - (int)RowParity) / 2;
            int r = this.y;
            int s = -q - r;

            return new CubeCoordinates(q, r, s);
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
        /// Check if this OffsetCoordinate is equal to an arbitrary object.
        /// </summary>
        /// <returns>Whether or not this OffsetCoordinate and the given object are equal.</returns>
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

            OffsetCoordinates other = (OffsetCoordinates)obj;

            return (this.x == other.x) && (this.y == other.y);
        }
    }
}
