using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedBoostBuff : Buff
{
    public override void StartBuffEffect(PlayerStats playerStats)
    {
        buffName = "Haste";
        buffDescription = "haha attack go brrrr\nOi\nlongboii";
        base.StartBuffEffect(playerStats);
        playerStats.attackSpeed.AddModifierAdd(value);
        isRemovable = false;
    }

    public override void EndBuffEffect(PlayerStats playerStats)
    {
        base.EndBuffEffect(playerStats);
        playerStats.attackSpeed.AddModifierAdd(-value);
    }

    public override Buff Clone()
    {
        AttackSpeedBoostBuff clone = (AttackSpeedBoostBuff)this.MemberwiseClone();
        return clone;
    }
}
