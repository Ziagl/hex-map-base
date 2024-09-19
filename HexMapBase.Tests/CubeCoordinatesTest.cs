using com.hexagonsimulations.Geometry.Hex;

namespace com.hexagonsimulations.Geometry.HexGridTest
{
    [TestFixture]
	public class CubeCoordinatesTest
	{
		#region Constructors
		
		[Test]
		public void ConstructorXYZ()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 2, 3 );

			Assert.That( cubic.x, Is.EqualTo( 1 ) );
			Assert.That( cubic.y, Is.EqualTo( 2 ) );
			Assert.That( cubic.z, Is.EqualTo( 3 ) );
		}

		[Test]
		public void ConstructorParameterless()
		{
			CubeCoordinates cubic = new CubeCoordinates();

			Assert.That( cubic.x, Is.EqualTo( 0 ) );
			Assert.That( cubic.y, Is.EqualTo( 0 ) );
			Assert.That( cubic.z, Is.EqualTo( 0 ) );
		}

		#endregion


		#region Type Conversions
		
		[Test]
		public void ToAxial()
		{
			AxialCoordinates axial = new CubeCoordinates( 1, 2, 3 ).ToAxial();

			Assert.That( axial.q, Is.EqualTo( 1 ) );
			Assert.That( axial.r, Is.EqualTo( 3 ) );
		}
		
		[Test]
		public void ToOffset()
		{
			OffsetCoordinates offset = new CubeCoordinates( 0, -2, 2 ).ToOffset();

			Assert.That( offset.q, Is.EqualTo( 1 ) );
			Assert.That( offset.r, Is.EqualTo( 2 ) );
		}

		#endregion


		#region Operator Overloads

		[Test]
		public void OperatorOverloadPlus()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 2, 3 ) + new CubeCoordinates( 4, 5, 6 );

			Assert.That( cubic.x, Is.EqualTo( 5 ) );
			Assert.That( cubic.y, Is.EqualTo( 7 ) );
			Assert.That( cubic.z, Is.EqualTo( 9 ) );
		}
		
		[Test]
		public void OperatorOverloadMinus()
		{
			CubeCoordinates cubic = new CubeCoordinates( 6, 5, 4 ) - new CubeCoordinates( 1, 2, 3 );

			Assert.That( cubic.x, Is.EqualTo( 5 ) );
			Assert.That( cubic.y, Is.EqualTo( 3 ) );
			Assert.That( cubic.z, Is.EqualTo( 1 ) );
		}

		[Test]
		public void OperatorOverloadEquals()
		{
			bool isTrue  = new CubeCoordinates( 1, 2, 3 ) == new CubeCoordinates( 1, 2, 3 );
			bool isFalse = new CubeCoordinates( 1, 2, 3 ) == new CubeCoordinates( 4, 5, 6 );

			Assert.That( isTrue,  Is.True  );
			Assert.That( isFalse, Is.False );
		}

		[Test]
		public void OperatorOverloadNotEquals()
		{
			bool isTrue  = new CubeCoordinates( 1, 2, 3 ) != new CubeCoordinates( 4, 5, 6 );
			bool isFalse = new CubeCoordinates( 1, 2, 3 ) != new CubeCoordinates( 1, 2, 3 );

			Assert.That( isTrue,  Is.True  );
			Assert.That( isFalse, Is.False );
		}

		#endregion


		#region Instance Methods
		
		[Test]
		public void AreaAround()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 0, -1 );
			CubeCoordinates[] area = CubeCoordinates.Area( cubic, 2 );

            // Center
            Assert.Contains(new CubeCoordinates(1, 0, -1), area);
			// Distance 1
			Assert.Contains( new CubeCoordinates(  1,  1, -2 ), area );
			Assert.Contains( new CubeCoordinates(  2,  0, -2 ), area );
			Assert.Contains( new CubeCoordinates(  0,  1, -1 ), area );
			Assert.Contains( new CubeCoordinates(  2, -1, -1 ), area );
			Assert.Contains( new CubeCoordinates(  1, -1,  0 ), area );
			Assert.Contains( new CubeCoordinates(  0,  0,  0 ), area );
			// Distance 2
			Assert.Contains( new CubeCoordinates( -1,  2, -1 ), area );
			Assert.Contains( new CubeCoordinates(  0,  2, -2 ), area );
			Assert.Contains( new CubeCoordinates(  1,  2, -3 ), area );
			Assert.Contains( new CubeCoordinates(  2,  1, -3 ), area );
			Assert.Contains( new CubeCoordinates(  3,  0, -3 ), area );
			Assert.Contains( new CubeCoordinates(  3, -1, -2 ), area );
			Assert.Contains( new CubeCoordinates(  3, -2, -1 ), area );
			Assert.Contains( new CubeCoordinates(  2, -2,  0 ), area );
			Assert.Contains( new CubeCoordinates(  1, -2,  1 ), area );
			Assert.Contains( new CubeCoordinates(  0, -1,  1 ), area );
			Assert.Contains( new CubeCoordinates( -1,  0,  1 ), area );
			Assert.Contains( new CubeCoordinates( -1,  1,  0 ), area );
		}

		[Test]
		public void Diagonal()
		{
            CubeCoordinates cubic = new CubeCoordinates( 1, 2, 3 ).Diagonal(Hex.Diagonal.ESE );

			Assert.That( cubic.x, Is.EqualTo( 2 ) );
			Assert.That( cubic.y, Is.EqualTo( 0 ) );
			Assert.That( cubic.z, Is.EqualTo( 4 ) );
		}
		
		[Test]
		public void Diagonals()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 2, 3 );
			CubeCoordinates[] diagonals = cubic.Diagonals();

            Assert.That( diagonals, Is.EquivalentTo( new CubeCoordinates[ 6 ] {
				cubic.Diagonal( Hex.Diagonal.ESE ),
				cubic.Diagonal( Hex.Diagonal.S   ),
				cubic.Diagonal( Hex.Diagonal.WSW ),
				cubic.Diagonal( Hex.Diagonal.WNW ),
				cubic.Diagonal( Hex.Diagonal.N   ),
				cubic.Diagonal( Hex.Diagonal.ENE )
			} ) );
		}
		
		[Test]
		public void DistanceTo()
		{
			CubeCoordinates cubic1 = new CubeCoordinates( 0, 0, 0 );
			CubeCoordinates cubic2 = cubic1.Neighbor( Direction.E ).Neighbor( Direction.SE );
			int distance = cubic1.DistanceTo( cubic2 );

			Assert.That( distance, Is.EqualTo( 2 ) );
		}
		
		[Test]
		public void LineTo()
		{
			CubeCoordinates startCubic = new CubeCoordinates( 1, 0, -1 );
			CubeCoordinates endCubic = new CubeCoordinates( -2, 2, 0 );
			CubeCoordinates[] line = startCubic.LineTo( endCubic );

			Assert.That( line, Is.EquivalentTo( new CubeCoordinates[ 4 ] {
				new CubeCoordinates(  1,  0, -1 ),
				new CubeCoordinates(  0,  1, -1 ),
				new CubeCoordinates( -1,  1,  0 ),
				new CubeCoordinates( -2,  2,  0 )
			} ) );
		}
		
		[Test]
		public void Neighbor()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 2, 3 ).Neighbor( Direction.E );

			Assert.That( cubic.x, Is.EqualTo( 2 ) );
			Assert.That( cubic.y, Is.EqualTo( 1 ) );
			Assert.That( cubic.z, Is.EqualTo( 3 ) );
		}
		
		[Test]
		public void Neighbors()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 2, 3 );
			CubeCoordinates[] neighbors = cubic.Neighbors();

			Assert.That( neighbors, Is.EquivalentTo( new CubeCoordinates[ 6 ] {
				cubic.Neighbor( Direction.E  ),
				cubic.Neighbor( Direction.SE ),
				cubic.Neighbor( Direction.SW ),
				cubic.Neighbor( Direction.W  ),
				cubic.Neighbor( Direction.NW ),
				cubic.Neighbor( Direction.NE )
			} ) );
		}
		
		[Test]
		public void RingAround()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 0, -1 );
			CubeCoordinates[] ring = cubic.RingAround( 2, Direction.W );

			Assert.That( ring, Is.EquivalentTo( new CubeCoordinates[ 12 ] {
				new CubeCoordinates( -1,  2, -1 ),
				new CubeCoordinates(  0,  2, -2 ),
				new CubeCoordinates(  1,  2, -3 ),
				new CubeCoordinates(  2,  1, -3 ),
				new CubeCoordinates(  3,  0, -3 ),
				new CubeCoordinates(  3, -1, -2 ),
				new CubeCoordinates(  3, -2, -1 ),
				new CubeCoordinates(  2, -2,  0 ),
				new CubeCoordinates(  1, -2,  1 ),
				new CubeCoordinates(  0, -1,  1 ),
				new CubeCoordinates( -1,  0,  1 ),
				new CubeCoordinates( -1,  1,  0 )
			} ) );
		}
		
		[Test]
		public void RotateAroundOther()
		{
			// Not yet implemented
			Assert.Ignore();
		}
		
		[Test]
		public void RotateOtherAround()
		{
			// Not yet implemented
			Assert.Ignore();
		}
		
		[Test]
		public void Scale()
		{
			CubeCoordinates cubic = CubeCoordinates.DirectionDiff( Direction.SE );
			CubeCoordinates scaled = cubic.Scale( 3 );

			Assert.That( scaled.x, Is.EqualTo( 3 * cubic.x ) );
			Assert.That( scaled.y, Is.EqualTo( 3 * cubic.y ) );
			Assert.That( scaled.z, Is.EqualTo( 3 * cubic.z ) );
		}
		
		[Test]
		public void SpiralAroundInward()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 0, -1 );
			CubeCoordinates[] spiral = cubic.SpiralAroundOutward( 2, Direction.W );

			Assert.That( spiral, Is.EquivalentTo( new CubeCoordinates[ 19 ] {
				// Distance 2
				new CubeCoordinates( -1,  2, -1 ),
				new CubeCoordinates(  0,  2, -2 ),
				new CubeCoordinates(  1,  2, -3 ),
				new CubeCoordinates(  2,  1, -3 ),
				new CubeCoordinates(  3,  0, -3 ),
				new CubeCoordinates(  3, -1, -2 ),
				new CubeCoordinates(  3, -2, -1 ),
				new CubeCoordinates(  2, -2,  0 ),
				new CubeCoordinates(  1, -2,  1 ),
				new CubeCoordinates(  0, -1,  1 ),
				new CubeCoordinates( -1,  0,  1 ),
				new CubeCoordinates( -1,  1,  0 ),
				// Distance 1
				new CubeCoordinates(  0,  1, -1 ),
				new CubeCoordinates(  1,  1, -2 ),
				new CubeCoordinates(  2,  0, -2 ),
				new CubeCoordinates(  2, -1, -1 ),
				new CubeCoordinates(  1, -1,  0 ),
				new CubeCoordinates(  0,  0,  0 ),
				// Center
				new CubeCoordinates(  1,  0, -1 )
			} ) );
		}
		
		[Test]
		public void SpiralAroundOutward()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 0, -1 );
			CubeCoordinates[] spiral = cubic.SpiralAroundOutward( 2, Direction.W );

			Assert.That( spiral, Is.EquivalentTo( new CubeCoordinates[ 19 ] {
				// Center
				new CubeCoordinates(  1,  0, -1 ),
				// Distance 1
				new CubeCoordinates(  0,  1, -1 ),
				new CubeCoordinates(  1,  1, -2 ),
				new CubeCoordinates(  2,  0, -2 ),
				new CubeCoordinates(  2, -1, -1 ),
				new CubeCoordinates(  1, -1,  0 ),
				new CubeCoordinates(  0,  0,  0 ),
				// Distance 2
				new CubeCoordinates( -1,  2, -1 ), 
				new CubeCoordinates(  0,  2, -2 ),
				new CubeCoordinates(  1,  2, -3 ),
				new CubeCoordinates(  2,  1, -3 ),
				new CubeCoordinates(  3,  0, -3 ),
				new CubeCoordinates(  3, -1, -2 ),
				new CubeCoordinates(  3, -2, -1 ),
				new CubeCoordinates(  2, -2,  0 ),
				new CubeCoordinates(  1, -2,  1 ),
				new CubeCoordinates(  0, -1,  1 ),
				new CubeCoordinates( -1,  0,  1 ),
				new CubeCoordinates( -1,  1,  0 )
			} ) );
		}

		#endregion


		#region Static Methods

		[Test]
		public void StaticArea()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 0, -1 );
			CubeCoordinates[] area = CubeCoordinates.Area( cubic, 2 );

			// Center
			Assert.Contains( new CubeCoordinates(  1,  0, -1 ), area );
			// Distance 1
			Assert.Contains( new CubeCoordinates(  0,  1, -1 ), area );
			Assert.Contains( new CubeCoordinates(  1,  1, -2 ), area );
			Assert.Contains( new CubeCoordinates(  2,  0, -2 ), area );
			Assert.Contains( new CubeCoordinates(  2, -1, -1 ), area );
			Assert.Contains( new CubeCoordinates(  1, -1,  0 ), area );
			Assert.Contains( new CubeCoordinates(  0,  0,  0 ), area );
			// Distance 2
			Assert.Contains( new CubeCoordinates( -1,  2, -1 ), area );
			Assert.Contains( new CubeCoordinates(  0,  2, -2 ), area );
			Assert.Contains( new CubeCoordinates(  1,  2, -3 ), area );
			Assert.Contains( new CubeCoordinates(  2,  1, -3 ), area );
			Assert.Contains( new CubeCoordinates(  3,  0, -3 ), area );
			Assert.Contains( new CubeCoordinates(  3, -1, -2 ), area );
			Assert.Contains( new CubeCoordinates(  3, -2, -1 ), area );
			Assert.Contains( new CubeCoordinates(  2, -2,  0 ), area );
			Assert.Contains( new CubeCoordinates(  1, -2,  1 ), area );
			Assert.Contains( new CubeCoordinates(  0, -1,  1 ), area );
			Assert.Contains( new CubeCoordinates( -1,  0,  1 ), area );
			Assert.Contains( new CubeCoordinates( -1,  1,  0 ), area );
		}

		[Test]
		public void StaticDiagonalDiff()
		{
            CubeCoordinates cubic = CubeCoordinates.DiagonalDiff(Hex.Diagonal.ESE );

			Assert.That( cubic.x, Is.EqualTo(  1 ) );
			Assert.That( cubic.y, Is.EqualTo( -2 ) );
			Assert.That( cubic.z, Is.EqualTo(  1 ) );
		}

		[Test]
		public void StaticDirectionDiff()
		{
			CubeCoordinates cubic = CubeCoordinates.DirectionDiff( Direction.E );

			Assert.That( cubic.x, Is.EqualTo(  1 ) );
			Assert.That( cubic.y, Is.EqualTo( -1 ) );
			Assert.That( cubic.z, Is.EqualTo(  0 ) );
		}

		[Test]
		public void StaticIntersectRanges()
		{
			// Not yet implemented
			Assert.Ignore();
		}

		[Test]
		public void StaticLine()
		{
			CubeCoordinates startCubic = new CubeCoordinates( 1, 0, -1 );
			CubeCoordinates endCubic = new CubeCoordinates( -2, 2, 0 );
			CubeCoordinates[] line = CubeCoordinates.Line( startCubic, endCubic );

			Assert.That( line, Is.EquivalentTo( new CubeCoordinates[ 4 ] {
				new CubeCoordinates(  1,  0, -1 ),
				new CubeCoordinates(  0,  1, -1 ),
				new CubeCoordinates( -1,  1,  0 ),
				new CubeCoordinates( -2,  2,  0 )
			} ) );
		}

		[Test]
		public void StaticRing()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 0, -1 );
			CubeCoordinates[] ring = CubeCoordinates.Ring( cubic, 2, Direction.W );

			Assert.That( ring, Is.EquivalentTo( new CubeCoordinates[ 12 ] {
				new CubeCoordinates( -1,  2, -1 ),
				new CubeCoordinates(  0,  2, -2 ),
				new CubeCoordinates(  1,  2, -3 ),
				new CubeCoordinates(  2,  1, -3 ),
				new CubeCoordinates(  3,  0, -3 ),
				new CubeCoordinates(  3, -1, -2 ),
				new CubeCoordinates(  3, -2, -1 ),
				new CubeCoordinates(  2, -2,  0 ),
				new CubeCoordinates(  1, -2,  1 ),
				new CubeCoordinates(  0, -1,  1 ),
				new CubeCoordinates( -1,  0,  1 ),
				new CubeCoordinates( -1,  1,  0 )
			} ) );
		}

		[Test]
		public void StaticRotate()
		{
			// Not yet implemented
			Assert.Ignore();
		}

		[Test]
		public void StaticSpiralInward()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 0, -1 );
			CubeCoordinates[] spiral = CubeCoordinates.SpiralOutward( cubic, 2, Direction.W );

			Assert.That( spiral, Is.EquivalentTo( new CubeCoordinates[ 19 ] {
				// Distance 2
				new CubeCoordinates( -1,  2, -1 ),
				new CubeCoordinates(  0,  2, -2 ),
				new CubeCoordinates(  1,  2, -3 ),
				new CubeCoordinates(  2,  1, -3 ),
				new CubeCoordinates(  3,  0, -3 ),
				new CubeCoordinates(  3, -1, -2 ),
				new CubeCoordinates(  3, -2, -1 ),
				new CubeCoordinates(  2, -2,  0 ),
				new CubeCoordinates(  1, -2,  1 ),
				new CubeCoordinates(  0, -1,  1 ),
				new CubeCoordinates( -1,  0,  1 ),
				new CubeCoordinates( -1,  1,  0 ),
				// Distance 1
				new CubeCoordinates(  0,  1, -1 ),
				new CubeCoordinates(  1,  1, -2 ),
				new CubeCoordinates(  2,  0, -2 ),
				new CubeCoordinates(  2, -1, -1 ),
				new CubeCoordinates(  1, -1,  0 ),
				new CubeCoordinates(  0,  0,  0 ),
				// Center
				new CubeCoordinates(  1,  0, -1 )
			} ) );
		}

		[Test]
		public void StaticSpiralOutward()
		{
			CubeCoordinates cubic = new CubeCoordinates( 1, 0, -1 );
			CubeCoordinates[] spiral = CubeCoordinates.SpiralOutward( cubic, 2, Direction.W );

			Assert.That( spiral, Is.EquivalentTo( new CubeCoordinates[ 19 ] {
				// Center
				new CubeCoordinates(  1,  0, -1 ),
				// Distance 1
				new CubeCoordinates(  0,  1, -1 ),
				new CubeCoordinates(  1,  1, -2 ),
				new CubeCoordinates(  2,  0, -2 ),
				new CubeCoordinates(  2, -1, -1 ),
				new CubeCoordinates(  1, -1,  0 ),
				new CubeCoordinates(  0,  0,  0 ),
				// Distance 2
				new CubeCoordinates( -1,  2, -1 ), 
				new CubeCoordinates(  0,  2, -2 ),
				new CubeCoordinates(  1,  2, -3 ),
				new CubeCoordinates(  2,  1, -3 ),
				new CubeCoordinates(  3,  0, -3 ),
				new CubeCoordinates(  3, -1, -2 ),
				new CubeCoordinates(  3, -2, -1 ),
				new CubeCoordinates(  2, -2,  0 ),
				new CubeCoordinates(  1, -2,  1 ),
				new CubeCoordinates(  0, -1,  1 ),
				new CubeCoordinates( -1,  0,  1 ),
				new CubeCoordinates( -1,  1,  0 )
			} ) );
		}
		
		#endregion
	}
}
