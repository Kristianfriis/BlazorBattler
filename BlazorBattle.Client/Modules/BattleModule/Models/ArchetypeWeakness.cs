namespace BlazorBattle.Client.Modules.BattleModule.Models;

public class ArchetypeWeakness
{
    public static Archetype GetWeakness(Archetype archetype)
    {
        return archetype switch
        {
            Archetype.Warrior => Archetype.Rogue, // Warrior is weak against Rogue
            Archetype.Mage => Archetype.Warrior,  // Mage is weak against Warrior
            Archetype.Rogue => Archetype.Mage,    // Rogue is weak against Mage
            _ => archetype
        };
    }
}