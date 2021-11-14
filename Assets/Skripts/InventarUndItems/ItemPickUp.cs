// Dieses Skript sorgt daf�r, dass Items mit Rechtsklick aufgehoben werden k�nnen
// Eng verbunden mit "Interactable"-Skript
// Verantwortlicher f�r den Quatsch: Basti

using UnityEngine;

public class ItemPickUp : Interactable {

    public Item item;

    public override void Interact() // �bernimmt Interaktionsmethode von Interactable
    {
        base.Interact();

        PickUp(); // Zus�tzlich zu den bisherigen Funktionen wird PickUp angewendet
    }

    void PickUp()   // Versucht Item aufzuheben
    {
        Debug.Log("Picking Up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);        // Checkt ob erfolgreich (aka habe ich genug Platz im Inventar)

        if (wasPickedUp)
        {
            Destroy(gameObject);        // Zerst�rt Item am Boden
        }
        
    }

}