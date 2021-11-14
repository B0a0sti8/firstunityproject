// Dieses Skript sorgt dafür, dass im Projekt Verzeichnis neue Objekte generiert werden können:
// Rechtsklick -> Create -> Inventory -> Item
// Diese übernehmen die unten stehenden Parameter
// Verantwortlicher für den Quatsch: Basti

using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {       // Statt Monobehaviour (übernimmt Sachen von Objekt dem das Skript zugewiesen ist): ScriptableObjekt

    new public string name = "New Item";     // Bisherige Definiton des Namens wird überschrieben
    public Sprite icon = null;               // Item Sprite 
    public bool isDefaultItem = false;       // Zusätzlicher möglicher Unterscheidungsparameter. z.B. keine Default Items ins Inventar oder ähnliches.

    public virtual void Use ()               // Wird überschrieben, je nach Itemsorte.
    {
        // Item verwenden.
        // Es könnte etwas passieren (je nach Item)

        Debug.Log("Using " + name);
    }
}
