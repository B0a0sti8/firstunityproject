// Dieses Skript stellt die Brücke zwischen Inventar Interface (in Game) und dem gecodeten Inventar dar.
// Aktualisiert Items graphisch und ermöglicht öffnen/schließen
// Eng verbunden mit dem "InventorySlot"- und dem "Inventory"-Skript
// Verantwortlicher für den Quatsch: Basti

using UnityEngine;
using UnityEngine.InputSystem;

public class ClassChoiceUI : MonoBehaviour
{
    //public Transform itemsParent;
    public GameObject classChoiceUI;

    //Inventory inventory;
    //InventorySlot[] slots;      // Erstellt Array aus allen Inventarslots

    // Start is called before the first frame update
    void Start()
    {
        classChoiceUI = gameObject.transform.Find("ClassChoice").gameObject;
        //inventory = Inventory.instance;         // Generiert Instanz des Inventars. Nachdem Inventar als Singleton definiert ist, kann es immer nur ein Inventar geben. Vorsicht!
        //inventory.onItemChangedCallback += UpdateUI;            // Sobald sich etwas am Inventar ändert (Callback aus "Inventar"-Skript) wird das UI geupdated
        //slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    public void OpenClassChoice()
    {
        Debug.Log("Class Choice An/Aus");
        classChoiceUI.SetActive(!classChoiceUI.activeSelf);
    }

    public void ApplyClass()
    {
        classChoiceUI.SetActive(!classChoiceUI.activeSelf);
    }

    //void UpdateUI()        //Updated das UI
    //{}
}