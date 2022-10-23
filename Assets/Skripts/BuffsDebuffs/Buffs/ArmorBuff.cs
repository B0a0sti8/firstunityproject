using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBuff : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "<color=grey>ARMOR</color>!";
        buffDescription = "Gives Armor (50)";
        base.StartBuffEffect(playerStats);
        playerStats.armor.AddModifierAdd(value);
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
        playerStats.armor.AddModifierAdd(-value);
    }

    public override Buff Clone()
    {
        ArmorBuff clone = (ArmorBuff)this.MemberwiseClone();
        return clone;
    }
}
