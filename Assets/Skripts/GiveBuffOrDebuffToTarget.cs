using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public static class GiveBuffOrDebuffToTarget
{
    public static void GiveBuffOrDebuff(NetworkObjectReference target, NetworkObjectReference source, string buffName, string buffImageName, bool hasTicks, float duration, float tickTime, float value, float additionalValue1=0, float additionalValue2=0, float additionalValue3=0)
    {
        target.TryGet(out NetworkObject tar);
        //Debug.Log("Giving Buff: " + buffName + " to " + target);

        if (tar.gameObject.CompareTag("Player"))
        {
            tar.gameObject.GetComponent<BuffManager>().AddBuffProcedure(target, source, buffName, buffImageName, hasTicks, duration, tickTime, value, additionalValue1, additionalValue2, additionalValue3);
            //Debug.Log("Giving Buff to Player");
        }
        else        // Falls es ein Enemy oder Friendly NPC ist.
        {
            tar.gameObject.GetComponent<BuffManagerNPC>().AddBuffProcedure(target, source, buffName, buffImageName, hasTicks, duration, tickTime, value, additionalValue1, additionalValue2, additionalValue3);
            //Debug.Log("Giving Buff to NPC");
        }
    }
}
