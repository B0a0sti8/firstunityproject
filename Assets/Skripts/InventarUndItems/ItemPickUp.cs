// Dieses Skript sorgt dafür, dass Items mit Rechtsklick aufgehoben werden können
// Eng verbunden mit "Interactable"-Skript
// Verantwortlicher für den Quatsch: Basti

using UnityEngine;

public class ItemPickUp : Interactable {

    public Item item;

    public override void Interact() // Übernimmt Interaktionsmethode von Interactable
    {
        base.Interact();

        PickUp(); // Zusätzlich zu den bisherigen Funktionen wird PickUp angewendet
    }

    void PickUp()   // Versucht Item aufzuheben
    {
        Debug.Log("Picking Up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);        // Checkt ob erfolgreich (aka habe ich genug Platz im Inventar)

        if (wasPickedUp)
        {
            Destroy(gameObject);        // Zerstört Item am Boden
        }
        
    }

}