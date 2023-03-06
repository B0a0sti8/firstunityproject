using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

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
        tickValue = 100f;

    }

    public override void StartBuffUI()
    {
        Debug.Log("Started Buff Effect");
        internTransferName = "HoTBuff";
        buffName = "Cure";
        buffDescription = "Gain <color=green>Healinge</color> over <color=yellow>Timere</color>. This is a verry long buff description. Oh yeaaah!";
        base.StartBuffUI();
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


    public override void UpdateEffect(CharacterStats playerStats)
    {
        base.UpdateEffect(playerStats);

        tickTimeElapsed += Time.deltaTime;
        if (tickTimeElapsed >= tickTime)
        {
            tickTimeElapsed = 0;

            DamageOrHealing.DoHealing(buffSource.GetComponent<NetworkBehaviour>(), playerStats.gameObject.GetComponent<NetworkBehaviour>(), tickValue);
        }
    }
}
