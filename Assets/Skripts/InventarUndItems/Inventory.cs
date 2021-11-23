// Dieses Skript sorgt daf�r, dass Items ins Inventar hinzugef�gt oder daraus entfernt werden k�nnen.
// Gibt au�erdem R�ckmeldung wenn das passiert, um z.B. das UI upzudaten
// Verantwortlicher f�r den Quatsch: Basti

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    #region Singleton

    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;      // Platz im Inventar

    public List<Item> items = new List<Item>();     // Liste in die Items eingetragen werden

    public bool Add (Item item)     // Funktion zum Hinzuf�gen eines Items
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= space)   // Falls Inventar voll ist wird R�ckmeldung gegeben
            {
                Debug.Log("Not enough room.");
                return false;
            }

            items.Add(item);            // Ansonsten Item aufnehmen
            if (onItemChangedCallback != null) 
            {
                onItemChangedCallback.Invoke();  // Triggert immer wenn Item hinzugef�gt oder entfernt wird. Nice f�r Update des UI.
            }
        }
        return true; 
    }

    public void Remove (Item item)  // Funktion zum Entfernen eines Items
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();     // Triggert immer wenn Item hinzugef�gt oder entfernt wird. Nice f�r Update des UI.
        }
    }
}
 