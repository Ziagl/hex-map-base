using com.hexagonsimulations.HexMapBase.Geometry.Hex;

namespace com.hexagonsimulations.HexMapBase.Tests
{
    [TestClass]
    public sealed class HexTileTest
    {
        [TestMethod]
        public void Neighbors()
        {
            // base
            int width = 3;
            int height = 3;
            HexTile tile = new() { Coordinates = new CubeCoordinates(1, 1, -2) };
            var grid = HexGrid.InitializeGrid<HexTile>(height, width);
            var neighbors = tile.Neighbors(grid, height, width);
            CollectionAssert.AreEquivalent(
                neighbors,
                new List<HexTile>()
                {
                    new() { Coordinates = new CubeCoordinates(1, 0,-1) },
                    new() { Coordinates = new CubeCoordinates(2, 0,-2) },
                    new() { Coordinates = new CubeCoordinates(2, 1,-3) },
                    new() { Coordinates = new CubeCoordinates(1, 2,-3) },
                    new() { Coordinates = new CubeCoordinates(0, 2,-2) },
                    new() { Coordinates = new CubeCoordinates(0, 1,-1) },
                }
            );
            //example
            width = 74;
            height = 46;
            tile = new() { Coordinates = new CubeCoordinates(0, 0, 0) };
            grid = HexGrid.InitializeGrid<HexTile>(height, width);
            neighbors = tile.Neighbors(grid, height, width);
            CollectionAssert.AreEquivalent(
                neighbors,
                new List<HexTile>()
                {
                    new() { Coordinates = new CubeCoordinates(1, 0,-1) },
                    new() { Coordinates = new CubeCoordinates(0, 1,-1) },
                }
            );
        }
    }
}
