// Dieses Skript beschreibt die Änderung eines einzelnen Equipmentslots
// Zeigt z.B. die entsprechenden Equipments und ermöglicht Nutzung/Entfernen des entsprechenden Equipments
// Eng verbunden mit dem "EquipmentWindowUI"- und dem "EquipmentManager"-Skript
// Verantwortlicher für den Quatsch: Basti

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EquipmentWindowSlot : MonoBehaviour
{
    Item item;
    public Image icon;
    public Button removeButton;
    string objectName;
    public Transform equipSlotParent;
    EquipmentWindowSlot[] eSlots;
    EquipmentManager equipmentManager;
    
    

    private void Start()
    {
        equipmentManager = EquipmentManager.instance;
        eSlots = equipSlotParent.GetComponentsInChildren<EquipmentWindowSlot>();
    }

    public void AddItem(Item newItem)
    {
        item = newItem; // Neues Item wird in den Slot gepackt
        icon.sprite = item.MyIcon;    // Icon wird aktualisiert
        icon.enabled = true;        // Icon wird angezeigt
        removeButton.interactable = true;   // Der Button zum Entfernen des Items wird nutzbar
    }

    public void ClearSlot()
    {
        item = null;        // Item wird entfernt
        icon.sprite = null; // Icon gelöscht
        icon.enabled = false;   // Kein Icon angezeigt
        removeButton.interactable = false; // Und der Button ist nicht mehr nutzbar
    }

    public void OnRemoveButton()
    {
        objectName = gameObject.name;

        for (int i = 0; i < eSlots.Length; i++)
        {
            if (objectName == eSlots[i].name)
            {
                Debug.Log(eSlots[i].name);
                int index = i;
                EquipmentManager.instance.Unequip(index);
                break;
            }  
        }



        // Item in entsprechendem Slot soll entfernt werden. Dazu:
        // 
        // Mach 





        //Inventory.instance.Remove(item);    // Wenn Button gedrückt, entferne Item
    }
}