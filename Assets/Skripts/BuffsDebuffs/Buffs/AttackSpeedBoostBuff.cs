using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedBoostBuff : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Haste";
        buffDescription = "haha attack go brrrr\nOi\nlongboii";
        base.StartBuffEffect(playerStats);
        playerStats.actionSpeed.AddModifierAdd(value);
        isRemovable = false;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
        playerStats.actionSpeed.AddModifierAdd(-value);
    }

    public override Buff Clone()
    {
        AttackSpeedBoostBuff clone = (AttackSpeedBoostBuff)this.MemberwiseClone();
        return clone;
    }
}
