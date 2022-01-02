using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffSlot : MonoBehaviour
{
    Buff buff;
    public Image icon;

    public void AddBuff(Buff newBuff)
    {
        buff = newBuff;             // Neuer Buff wird in den Slot gepackt
        icon.sprite = buff.icon;    // Icon wird aktualisiert
        icon.enabled = true;        // Icon wird angezeigt
    }

    public void ClearSlot()
    {
        buff = null;            // Item wird entfernt
        icon.sprite = null;     // Icon gelöscht
        icon.enabled = false;   // Kein Icon angezeigt
    }


    // Kann später verwendet werden um Buffs wegzuklicken, falls man sie nicht haben will.
    //public void UseItem()
    //{
    //    if (buff != null)
    //    {
    //        buff.Use(); // Wenn Button gedrückt und Item vorhanden, verwende Item
    //    }
    //}
}