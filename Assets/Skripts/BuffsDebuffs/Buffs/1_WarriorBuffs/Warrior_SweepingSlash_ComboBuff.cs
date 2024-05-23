using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_SweepingSlash_ComboBuff : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Warrior_SweepingSlash_ComboBuff";
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
        Warrior_SweepingSlash_ComboBuff clone = (Warrior_SweepingSlash_ComboBuff)this.MemberwiseClone();
        return clone;
    }
}
