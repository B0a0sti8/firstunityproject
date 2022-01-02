using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthBuff : Buff
{
    public override void Dispell()
    {
        isRemovable = false;
        base.Dispell();
    }

    public override void StartBuffEffect(PlayerStats playerStats)
    {
        base.StartBuffEffect(playerStats);
        playerStats.maxHealth.AddModifierAdd(value);
        playerStats.currentHealth += value;
    }

    public override void EndBuffEffect(PlayerStats playerStats)
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