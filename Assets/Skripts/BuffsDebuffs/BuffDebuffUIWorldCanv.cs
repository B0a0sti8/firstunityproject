using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BuffDebuffUIWorldCanv : NetworkBehaviour
{
    Transform buffsParent;

    BuffManager buffManager;
    BuffSlot[] slots;      // Erstellt Array aus allen Inventarslots

    void Awake()
    {
        buffsParent = transform.Find("BuffsParent");
        buffManager = transform.parent.parent.GetComponent<BuffManager>();         // Generiert Instanz der Buffleiste. Nachdem BuffManager als Singleton definiert ist, kann es immer nur einen BuffManager geben. Vorsicht!
        buffManager.onBuffsChangedCallback += UpdateUIStart;            // Sobald sich etwas an der Buffleiste ändert (Callback aus "BuffManager"-Skript) wird das UI geupdated
        slots = buffsParent.GetComponentsInChildren<BuffSlot>();
    }

    [ServerRpc]
    void AddBuffServerRpc(string title, string description, string SpriteName, float duration)
    {
        //Buff newBuf = new Buff;

        //newBuf.duration = duration;
    }

    [ClientRpc]
    void AddBuffClientRpc()
    {

    }










    [ServerRpc]
    void UpdateUIServerRpc(NetworkBehaviourReference nBref, int slotNr, string buffName, string buffDescription, string buffSpriteName, float buffDur) 
    {
        Debug.Log("ServerRPC Update");
        UpdateUIClientRpc(nBref, slotNr, buffName, buffDescription, buffSpriteName, buffDur);
    }

    [ClientRpc]
    void UpdateUIClientRpc(NetworkBehaviourReference nBref, int slotNr, string buffName, string buffDescription, string buffSpriteName, float buffDur)
    {
        Debug.Log("ClientRPc Update");
        nBref.TryGet<BuffDebuffUIWorldCanv>(out BuffDebuffUIWorldCanv UiWC);

        Debug.Log(buffSpriteName);
        Debug.Log(buffDur);

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

    public void UpdateUIStart()        //Updated das UI
    {
        if (!IsOwner) { return; }

        NetworkBehaviourReference nBref = this;
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < buffManager.buffs.Count)
            {
                string buffN = buffManager.buffs[i].buffName;
                string buffDes = buffManager.buffs[i].buffDescription;
                string buffSpN = buffManager.buffs[i].icon.name;
                float buffDur = buffManager.buffs[i].duration;
                UpdateUIServerRpc(nBref, i, buffN, buffDes, buffSpN, buffDur);
            }
            else
            {
                UpdateUIServerRpc(nBref, i, "", "", "", 0f);
            }
        }
    }
}