namespace com.hexagonsimulations.Geometry.Hex.Interfaces;

/// <summary>
/// This is the base interface for all game objects that can be placed on the map.
/// They belong to a specific player and are described by an uniqu id.
/// </summary>
public interface IBaseEntity
{
    public int Id { get; set; }         // id of the entity (unique only in its scope)
    public int Player { get; set; }     // id of player who owns the entity
}
