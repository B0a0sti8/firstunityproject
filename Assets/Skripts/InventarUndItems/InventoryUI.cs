// Dieses Skript stellt die Br�cke zwischen Inventar Interface (in Game) und dem gecodeten Inventar dar.
// Aktualisiert Items graphisch und erm�glicht �ffnen/schlie�en
// Eng verbunden mit dem "InventorySlot"- und dem "Inventory"-Skript
// Verantwortlicher f�r den Quatsch: Basti

using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;

    Inventory inventory;        
    InventorySlot[] slots;      // Erstellt Array aus allen Inventarslots

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;         // Generiert Instanz des Inventars. Nachdem Inventar als Singleton definiert ist, kann es immer nur ein Inventar geben. Vorsicht!
        inventory.onItemChangedCallback += UpdateUI;            // Sobald sich etwas am Inventar �ndert (Callback aus "Inventar"-Skript) wird das UI geupdated
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    private void OnInventory(InputValue value)
    {
        Debug.Log("Inventar An/Aus");
        inventoryUI.SetActive(!inventoryUI.activeSelf);    
    }

    void UpdateUI ()        //Updated das UI
    {
        Debug.Log("Updating UI");
        for (int i = 0; i < slots.Length; i++)      // Geht alle Slots durch
        {
            if (i < inventory.items.Count)          // Solange die Z�hlvariable kleiner ist, als die Anzahl der Items im Inventar
            {
                slots[i].AddItem(inventory.items[i]);   // F�ge dem n�chsten Slot das n�chste Item hinzu
            }
            else                    // Wenn keine Items mehr �brig sind
            {
                slots[i].ClearSlot();       // Mach die �brigen Slots leer.
            }
        }

    }
}
