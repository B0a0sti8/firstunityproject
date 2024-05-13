using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMinionBuffDamageAndHealingBuff : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Minion Damage and Healing buff";
        buffDescription = "";
        base.StartBuffEffect(playerStats);
        ((EnemyStats)playerStats).dmgModifier.AddModifierAdd(value);
        isRemovable = false;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
        ((EnemyStats)playerStats).dmgModifier.RemoveModifierAdd(value);
    }

    public override Buff Clone()
    {
        MainMinionBuffDamageAndHealingBuff clone = (MainMinionBuffDamageAndHealingBuff)this.MemberwiseClone();
        return clone;
    }
}