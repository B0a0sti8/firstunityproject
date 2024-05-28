using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_DevastatingStrikeDebuff : Buff
{
    public override void StartBuffUI()
    {
        base.StartBuffUI();
        buffName = "Warrior_DevastatingStrikeDebuff";
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
        Warrior_DevastatingStrikeDebuff clone = (Warrior_DevastatingStrikeDebuff)this.MemberwiseClone();
        return clone;
    }
}
