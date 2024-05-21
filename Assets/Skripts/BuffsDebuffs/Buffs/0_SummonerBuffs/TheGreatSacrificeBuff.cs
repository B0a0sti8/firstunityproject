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
        ((PlayerStats)playerStats).mastery.AddModifierMultiply(value);
        ((PlayerStats)playerStats).toughness.AddModifierMultiply(value);
        ((PlayerStats)playerStats).intellect.AddModifierMultiply(value);
        ((PlayerStats)playerStats).charisma.AddModifierMultiply(value);
        ((PlayerStats)playerStats).tempo.AddModifierMultiply(value);
        ((PlayerStats)playerStats).ComputeSideStats();
        isRemovable = false;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
        ((PlayerStats)playerStats).mastery.RemoveModifierMultiply(value);
        ((PlayerStats)playerStats).toughness.RemoveModifierMultiply(value);
        ((PlayerStats)playerStats).intellect.RemoveModifierMultiply(value);
        ((PlayerStats)playerStats).charisma.RemoveModifierMultiply(value);
        ((PlayerStats)playerStats).tempo.RemoveModifierMultiply(value);
    }

    public override Buff Clone()
    {
        TheGreatSacrificeBuff clone = (TheGreatSacrificeBuff)this.MemberwiseClone();
        return clone;
    }
}
