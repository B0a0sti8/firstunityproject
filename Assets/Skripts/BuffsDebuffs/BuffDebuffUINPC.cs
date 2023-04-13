using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BuffDebuffUINPC : NetworkBehaviour
{
    Transform buffsParent;

    BuffManagerNPC buffManager;
    BuffSlot[] slots;      // Erstellt Array aus allen Inventarslots

    void Awake()
    {
        buffsParent = transform.Find("BuffsParent");
        buffManager = transform.parent.parent.GetComponent<BuffManagerNPC>();         // Generiert Instanz der Buffleiste. Nachdem BuffManager als Singleton definiert ist, kann es immer nur einen BuffManager geben. Vorsicht!
        buffManager.onBuffsChangedCallback += UpdateUI;            // Sobald sich etwas an der Buffleiste ändert (Callback aus "BuffManager"-Skript) wird das UI geupdated
        slots = buffsParent.GetComponentsInChildren<BuffSlot>();
    }

    [ServerRpc]
    void UpdateUIServerRpc(NetworkBehaviourReference nBref, int slotNr, string buffName, string buffDescription, string buffSpriteName, float buffDur)
    {
        UpdateUIClientRpc(nBref, slotNr, buffName, buffDescription, buffSpriteName, buffDur);
    }

    [ClientRpc]
    void UpdateUIClientRpc(NetworkBehaviourReference nBref, int slotNr, string buffName, string buffDescription, string buffSpriteName, float buffDur)
    {
        nBref.TryGet<BuffDebuffUINPC>(out BuffDebuffUINPC UiWC);


        if (buffName == "")
        {
            UiWC.slots[slotNr].ClearSlot();
        }
        else
        {
            DummyBuffMultiplayer slotBuff = new DummyBuffMultiplayer();
            Buff clone = slotBuff.Clone();
            clone.buffName = buffName;
            clone.buffDescription = buffDescription;
            clone.icon = Resources.Load<Sprite>("BuffDebuffSprites/" + buffSpriteName);
            clone.duration = buffDur;
            clone.durationTimeLeft = buffDur;
            StartCoroutine(ReduceTime((DummyBuffMultiplayer)clone, 0.3f));


            UiWC.slots[slotNr].AddBuff(clone);
        }
    }

    public IEnumerator ReduceTime(DummyBuffMultiplayer buff, float tickTime)
    {
        while (buff.durationTimeLeft >= 0)
        {
            yield return new WaitForSeconds(tickTime);
            buff.StartTicking(tickTime);
        }
    }

    public void UpdateUI()        //Updated das UI
    {
        if (!IsOwner) { return; }

        if (!transform.parent.gameObject.activeSelf || !transform.parent.gameObject.activeInHierarchy)
        { return; }

        NetworkBehaviourReference nBref = this;
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < buffManager.buffs.Count)
            {
                string buffN = buffManager.buffs[i].buffName;
                string buffDes = buffManager.buffs[i].buffDescription;
                string buffSpN = buffManager.buffs[i].icon.name;
                float buffDur = buffManager.buffs[i].durationTimeLeft;
                UpdateUIServerRpc(nBref, i, buffN, buffDes, buffSpN, buffDur);
                Debug.Log("BuffSprite heißt: " +  buffSpN);
            }
            else
            {
                UpdateUIServerRpc(nBref, i, "", "", "", 0f);
            }
        }
    }
}
