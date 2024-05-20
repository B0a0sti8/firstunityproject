using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonerSpiderSlowEffectBuff : Buff
{
    public override void StartBuffUI()
    {
        base.StartBuffUI();
        buffName = "SummonerSpiderSlowEffect";
        buffDescription = "Reduces the movement speed by " + (value * 100).ToString() + " %.";
    }

    public override void StartBuffEffect(CharacterStats enemyStats)
    {
        base.StartBuffEffect(enemyStats);
        enemyStats.movementSpeed.AddModifierAdd(value);
        isRemovable = true;
    }

    public override void EndBuffEffect(CharacterStats enemyStats)
    {
        base.EndBuffEffect(enemyStats);
        enemyStats.movementSpeed.RemoveModifierAdd(value);
    }

    public override Buff Clone()
    {
        SummonerSpiderSlowEffectBuff clone = (SummonerSpiderSlowEffectBuff)this.MemberwiseClone();
        return clone;
    }
}
