namespace com.hexagonsimulations.Geometry.Hex.Interfaces;

/// <summary>
/// This is the base interface for all game objects that can be placed on the map.
/// They belong to a specific player and it has a health value that describes its life status.
/// </summary>
public interface IGameEntity
{
    public int Id { get; set; }         // id of the entity (unique only in its scope)
    public int Player { get; set; }     // id of player who owns the entity
    public int Health { get; set; }     // current health of the entity
    public int MaxHealth { get; set; }  // maximum health of the entity
}
