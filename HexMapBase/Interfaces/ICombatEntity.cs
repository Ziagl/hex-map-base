using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace com.hexagonsimulations.Geometry.Hex.Interfaces;

/// <summary>
/// This is the interface for all game objects that can e involved into combat.
/// They have values for attack and defence and health.
/// </summary>
public interface ICombatEntity
{
    // health
    public int Health { get; set; }     // current health of the entity
    public int MaxHealth { get; set; }  // maximum health of the entity
    // combat
    public int WeaponType { get; set; } // type of weapon/combat of this unit (infantry, cavalry, ...)
    public int Attack { get; set; }     // attack points (damage in fight)
    public int RangedAttack { get; set; } // ranged attack points (damage of airstrike)
    public int Defense { get; set; }    // defence points (how much damage is reduced)
    public int Range { get; set; }      // attack range (how far can this unit attack)
}
