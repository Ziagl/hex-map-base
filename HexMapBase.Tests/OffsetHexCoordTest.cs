﻿using com.hexagonsimulations.Geometry.Hex;

namespace com.hexagonsimulations.Geometry.HexGridTest
{
    [TestFixture]
	public class OffsetHexCoordTest
	{
		#region Properties

		[Test]
		public void PropertyIsOddRow()
		{
			bool isTrue  = new OffsetCoordinates( 2, 3 ).IsOddRow;
			bool isFalse = new OffsetCoordinates( 1, 2 ).IsOddRow;
			
			Assert.That( isTrue,  Is.True  );
			Assert.That( isFalse, Is.False );
		}

		[Test]
		public void PropertyRowParity()
		{
			ParityEnum odd  = new OffsetCoordinates( 2, 3 ).RowParity;
			ParityEnum even = new OffsetCoordinates( 1, 2 ).RowParity;
			
			Assert.That( odd,  Is.EqualTo( ParityEnum.Odd  ) );
			Assert.That( even, Is.EqualTo( ParityEnum.Even ) );
		}

		#endregion

		#region Constructors
		
		[Test]
		public void ConstructorQR()
		{
			OffsetCoordinates offset = new OffsetCoordinates( 1, 2 );

			Assert.That( offset.q, Is.EqualTo( 1 ) );
			Assert.That( offset.r, Is.EqualTo( 2 ) );
		}

		[Test]
		public void ConstructorParameterless()
		{
			OffsetCoordinates offset = new OffsetCoordinates();

			Assert.That( offset.q, Is.EqualTo( 0 ) );
			Assert.That( offset.r, Is.EqualTo( 0 ) );
		}

		#endregion

		#region Type Conversions

		[Test]
		public void ToCubic()
		{
			// Odd row
			CubeCoordinates cubic = new OffsetCoordinates( 1, 2 ).ToCubic();

			Assert.That( cubic.x, Is.EqualTo(  0 ) );
			Assert.That( cubic.y, Is.EqualTo( -2 ) );
			Assert.That( cubic.z, Is.EqualTo(  2 ) );

			// Even row
			cubic = new OffsetCoordinates( 2, 3 ).ToCubic();

			Assert.That( cubic.x, Is.EqualTo(  1 ) );
			Assert.That( cubic.y, Is.EqualTo( -4 ) );
			Assert.That( cubic.z, Is.EqualTo(  3 ) );
		}

		#endregion

		#region Operator Overloads

		[Test]
		public void OperatorOverloadPlus()
		{
			OffsetCoordinates offset = new OffsetCoordinates( 1, 2 ) + new OffsetCoordinates( 3, 4 );

			Assert.That( offset.q, Is.EqualTo( 4 ) );
			Assert.That( offset.r, Is.EqualTo( 6 ) );
		}

		[Test]
		public void OperatorOverloadMinus()
		{
			OffsetCoordinates offset = new OffsetCoordinates( 4, 3 ) - new OffsetCoordinates( 1, 2 );

			Assert.That( offset.q, Is.EqualTo( 3 ) );
			Assert.That( offset.r, Is.EqualTo( 1 ) );
		}

		[Test]
		public void OperatorOverloadEquals()
		{
			bool isTrue  = new OffsetCoordinates( 1, 2 ) == new OffsetCoordinates( 1, 2 );
			bool isFalse = new OffsetCoordinates( 1, 2 ) == new OffsetCoordinates( 3, 4 );

			Assert.That( isTrue,  Is.True  );
			Assert.That( isFalse, Is.False );
		}

		[Test]
		public void OperatorOverloadNotEquals()
		{
			bool isTrue  = new OffsetCoordinates( 1, 2 ) != new OffsetCoordinates( 3, 4 );
			bool isFalse = new OffsetCoordinates( 1, 2 ) != new OffsetCoordinates( 1, 2 );
			
			Assert.That( isTrue,  Is.True  );
			Assert.That( isFalse, Is.False );
		}

		#endregion
	}
}
