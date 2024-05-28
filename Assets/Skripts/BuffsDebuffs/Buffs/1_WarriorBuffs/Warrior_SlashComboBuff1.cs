using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_SlashComboBuff1 : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Warrior_SlashComboBuff1";
        buffDescription = "Your next Sweeping Slash deals double damage. ";
        base.StartBuffEffect(playerStats);

        isRemovable = false;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
    }

    public override Buff Clone()
    {
        Warrior_SlashComboBuff1 clone = (Warrior_SlashComboBuff1)this.MemberwiseClone();
        return clone;
    }
}
