using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_OffensiveStanceBuff : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Warrior_OffensiveStanceBuff";
        buffDescription = "You are in offensive Stance";
        base.StartBuffEffect(playerStats);
        ((PlayerStats)playerStats).tempo.AddModifierMultiply(value);
        ((PlayerStats)playerStats).dmgInc.AddModifierMultiply(value);
        ((PlayerStats)playerStats).toughness.AddModifierMultiply(-value / 2);
        ((PlayerStats)playerStats).healInc.AddModifierMultiply(-value / 2);
        ((PlayerStats)playerStats).ComputeSideStats();
        isRemovable = false;
        playerStats.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>().offensiveStanceOn = true;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
        ((PlayerStats)playerStats).tempo.RemoveModifierMultiply(value);
        ((PlayerStats)playerStats).dmgInc.RemoveModifierMultiply(value);
        ((PlayerStats)playerStats).toughness.RemoveModifierMultiply(-value / 2);
        ((PlayerStats)playerStats).healInc.RemoveModifierMultiply(-value / 2);
        ((PlayerStats)playerStats).ComputeSideStats();
        playerStats.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>().offensiveStanceOn = true;
    }

    public override Buff Clone()
    {
        Warrior_OffensiveStanceBuff clone = (Warrior_OffensiveStanceBuff)this.MemberwiseClone();
        return clone;
    }
}
