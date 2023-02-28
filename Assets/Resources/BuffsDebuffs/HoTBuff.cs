using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoTBuff : Buff
{
    public new string internTransferName = "HoTBuff";
    //new public bool isOverTime = true;

    public override void InitializeBuff(GameObject source)
    {
        internTransferName = "HoTBuff";
        buffName = "Cure";
        buffDescription = "Gain <color=green>Healinge</color> over <color=yellow>Timere</color>. This is a verry long buff description. Oh yeaaah!";
        buffSource = source;
        tickValue = 10f;

    }

    public override void StartBuffEffect(CharacterStats playerStats)
    {
        Debug.Log("Started Buff Effect");
        internTransferName = "HoTBuff";
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
            Debug.Log(buffSource);
            Debug.Log(playerStats.gameObject);

            DamageOrHealing.DoHealing(buffSource, playerStats.gameObject, tickValue);
        }
    }
}
