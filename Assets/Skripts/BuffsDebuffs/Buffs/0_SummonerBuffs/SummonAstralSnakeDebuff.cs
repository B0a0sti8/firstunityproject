using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonAstralSnakeDebuff : Buff
{
    public override void StartBuffUI()
    {
        base.StartBuffUI();
        buffName = "SummonAstralSnakeDebuff";
        buffDescription = "Increases the damage taken by " + value + " %.";
    }

    public override void StartBuffEffect(CharacterStats enemyStats)
    {
        base.StartBuffEffect(enemyStats);
        enemyStats.armor.AddModifierAdd(value);
        isRemovable = true;
    }

    public override void EndBuffEffect(CharacterStats enemyStats)
    {
        base.EndBuffEffect(enemyStats);
        enemyStats.armor.RemoveModifierAdd(value);
    }

    public override Buff Clone()
    {
        SummonAstralSnakeDebuff clone = (SummonAstralSnakeDebuff)this.MemberwiseClone();
        return clone;
    }
}
