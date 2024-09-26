using com.hexagonsimulations.Geometry.Hex;

namespace com.hexagonsimulations.Geometry.Test
{
    [TestFixture]
    public class HexTileTest
    {
        [Test]
        public void Neighbors()
        {
            HexTile tile = new() { Coordinates = new CubeCoordinates(1, 1,-2) };
            var grid = Enumerable.Repeat(new HexTile(), 9).ToList();
            HexGrid.InitializeGrid(grid, 3, 3);
            var neighbors = tile.Neighbors(grid, 3, 3);
            Assert.That(
                neighbors,
                Is.EquivalentTo(
                    new List<HexTile>()
                    {
                        new() { Coordinates = new CubeCoordinates(1, 0,-1) },
                        new() { Coordinates = new CubeCoordinates(2, 0,-2) },
                        new() { Coordinates = new CubeCoordinates(2, 1,-3) },
                        new() { Coordinates = new CubeCoordinates(1, 2,-3) },
                        new() { Coordinates = new CubeCoordinates(0, 2,-2) },
                        new() { Coordinates = new CubeCoordinates(0, 1,-1) },
                    }
                )
            );
        }
    }
}
