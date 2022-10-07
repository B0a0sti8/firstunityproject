// Dieses Skript beschreibt die Änderung eines einzelnen Inventarslots
// Zeigt z.B. die entsprechenden Items und ermöglicht Nutzung/Entfernen des entsprechenden Items
// Eng verbunden mit dem "InventoryUI"- und dem "Inventory"-Skript
// Verantwortlicher für den Quatsch: Basti

using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Item item;
    public Image icon;
    public Button removeButton;

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
        Inventory.instance.Remove(item);    // Wenn Button gedrückt, entferne Item
        Debug.Log("HI");
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use(); // Wenn Button gedrückt und Item vorhanden, verwende Item
        }
    }
}