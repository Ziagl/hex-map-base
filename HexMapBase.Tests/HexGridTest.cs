using com.hexagonsimulations.HexMapBase.Geometry.Hex.Enums;
using com.hexagonsimulations.HexMapBase.Geometry.Hex;

namespace com.hexagonsimulations.HexMapBase.Tests
{
    [TestClass]
    public sealed class HexGridTest
    {
        // The range in which floating-point numbers are consider equal.
        public const float EPSILON = 0.000001f;

        // The result of (float)Math.Sqrt( 3f ) just for convenience.
        public const float SQRT_3 = 1.7320508075688772935274463415059f;

        [TestMethod]
        public void PropertyHexRadius()
        {
            float hexRadius = new HexGrid(2f).HexRadius;

            Assert.AreEqual(2f, hexRadius);
        }

        [TestMethod]
        public void PropertySlice()
        {
            var grid = new HexGrid(2f);
            float hexRadius = grid.HexRadius;
            Vec2D slice = grid.Slice;

            float xExpected = 0.5f * SQRT_3 * hexRadius;
            float yExpected = 0.5f * hexRadius;

            Assert.IsTrue(slice.x >= xExpected - EPSILON && slice.x <= xExpected + EPSILON);
            Assert.IsTrue(slice.y >= yExpected - EPSILON && slice.y <= yExpected + EPSILON);
        }

        [TestMethod]
        public void ConstructorHexRadius()
        {
            var grid = new HexGrid(2f);

            Assert.AreEqual(2f, grid.HexRadius);
        }

        [TestMethod]
        public void AxialToPoint()
        {
            var grid = new HexGrid(2f);
            float hexRadius = grid.HexRadius;
            Vec2D point = grid.AxialToPoint(new AxialCoordinates(10, 10));

            float xExpected = hexRadius * SQRT_3 * (10 + 10 / 2);
            float yExpected = hexRadius * (3f / 2f) * 10;

            Assert.IsTrue(point.x >= xExpected - EPSILON && point.x <= xExpected + EPSILON);
            Assert.IsTrue(point.y >= yExpected - EPSILON && point.y <= yExpected + EPSILON);
        }

        [TestMethod]
        public void OffsetToPoint()
        {
            var grid = new HexGrid(2f);
            float hexRadius = grid.HexRadius;

            // Test an even row hex
            Vec2D point = grid.OffsetToPoint(new OffsetCoordinates(10, 10));
            float xExpected = hexRadius * SQRT_3 * 10;
            float yExpected = hexRadius * (3f / 2f) * 10;

            Assert.IsTrue(point.x >= xExpected - EPSILON && point.x <= xExpected + EPSILON);
            Assert.IsTrue(point.y >= yExpected - EPSILON && point.y <= yExpected + EPSILON);

            // Text an odd row hex
            point = grid.OffsetToPoint(new OffsetCoordinates(10, 11));
            xExpected = hexRadius * SQRT_3 * (10 + 0.5f);
            yExpected = hexRadius * (3f / 2f) * 11;

            Assert.IsTrue(point.x >= xExpected - EPSILON && point.x <= xExpected + EPSILON);
            Assert.IsTrue(point.y >= yExpected - EPSILON && point.y <= yExpected + EPSILON);
        }

        [TestMethod]
        public void PointToCubic()
        {
            var grid = new HexGrid(3f);
            float hexRadius = grid.HexRadius;
            CubeCoordinates cubic = grid.PointToCubic(new Vec2D(10f, 10f));

            float rExpected = 10f / ((3f / 2f) * hexRadius);
            float qExpected = 10f / (SQRT_3 * hexRadius * (1f + 0.5f * rExpected));

            Console.WriteLine("Axial: q: " + qExpected + ", r: " + rExpected);

            float xExpected = qExpected;
            float zExpected = rExpected;
            float yExpected = -xExpected - zExpected;

            Console.WriteLine("Cubic: q: " + xExpected + ", r: " + yExpected + ", s: " + zExpected);

            // Now that I'm close enough to guess, I'll just do that rather than trying to emulate
            // the rounding all the way through. Not accurate, but good enough for government work.
            Assert.AreEqual(1, cubic.q);
            Assert.AreEqual(-3, cubic.r);
            Assert.AreEqual(2, cubic.s);
        }

        [TestMethod]
        public void PointToDirectionInHex()
        {
            var grid = new HexGrid(2f);
            Vec2D hexPos = grid.AxialToPoint(new AxialCoordinates(10, 10));
            float offset = 0.5f * grid.HexRadius;

            Vec2D[] points = new Vec2D[12]
            {
                new Vec2D(hexPos.x + offset, hexPos.y + 0f * offset - 0.01f), // 0a
                new Vec2D(hexPos.x + offset, hexPos.y + 0f * offset + 0.01f), // 0b
                new Vec2D(hexPos.x + offset, hexPos.y + 1f * offset - 0.01f), // 1a
                new Vec2D(hexPos.x + offset, hexPos.y + 1f * offset + 0.01f), // 1b
                new Vec2D(hexPos.x - offset, hexPos.y + 1f * offset + 0.01f), // 2a
                new Vec2D(hexPos.x - offset, hexPos.y + 1f * offset - 0.01f), // 2b
                new Vec2D(hexPos.x - offset, hexPos.y + 0f * offset + 0.01f), // 3a
                new Vec2D(hexPos.x - offset, hexPos.y + 0f * offset - 0.01f), // 3b
                new Vec2D(hexPos.x - offset, hexPos.y - 1f * offset + 0.01f), // 4a
                new Vec2D(hexPos.x - offset, hexPos.y - 1f * offset - 0.01f), // 4b
                new Vec2D(hexPos.x + offset, hexPos.y - 1f * offset - 0.01f), // 5a
                new Vec2D(
                    hexPos.x + offset,
                    hexPos.y - 1f * offset + 0.01f
                ) // 5b
                ,
            };

            Direction[] results = new Direction[12];
            for (int i = 0; i < points.Length; i++)
            {
                results[i] = grid.PointToDirectionInHex(points[i]);
            }

            CollectionAssert.AreEquivalent(
                results,
                new Direction[12]
                {
                    Direction.E, // 0a
                    Direction.E, // 0b
                    Direction.SE, // 1a
                    Direction.SE, // 1b
                    Direction.SW, // 2a
                    Direction.SW, // 2b
                    Direction.W, // 3a
                    Direction.W, // 3b
                    Direction.NW, // 4a
                    Direction.NW, // 4b
                    Direction.NE, // 5a
                    Direction.NE, // 5b
                }
            );
        }

        [TestMethod]
        public void InitializeGrid()
        {
            var grid = HexGrid.InitializeGrid<HexTile>(3, 3);
            Assert.IsTrue(grid[1].Coordinates == new CubeCoordinates(1, 0, -1));
            Assert.IsTrue(grid[^1].Coordinates == new CubeCoordinates(1, 2, -3));
        }
    }
}
