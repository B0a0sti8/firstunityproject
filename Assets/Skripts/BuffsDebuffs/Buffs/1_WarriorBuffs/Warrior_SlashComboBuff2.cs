using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_SlashComboBuff2 : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Warrior_SlashComboBuff2";
        buffDescription = " ";
        base.StartBuffEffect(playerStats);

        isRemovable = false;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
    }

    public override Buff Clone()
    {
        Warrior_SlashComboBuff2 clone = (Warrior_SlashComboBuff2)this.MemberwiseClone();
        return clone;
    }
}
