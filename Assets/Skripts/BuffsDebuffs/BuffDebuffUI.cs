 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffUI : MonoBehaviour
{
    public Transform buffsParent;

    BuffManager buffManager;
    BuffSlot[] slots;      // Erstellt Array aus allen Inventarslots

    void Start()
    {
        buffManager = transform.parent.parent.GetComponent<BuffManager>();         // Generiert Instanz der Buffleiste. Nachdem BuffManager als Singleton definiert ist, kann es immer nur einen BuffManager geben. Vorsicht!
        buffManager.onBuffsChangedCallback += UpdateUI;            // Sobald sich etwas an der Buffleiste �ndert (Callback aus "BuffManager"-Skript) wird das UI geupdated
        slots = buffsParent.GetComponentsInChildren<BuffSlot>();
    }

    void UpdateUI()        //Updated das UI
    {
        for (int i = 0; i < slots.Length; i++)      // Geht alle Slots durch
        {
            if (i < buffManager.buffs.Count)          // Solange die Z�hlvariable kleiner ist, als die Anzahl der Buffs
            {
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
}