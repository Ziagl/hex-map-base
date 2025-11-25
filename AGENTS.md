# AI Agent Instructions for HexMapBase

## Project Overview

**HexMapBase** is a .NET library implementing hexagonal grid operations based on the algorithms from [Red Blob Games](http://www.redblobgames.com/grids/hexagons/). It provides coordinate system conversions, spatial operations, and combat entity interfaces for hexagonal tile-based games.

**Target Framework**: .NET 10  
**Language Version**: C# 14.0  
**Package ID**: HexMapBase  
**Current Version**: 0.5.0  
**Company**: Hexagon Simulations  
**License**: See LICENSE file  

## Core Concepts

### Coordinate Systems

The library implements three coordinate systems with conversion methods:

1. **CubeCoordinates** (q, r, s) - Primary system for computations
   - Constraint: `q + r + s = 0`
   - Most efficient for spatial operations
   - Supports: neighbors, diagonals, distance, line drawing, rings, spirals, area calculations
   - **Serializable**: JSON, Binary, Span-based
   - **Dictionary Key Support**: Custom JsonConverter for use in `Dictionary<CubeCoordinates, T>`

2. **AxialCoordinates** (q, r) - 2D representation
   - Parallelogram grid layout
   - Good for rectangular array storage
   - Less computational overhead than offset
   - **Serializable**: JSON, Binary, Span-based
   - **Dictionary Key Support**: Custom JsonConverter for use in `Dictionary<AxialCoordinates, T>`

3. **OffsetCoordinates** (x, y) - Odd-row offset
   - Pointy-topped hexagons
   - Odd rows offset by half hex width
   - Most intuitive for rectangular storage
   - Least efficient for computations
   - **Serializable**: JSON, Binary, Span-based
   - **Dictionary Key Support**: Custom JsonConverter for use in `Dictionary<OffsetCoordinates, T>`

### Key Operations

All coordinate types support:
- **Arithmetic**: `+`, `-`, `==`, `!=`
- **Conversions**: `ToCubic()`, `ToAxial()`, `ToOffset()`
- **Serialization**: 
  - JSON: `ToJson()`, `FromJson()`
  - Binary: `Write()`, `Read()`
  - Span: `TryWriteBytes()`, `TryRead()`
- **Dictionary Keys**: All three coordinate types can be used as dictionary keys in JSON serialization

CubeCoordinates additionally provides:
- `Neighbor(Direction)` - Get adjacent hex
- `Neighbors()` - Get all 6 neighbors
- `Diagonal(Diagonal)` - Get diagonal hex
- `Diagonals()` - Get all 6 diagonals
- `DistanceTo(other)` - Manhattan distance
- `LineTo(other)` - Straight line path
- `RingAround(range)` - Hexes at exact distance
- `AreaAround(range)` - All hexes within distance
- `SpiralAroundInward(range)` - Spiral from outside to center
- `SpiralAroundOutward(range)` - Spiral from center outward
- `Scale(factor)` - Proportional scaling

### Floating-Point Coordinate Types

For sub-hex precision and geometric calculations, the library provides floating-point versions of the coordinate systems:

1. **FloatAxial** (q, r) - Floating-point axial coordinates
   - Fields: `q`, `r` (both `float`)
   - Constructor from `AxialCoordinates` or `(float q, float r)`
   - `ToFloatCubic()` - Convert to cubic space
   - Used internally for interpolation and conversions

2. **FloatCubic** (q, r, s) - Floating-point cubic coordinates
   - Fields: `q`, `r`, `s` (all `float`)
   - Constructor from `CubeCoordinates` or `(float q, float r, float s)`
   - `Round()` - **Critical method**: Rounds to nearest integer hex
   - `Scale(float factor)` - Proportional scaling
   - `ToFloatAxial()` - Convert to axial space
   - Used in line drawing (`CubeCoordinates.LineTo()`) and scaling operations

**When to Use**:
- Don't create these directly in typical game code
- Used automatically by `CubeCoordinates.LineTo()`, `.Scale()`, and `HexGrid.PointToCubic()`
- Useful for custom interpolation or sub-hex calculations
- `FloatCubic.Round()` is the standard algorithm for converting fractional coordinates to the nearest hex

### World-Space Integration

**HexGrid** bridges hex coordinate space and 2D world/screen space, enabling conversion between pixel positions and hex coordinates.

**Setup**:
```csharp
float hexRadius = 50f; // Distance from center to vertex (pointy-top)
HexGrid grid = new HexGrid(hexRadius);
// Or use static factory:
var tiles = HexGrid.InitializeGrid<MyCustomTile>(rows, columns);
```

**Properties**:
- `HexRadius` - Distance from center to vertex
- `HexHeight` - Full height of hexagon
- Constants: `SQRT_3`, `SQRT_3_2`, `SQRT_3_3`

**Key Operations**:

1. **Hex → World Position**:
   ```csharp
   Vec2D worldPos = grid.AxialToPoint(axialCoords);
   Vec2D worldPos = grid.OffsetToPoint(offsetCoords);
   ```

2. **World Position → Hex** (most common for mouse input):
   ```csharp
   Vec2D mousePosition = new Vec2D(screenX, screenY);
   CubeCoordinates hex = grid.PointToCubic(mousePosition);
   ```

3. **Direction Within Hex**:
   ```csharp
   Direction dir = grid.PointToDirectionInHex(mousePosition);
   // Returns which face of the hex the point is closest to
   ```

4. **Grid Initialization**:
   ```csharp
   var tiles = HexGrid.InitializeGrid<MyCustomTile>(rows, columns);
   // Creates List<T> with coordinates pre-assigned to each tile
   ```

**Note**: Y-axis increases **downward** (standard screen coordinates).

### Geometric Operations

**Vec2D** - 2D vector for world-space calculations:
- Fields: `x`, `y` (both `float`)
- Operators: `+`, `-`, `*` (scalar multiplication), `==`, `!=`
- `Distance(Vec2D other)` - Euclidean distance between points
- `Length()` - Vector magnitude
- Static `Rotate(Vec2D v, float angle)` - Rotate by degrees (clockwise)
- Static `Normalize(Vec2D v)` - Unit vector
- Static `DotProduct(Vec2D a, Vec2D b)` - Dot product

**Hexagon** - Geometric hexagon model for precise hit testing:
- Constructor: `Hexagon(Vec2D centerPoint, float halfSize)`
- `IsInside(Vec2D point)` - Precise point-in-hexagon test
- Properties: `CenterPoint`, `HalfSize`, corner positions (`CornerNE`, `CornerE`, etc.)

**Use Case**: When you need pixel-perfect collision detection beyond HexGrid's hex-level precision, use `Hexagon.IsInside()` for exact geometric testing.

### HexTile Base Class

**HexTile** is the base class for custom tile types, designed for use in derived libraries and game implementations.

**Built-in Features**:
```csharp
public class MyTile : HexTile
{
    public int TerrainType { get; set; }
    public IBaseEntity OccupyingUnit { get; set; }
    // Add your custom properties
}

HexTile tile = new MyTile();
tile.Coordinates = new CubeCoordinates(0, 0, 0);

// Get valid neighbors (respects grid boundaries)
List<HexTile> neighbors = tile.Neighbors(allTiles, rows, columns);
```

**Key Methods**:
- `Coordinates` (property) - `CubeCoordinates` position
- `Equals()` - Compares by `Coordinates` (sealed)
- `GetHashCode()` - Based on `Coordinates` (sealed)
- `Neighbors(List<HexTile> tiles, int rows, int columns)` - Returns valid neighbors only, handling grid boundaries

**Best Practice**: Inherit from `HexTile` for any game object that occupies a hex position and needs neighbor queries with boundary validation.

### Pathfinding Support

**WeightedCubeCoordinates** - Record type for pathfinding with terrain costs:
```csharp
public record WeightedCubeCoordinates
{
    public CubeCoordinates Coordinates;
    public int Cost;
}
```

**Use Cases**: 
- A* pathfinding
- Dijkstra's algorithm
- Movement cost calculations
- Terrain-based cost modeling

**Example**:
```csharp
var path = new List<WeightedCubeCoordinates>();
path.Add(new WeightedCubeCoordinates 
{ 
    Coordinates = start, 
    Cost = 0 
});

foreach (var neighbor in current.Neighbors())
{
    int terrainCost = GetTerrainCost(neighbor);
    path.Add(new WeightedCubeCoordinates 
    { 
        Coordinates = neighbor, 
        Cost = currentCost + terrainCost 
    });
}
```

**Note**: This is a simple data container - implement your own pathfinding algorithm using this structure.

## Coding Standards

### Naming Conventions

- **Structs**: PascalCase (e.g., `CubeCoordinates`)
- **Public fields**: lowercase (e.g., `q`, `r`, `s`, `x`, `y`)
- **Properties**: PascalCase (e.g., `IsOddRow`, `RowParity`)
- **Methods**: PascalCase (e.g., `ToCubic`, `Neighbor`)
- **Enums**: PascalCase with UPPERCASE values (e.g., `Direction.E`, `Parity.Odd`)

### Code Style

- Use `struct` for coordinate types (value semantics)
- Implement `IEquatable<T>` for all coordinate types
- Override `GetHashCode()`, `Equals()`, `ToString()`
- Use `readonly` for immutable properties
- Prefer expression-bodied members for simple methods
- Use pattern matching in switch expressions
- Apply `[Serializable]`, `[StructLayout(LayoutKind.Sequential)]` to coordinate structs
- Apply `[JsonConverter(typeof(CustomConverter))]` for dictionary key support

## Testing Guidelines

### Test Framework

- **MSTest** (`Microsoft.VisualStudio.TestTools.UnitTesting`)
- Test class naming: `{TypeName}Test` (e.g., `CubeCoordinatesTest`)
- Test method naming: Descriptive of operation (e.g., `SerializationDeserializationKey`)

### Required Test Coverage

For each coordinate type, test:
1. Constructors (parameterless, parameterized)
2. Conversions between coordinate systems
3. Operator overloads (`+`, `-`, `==`, `!=`)
4. `ToString()` output format
5. Serialization/deserialization:
   - JSON round-trip
   - Binary round-trip
   - Span round-trip
   - Dictionary key serialization
6. Spatial operations (for CubeCoordinates)
7. Edge cases and boundary conditions

## Common Tasks

### Adding a New Coordinate Type

1. Create struct in `HexMapBase/Models/`
2. Implement:
   - `IEquatable<T>`
   - Constructor(s)
   - Public fields with `[JsonInclude]`
   - Conversion methods
   - Operators (`+`, `-`, `==`, `!=`)
   - `GetHashCode()`, `Equals()`, `ToString()`
   - Serialization (JSON, Binary, Span)
   - `ToKeyString()` and `TryParseKey()`
3. Create custom `JsonConverter` with `WriteAsPropertyName`/`ReadAsPropertyName`
4. Add `[JsonConverter(typeof(CustomConverter))]` attribute
5. Add comprehensive unit tests

### Adding Spatial Operations

1. Implement in `CubeCoordinates` (most efficient)
2. Provide both instance and static versions
3. Validate inputs (throw `ArgumentOutOfRangeException` for invalid ranges)
4. Return arrays for collections of coordinates
5. Document algorithm source (e.g., Red Blob Games)
6. Add unit tests with known results

### Adding Enums

1. Place in `HexMapBase/Enums/`
2. Use PascalCase for enum type
3. Use UPPERCASE for values
4. Document meaning and use cases

### Enums Reference

1. **Direction** - Hex face directions
   - Values: `E`, `SE`, `SW`, `W`, `NW`, `NE`
   - Used by: `CubeCoordinates.Neighbor()`, `CubeCoordinates.Neighbors()`

2. **Diagonal** - Diagonal hex positions (vertex to vertex)
   - Values: `ESE`, `S`, `WSW`, `WNW`, `N`, `ENE`
   - Used by: `CubeCoordinates.Diagonal()`, `CubeCoordinates.Diagonals()`

3. **Parity** - Row parity for offset coordinates
   - Values: `Even`, `Odd`
   - Used by: `OffsetCoordinates.RowParity` property

4. **Rotation** ⚠️ **NOT YET IMPLEMENTED**
   - Values: `CW_60`, `CW_120`, `CW_180`, `CW_240`, `CW_300`, `CCW_60`, `CCW_120`, `CCW_180`, `CCW_240`, `CCW_300`
   - Intended for: `CubeCoordinates.Rotate()` (currently throws `NotImplementedException`)
   - Future feature for hex rotation operations

5. **Triangle** - Internal enum for hex slice subdivision
   - Values: `Top`, `Bottom`
   - Used by: `HexGrid.PointToDirectionInHex()` for precise direction detection
   - Internal implementation detail for geometric calculations

## Architecture Best Practices

### Coordinate Type Selection

**Choose the right coordinate system for each operation**:

- **Storage**: Use `OffsetCoordinates` for rectangular arrays
  - Natural mapping to 2D arrays: `array[y, x]`
  - Most intuitive for rectangular storage
  
- **Computation**: Convert to `CubeCoordinates` for all spatial operations
  - Most efficient for neighbors, distance, rings, areas
  - Required for `LineTo()`, `AreaAround()`, etc.
  
- **Display/API**: Use `AxialCoordinates` for 2D UI or external APIs
  - Only two coordinates to serialize
  - Good for JSON APIs and save files
  
- **World Space**: Use `HexGrid` + `Vec2D` for screen/world positioning
  - Convert pixel coordinates to hex coordinates
  - Render hexes at correct world positions

### Type Conversion Flow

**Typical game loop flow**:
```
Screen Click (Vec2D) 
  → HexGrid.PointToCubic() 
  → CubeCoordinates (perform spatial operations)
  → ToOffset() for storage in grid array
  → OffsetToPoint() for rendering
```

**Example**:
```csharp
// 1. Mouse click
Vec2D mousePos = new Vec2D(Input.MouseX, Input.MouseY);

// 2. Convert to hex
CubeCoordinates clickedHex = hexGrid.PointToCubic(mousePos);

// 3. Calculate movement range
CubeCoordinates[] reachable = clickedHex.AreaAround(moveRange);

// 4. Store in grid
OffsetCoordinates offsetPos = clickedHex.ToOffset();
grid[offsetPos.y, offsetPos.x] = selectedUnit;

// 5. Render
Vec2D renderPos = hexGrid.OffsetToPoint(offsetPos);
DrawSprite(unitSprite, renderPos.x, renderPos.y);
```

### Custom Tile Implementation

**Pattern for game-specific tile types**:
```csharp
public class GameTile : HexTile
{
    public int TerrainType { get; set; }
    public int MovementCost { get; set; }
    public IBaseEntity OccupyingUnit { get; set; }
    public bool IsVisible { get; set; }
}

// Initialize grid
var grid = HexGrid.InitializeGrid<GameTile>(rows, columns);

// Access with O(1) lookup using offset coordinates
int index = y * columns + x;
var tile = grid[index];

// Or find by coordinates (O(n) search)
var tile = grid.FirstOrDefault(t => t.Coordinates == targetCoords);
```

### Combining with Game Entities

**Pattern for units and combat**:
```csharp
public class Unit : ICombatEntity
{
    // ICombatEntity implementation
    public int Id { get; set; }
    public int Player { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int WeaponType { get; set; }
    public int CombatStrength { get; set; }
    public int RangedAttack { get; set; }
    public int Range { get; set; }
    public int Seed { get; set; }
    
    // Position - store as CubeCoordinates for efficient operations
    public CubeCoordinates Position { get; set; }
    
    // Calculate movement range
    public List<CubeCoordinates> GetMovementRange(int movePoints)
    {
        return Position.AreaAround(movePoints)
            .Where(hex => IsValidTerrain(hex))
            .ToList();
    }
    
    // Calculate attack range
    public List<CubeCoordinates> GetAttackRange()
    {
        if (RangedAttack > 0)
            return Position.AreaAround(Range).ToList();
        else
            return Position.Neighbors(); // Melee - adjacent only
    }
    
    // Check if target is in range
    public bool CanAttack(CubeCoordinates target)
    {
        int distance = Position.DistanceTo(target);
        return RangedAttack > 0 ? distance <= Range : distance == 1;
    }
}
```

**Pathfinding with terrain costs**:
```csharp
public List<CubeCoordinates> FindPath(CubeCoordinates start, CubeCoordinates goal)
{
    var frontier = new PriorityQueue<WeightedCubeCoordinates>();
    frontier.Enqueue(new WeightedCubeCoordinates { Coordinates = start, Cost = 0 });
    
    var cameFrom = new Dictionary<CubeCoordinates, CubeCoordinates>();
    var costSoFar = new Dictionary<CubeCoordinates, int>();
    cameFrom[start] = start;
    costSoFar[start] = 0;
    
    while (frontier.Count > 0)
    {
        var current = frontier.Dequeue();
        
        if (current.Coordinates == goal)
            break;
            
        foreach (var next in current.Coordinates.Neighbors())
        {
            int newCost = costSoFar[current.Coordinates] + GetTerrainCost(next);
            
            if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
            {
                costSoFar[next] = newCost;
                int priority = newCost + next.DistanceTo(goal);
                frontier.Enqueue(new WeightedCubeCoordinates 
                { 
                    Coordinates = next, 
                    Cost = priority 
                });
                cameFrom[next] = current.Coordinates;
            }
        }
    }
    
    // Reconstruct path
    return ReconstructPath(cameFrom, start, goal);
}
```

## Important Notes

### Performance Considerations

- **Always use CubeCoordinates for calculations** - it's the most efficient
- Convert to Axial/Offset only for storage or display
- Offset↔Cube conversion is most expensive
- Use `stackalloc` for temporary span buffers

### Serialization Notes

- JSON serialization requires `IncludeFields = true` for public fields
- All coordinate types use custom converters for dictionary key support
- Binary serialization uses little-endian for cross-platform compatibility
- Dictionary keys serialized as strings in format: `"c1,c2,..."` (e.g., `"1,0,-1"`)

### Breaking Changes to Avoid

- Don't change public field names (breaks serialization)
- Don't modify `ToKeyString()` format (breaks existing serialized data)
- Don't change operator behavior
- Don't modify conversion algorithms (maintains compatibility)

### Combat Entity Interface

The library provides `ICombatEntity` for game objects:
- Extends `IBaseEntity`
- Properties: `Health`, `MaxHealth`, `WeaponType`, `CombatStrength`, `RangedAttack`, `Range`, `Seed`
- Use this interface for units, buildings, etc. that participate in combat

## Documentation Standards

### XML Comments Required For

- All public types, methods, properties
- Parameters and return values
- Exceptions thrown
- Examples for complex operations

## Getting Help

- **Algorithms**: Refer to [Red Blob Games](http://www.redblobgames.com/grids/hexagons/)
- **Repository**: https://github.com/Ziagl/hex-map-base
- **Company**: Hexagon Simulations (https://hexagon-simulations.com/)

## Version History Context

**Current**: 0.5.0
- .NET 10 target
- C# 14.0 features
- Full serialization support with dictionary key compatibility
- Comprehensive unit test coverage

When making changes, ensure backward compatibility with serialized data and maintain the established patterns for coordinate system operations.
