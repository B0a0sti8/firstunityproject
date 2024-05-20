using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGreatSacrificeBuff : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "The great sacrifice";
        buffDescription = "Buffs all main stats";
        base.StartBuffEffect(playerStats);
        ((PlayerStats)playerStats).mastery.baseValue += value;
        ((PlayerStats)playerStats).toughness.baseValue += value;
        ((PlayerStats)playerStats).intellect.baseValue += value;
        ((PlayerStats)playerStats).charisma.baseValue += value;
        ((PlayerStats)playerStats).tempo.baseValue += value;
        ((PlayerStats)playerStats).ComputeSideStats();
        isRemovable = false;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
        ((PlayerStats)playerStats).mastery.baseValue -= value;
        ((PlayerStats)playerStats).toughness.baseValue -= value;
        ((PlayerStats)playerStats).intellect.baseValue -= value;
        ((PlayerStats)playerStats).charisma.baseValue -= value;
        ((PlayerStats)playerStats).tempo.baseValue -= value;
    }

    public override Buff Clone()
    {
        TheGreatSacrificeBuff clone = (TheGreatSacrificeBuff)this.MemberwiseClone();
        return clone;
    }
}
