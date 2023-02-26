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
        buffManager.onBuffsChangedCallback += UpdateUIStart;            // Sobald sich etwas an der Buffleiste �ndert (Callback aus "BuffManager"-Skript) wird das UI geupdated
        slots = buffsParent.GetComponentsInChildren<BuffSlot>();
    }

    [ServerRpc]
    void UpdateUIServerRpc(NetworkBehaviourReference nBref) 
    {
        Debug.Log("ServerRPC Update");
        UpdateUIClientRpc(nBref);
    }

    [ClientRpc]
    void UpdateUIClientRpc(NetworkBehaviourReference nBref)
    {
        Debug.Log("ClientRPc Update");
        nBref.TryGet<BuffDebuffUIWorldCanv>(out BuffDebuffUIWorldCanv UiWC);
        UiWC.UpdateUI(); 
    }

    void UpdateUI()
    {
        Debug.Log("Update Buffs");
        for (int i = 0; i < slots.Length; i++)      // Geht alle Slots durch
        {
            Debug.Log(i);
            if (i < buffManager.buffs.Count)          // Solange die Z�hlvariable kleiner ist, als die Anzahl der Buffs
            {
                Debug.Log(i);
                slots[i].AddBuff(buffManager.buffs[i]);   // F�ge dem n�chsten Slot den n�chsten Buff hinzu
                slots[i].buffName = buffManager.buffs[i].buffName;
                slots[i].buffDescription = buffManager.buffs[i].buffDescription;
            }
            else                    // Wenn keine Buffs mehr �brig sind
            {
                slots[i].ClearSlot();       // Mach die �brigen Slots leer.
                slots[i].buffName = "";
                slots[i].buffDescription = "";
            }
        }
    }


    public void UpdateUIStart()        //Updated das UI
    {
        if (!IsOwner) { return; }

        Debug.Log("Starte Update");
        NetworkBehaviourReference nBref = this;
        UpdateUIServerRpc(nBref);
    }
}