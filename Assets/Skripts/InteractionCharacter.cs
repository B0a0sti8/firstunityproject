// Dieses Skript sorgt dafür dass bei Rechtsklick überprüft wird, ob auf ein Interaktives Objekt geklickt wurde
// z.B. Items, Truhen, Händler etc.
// Eng verbunden mit "Interactable"-Skript
// Verantwortlicher für den Quatsch: Basti <3

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System; // for array

public class InteractionCharacter : MonoBehaviour // Sorry Marcus, ist echt nicht schön gecoded. xD
{
    public Interactable focus;

    void Update()
    {
        if (focus != null)
        {
            if (focus.gameObject.layer == LayerMask.NameToLayer("Enemy") || focus.gameObject.layer == LayerMask.NameToLayer("Action"))
            {
                if (!focus.GetComponent<CharacterStats>().isAlive)
                {
                    focus = null;
                }
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)  // Wenn rechte Maustaste gedrückt
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero); // Die Mausposition (ursprünglich in Pixel) wird in Weltkoordinaten übersetzt (Unity transform z.B.)
                                                                                                                                    // Eine Linie zwischen der Position des Mauszeigers wird gebildet und dem Vektor (0,0) wird gebildet. Ist noch nicht ganz klar ob das der Ursprung der Map oder das Zentrum der Kamera ist.
            if (hit.collider != null) // Wird geprüft ob überhaupt was getroffen wurde
            {
                 Interactable interactable = hit.collider.GetComponent<Interactable>(); // Gegenstand der getroffen wurde wird fokusiert. (Für spätere Interaktion)
                 SetFocus(interactable);
            }
            else if(!EventSystem.current.IsPointerOverGameObject()) // Wenn Mauszeiger nicht über UI Element ist.
            {
                RemoveFocus();
            }
        }
    }

    void SetFocus (Interactable newFocus) 
    {
        if (newFocus != focus)
        {
            if (focus != null)
            {
                focus.OnDefocused(); // Legt Fokus des Charakters auf getroffenen interaktiven Gegenstand
            }
            focus = newFocus;

            if (newFocus is StorageChest)
            {
                (newFocus as StorageChest).canvasGroup = transform.Find("Own Canvases").Find("CanvasStorageChest").Find("StorageChest").GetComponent<CanvasGroup>();
            }
            else if (newFocus is Vendor)
            {
                (newFocus as Vendor).vendorWindow = transform.Find("Own Canvases").Find("CanvasVendorWindow").Find("VendorWindow").GetComponent<VendorWindow>();
            }
        }
        newFocus.OnFocused(transform);

    }

    void RemoveFocus () // Entfernt Fokus des Charakters
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
    }



    GameObject[] potentialEnemies;
    GameObject[] viableEnemies;
    float[] enemyDistances;
    float maxFocusRange = 20f;

    public void AutoFocus() // TAB
    {
        potentialEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (potentialEnemies.Length == 0) { return; }

        viableEnemies = new GameObject[potentialEnemies.Length];

        enemyDistances = new float[potentialEnemies.Length];
        for (int i = 0; i < potentialEnemies.Length; i++)
        {
            enemyDistances[i] = Mathf.Infinity;
        }

        int n = 0;
        foreach (GameObject potE in potentialEnemies)
        {
            float enemyDist = Vector2.Distance(gameObject.transform.position, potE.transform.position);
            if (enemyDist <= maxFocusRange && potE.GetComponent<EnemyStats>().isAlive == true && focus != potE.gameObject.GetComponent<Interactable>()) // does NOT check if in sight
            {
                viableEnemies[n] = potE;
                enemyDistances[n] = enemyDist;
                n += 1;
            }
        }

        if (viableEnemies[0] != null)
        {
            int minIndex = Array.IndexOf(enemyDistances, Mathf.Min(enemyDistances));
            Interactable closestEnemy = viableEnemies[minIndex].gameObject.GetComponent<Interactable>();
            SetFocus(closestEnemy);
        }
    }
}