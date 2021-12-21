// Dieses Skript sorgt dafür dass bei Rechtsklick überprüft wird, ob auf ein Interaktives Objekt geklickt wurde
// z.B. Items, Truhen, Händler etc.
// Eng verbunden mit "Interactable"-Skript
// Verantwortlicher für den Quatsch: Basti

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        }
        newFocus.OnFocused(transform);
    }

    void RemoveFocus ()             // Entfernt Fokus des Charakters
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
    }
}