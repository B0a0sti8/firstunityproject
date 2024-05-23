using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_BrutalStrike_ComboBuff : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Warrior_BrutalStrike_ComboBuff";
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
        Warrior_BrutalStrike_ComboBuff clone = (Warrior_BrutalStrike_ComboBuff)this.MemberwiseClone();
        return clone;
    }
}
