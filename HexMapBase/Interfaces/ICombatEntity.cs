namespace com.hexagonsimulations.HexMapBase.Interfaces;

/// <summary>
/// This is the interface for all game objects that can e involved into combat.
/// They have values for attack and defence and health.
/// </summary>
public interface ICombatEntity : IBaseEntity
{
    // health
    public int Health { get; set; }     // current health of the entity
    public int MaxHealth { get; set; }  // maximum health of the entity
    // combat
    public int WeaponType { get; set; } // type of weapon/combat of this unit (infantry, cavalry, ...)
    public int CombatStrength { get; set; } // attack and defense points (damage in fight)
    public int RangedAttack { get; set; } // ranged attack points (damage of airstrike)
    public int Range { get; set; }      // attack range (how far can this unit attack)
    // random numbers
    public int Seed { get; set; }       // random number seed for this unit for this turn
}
