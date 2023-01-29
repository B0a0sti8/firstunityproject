using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HoTBuff : Buff
{
    //new public bool isOverTime = true;

    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Cure";
        buffDescription = "Gain <color=green>Healinge</color> over <color=yellow>Timere</color>. This is a verry long buff description. Oh yeaaah!";
        base.StartBuffEffect(playerStats);
        tickTimeElapsed = 0;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
    }

    public override Buff Clone()
    {
        HoTBuff clone = (HoTBuff)this.MemberwiseClone();
        return clone;
    }

    public override void Update(CharacterStats playerStats)
    {
        base.Update(playerStats);
        tickTimeElapsed += Time.deltaTime;
        if (tickTimeElapsed >= tickTime)
        {
            tickTimeElapsed = 0;
            DamageOrHealing.DoHealing(buffSource, playerStats.gameObject, tickValue);
        }
    }
}
