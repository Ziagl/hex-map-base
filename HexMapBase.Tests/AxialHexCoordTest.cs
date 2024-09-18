using com.hexagonsimulations.Geometry.Hex;

namespace com.hexagonsimulations.Geometry.HexGridTest
{
    [TestFixture]
	public class AxialHexCoordTest
	{
		#region Constructors

		[Test]
		public void ConstructorQR()
		{
			AxialCoordinates axial = new AxialCoordinates( 1, 2 );

			Assert.That( axial.q, Is.EqualTo( 1 ) );
			Assert.That( axial.r, Is.EqualTo( 2 ) );
		}

		[Test]
		public void ConstructorParameterless()
		{
			AxialCoordinates axial = new AxialCoordinates();

			Assert.That( axial.q, Is.EqualTo( 0 ) );
			Assert.That( axial.r, Is.EqualTo( 0 ) );
		}

		#endregion

		#region Type Conversions

		[Test]
		public void ToCubic()
		{
			CubeCoordinates cubic = new AxialCoordinates( 1, 2 ).ToCubic();

			Assert.That( cubic.x, Is.EqualTo(  1 ) );
			Assert.That( cubic.y, Is.EqualTo( -3 ) );
			Assert.That( cubic.z, Is.EqualTo(  2 ) );
		}

		#endregion

		#region Operator Overloads

		[Test]
		public void OperatorOverloadPlus()
		{
			AxialCoordinates axial = new AxialCoordinates( 1, 2 ) + new AxialCoordinates( 3, 4 );

			Assert.That( axial.q, Is.EqualTo( 4 ) );
			Assert.That( axial.r, Is.EqualTo( 6 ) );
		}

		[Test]
		public void OperatorOverloadMinus()
		{
			AxialCoordinates axial = new AxialCoordinates( 4, 3 ) - new AxialCoordinates( 1, 2 );

			Assert.That( axial.q, Is.EqualTo( 3 ) );
			Assert.That( axial.r, Is.EqualTo( 1 ) );
		}

		[Test]
		public void OperatorOverloadEquals()
		{
			bool isTrue  = new AxialCoordinates( 1, 2 ) == new AxialCoordinates( 1, 2 );
			bool isFalse = new AxialCoordinates( 1, 2 ) == new AxialCoordinates( 3, 4 );

			Assert.That( isTrue,  Is.True  );
			Assert.That( isFalse, Is.False );
		}

		[Test]
		public void OperatorOverloadNotEquals()
		{
			bool isTrue  = new AxialCoordinates( 1, 2 ) != new AxialCoordinates( 3, 4 );
			bool isFalse = new AxialCoordinates( 1, 2 ) != new AxialCoordinates( 1, 2 );
			
			Assert.That( isTrue,  Is.True  );
			Assert.That( isFalse, Is.False );
		}

		#endregion
	}
}
