using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_DefensiveStanceBuff : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Warrior_DefensiveStanceBuff";
        buffDescription = "You are in defensive Stance";
        base.StartBuffEffect(playerStats);
        ((PlayerStats)playerStats).toughness.AddModifierMultiply(value);
        ((PlayerStats)playerStats).healInc.AddModifierMultiply(value);
        ((PlayerStats)playerStats).tempo.AddModifierMultiply(-value / 2);
        ((PlayerStats)playerStats).dmgInc.AddModifierMultiply(-value / 2);
        ((PlayerStats)playerStats).ComputeSideStats();
        isRemovable = false;
        playerStats.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>().defensiveStanceOn = true;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
        ((PlayerStats)playerStats).toughness.RemoveModifierMultiply(value);
        ((PlayerStats)playerStats).healInc.RemoveModifierMultiply(value);
        ((PlayerStats)playerStats).tempo.RemoveModifierMultiply(-value / 2);
        ((PlayerStats)playerStats).dmgInc.RemoveModifierMultiply(-value / 2);
        ((PlayerStats)playerStats).ComputeSideStats();
        playerStats.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>().defensiveStanceOn = false;
    }

    public override Buff Clone()
    {
        Warrior_DefensiveStanceBuff clone = (Warrior_DefensiveStanceBuff)this.MemberwiseClone();
        return clone;
    }
}
