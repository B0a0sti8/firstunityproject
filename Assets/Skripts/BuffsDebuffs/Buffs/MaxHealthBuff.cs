using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthBuff : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Chonky";
        buffDescription = "Gain more <color=green>Max-Health</color>";
        base.StartBuffEffect(playerStats);
        playerStats.maxHealth.AddModifierAdd(value);
        playerStats.currentHealth.Value += value;
        isRemovable = false;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
        playerStats.maxHealth.AddModifierAdd(-value);
    }

    public override Buff Clone()
    {
        MaxHealthBuff clone = (MaxHealthBuff)this.MemberwiseClone();
        return clone;
    }
}