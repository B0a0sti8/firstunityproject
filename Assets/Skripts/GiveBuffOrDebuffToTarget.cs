using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public static class GiveBuffOrDebuffToTarget
{
    public static void GiveBuffOrDebuff(NetworkBehaviourReference target, NetworkBehaviourReference source, string buffName, string buffImageName, bool isDamageOrHealing, float duration, float tickTime, float value)
    {
        target.TryGet<NetworkBehaviour>(out NetworkBehaviour tar);
        

        if (tar.gameObject.CompareTag("Player"))
        {
            tar.gameObject.GetComponent<BuffManager>().AddBuffProcedure(target, source, buffName, buffImageName, isDamageOrHealing, duration, tickTime, value);
        }
        else        // Falls es ein Enemy oder Friendly NPC ist.
        {
            if (isDamageOrHealing)
            {
                Debug.Log("Gegnern Kann noch kein Buff gegeben werden, siehe hier");
                //tar.gameObject.GetComponent<BuffManagerNPC>().AddBuffProcedure(target, source, buffName, buffImageName, isDamageOrHealing, duration, tickTime, value);
            }
            else
            {
                Debug.Log("Gegnern Kann noch kein Buff gegeben werden, siehe hier");
                //tar.gameObject.GetComponent<BuffManagerNPC>().AddBuffProcedure(target, source, buffName, buffImageName, isDamageOrHealing, duration, tickTime, value);
            }
        }
    }
}
