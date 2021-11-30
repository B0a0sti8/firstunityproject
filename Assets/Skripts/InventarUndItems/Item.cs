// Dieses Skript sorgt daf?r, dass im Projekt Verzeichnis neue Objekte generiert werden k?nnen:
// Rechtsklick -> Create -> Inventory -> Item
// Diese ?bernehmen die unten stehenden Parameter
// Verantwortlicher f?r den Quatsch: Basti

using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {       // Statt Monobehaviour (?bernimmt Sachen von Objekt dem das Skript zugewiesen ist): ScriptableObjekt

    new public string name = "New Item";     // Bisherige Definiton des Namens wird ?berschrieben
    public Sprite icon = null;               // Item Sprite 
    public bool isDefaultItem = false;       // Zus?tzlicher m?glicher Unterscheidungsparameter. z.B. keine Default Items ins Inventar oder ?hnliches.

    public virtual void Use ()               // Wird ?berschrieben, je nach Itemsorte.
    {
        // Item verwenden.
        // Es k?nnte etwas passieren (je nach Item)

        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}
