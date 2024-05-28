using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_StrikeComboBuff1 : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Warrior_StrikeComboBuff1";
        buffDescription = "Your next Brutal Strike deals double damage. ";
        base.StartBuffEffect(playerStats);

        isRemovable = false;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
    }

    public override Buff Clone()
    {
        Warrior_StrikeComboBuff1 clone = (Warrior_StrikeComboBuff1)this.MemberwiseClone();
        return clone;
    }
}
