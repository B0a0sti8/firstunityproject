using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostBuff : Buff
{
    public override void StartBuffEffect(PlayerStats playerStats)
    {
        base.StartBuffEffect(playerStats);
        playerStats.movementSpeed.AddModifierAdd(value);
    }

    public override void EndBuffEffect(PlayerStats playerStats)
    {
        base.EndBuffEffect(playerStats);
        playerStats.movementSpeed.AddModifierAdd(-value);
    }

    public override Buff Clone()
    {
        SpeedBoostBuff clone = (SpeedBoostBuff)this.MemberwiseClone();
        return clone;
    }
}