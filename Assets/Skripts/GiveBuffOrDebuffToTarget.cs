using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public static class GiveBuffOrDebuffToTarget
{
    [ServerRpc]
    public static void GiveBuffOrDebuffServerRpc(NetworkBehaviourReference target, NetworkBehaviourReference source, string buffName, string buffImageName, bool isDamageOrHealing, float duration, float tickTime, float value)
    {
        GiveBuffOrDebuffClientRpc(target, source, buffName, buffImageName, isDamageOrHealing, duration, tickTime, value);
    }

    [ClientRpc]
    public static void GiveBuffOrDebuffClientRpc(NetworkBehaviourReference target, NetworkBehaviourReference source, string buffName, string buffImageName, bool isDamageOrHealing, float duration, float tickTime, float value)
    {
        target.TryGet<NetworkBehaviour>(out NetworkBehaviour tar);
        source.TryGet<NetworkBehaviour>(out NetworkBehaviour sor);

        var buff = BuffMasterManager.MyInstance.ListOfAllBuffs[buffName];
        Buff clone = buff.Clone();
        clone.buffSource = sor.gameObject;
        Sprite buffIcon = Resources.Load<Sprite>("BuffDebuffSprites/" + buffImageName);

        if (tar.CompareTag("Player"))
        {
            if (isDamageOrHealing)
            {
                tar.GetComponent<BuffManager>().AddBuff(clone, buffIcon, duration, tickTime, value);
            }
            else
            {
                tar.GetComponent<BuffManager>().AddBuff(clone, buffIcon, duration, value);
            }
        }
        else        // Falls es ein Enemy oder Friendly NPC ist.
        {
            if (isDamageOrHealing)
            {
                tar.GetComponent<BuffManagerNPC>().AddBuff(clone, buffIcon, duration, tickTime, value);
            }
            else
            {
                tar.GetComponent<BuffManagerNPC>().AddBuff(clone, buffIcon, duration, value);
            }
        }
    }
}
