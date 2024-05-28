using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_StingComboBuff1 : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Warrior_StingComboBuff1";
        buffDescription = "";
        base.StartBuffEffect(playerStats);

        isRemovable = false;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
    }

    public override Buff Clone()
    {
        Warrior_StingComboBuff1 clone = (Warrior_StingComboBuff1)this.MemberwiseClone();
        return clone;
    }
}
