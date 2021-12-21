// Dieses Skript sorgt daf�r dass bei Rechtsklick �berpr�ft wird, ob auf ein Interaktives Objekt geklickt wurde
// z.B. Items, Truhen, H�ndler etc.
// Eng verbunden mit "Interactable"-Skript
// Verantwortlicher f�r den Quatsch: Basti

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractionCharacter : MonoBehaviour // Sorry Marcus, ist echt nicht sch�n gecoded. xD
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

        if (Mouse.current.rightButton.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)  // Wenn rechte Maustaste gedr�ckt
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero); // Die Mausposition (urspr�nglich in Pixel) wird in Weltkoordinaten �bersetzt (Unity transform z.B.)
                                                                                                                                    // Eine Linie zwischen der Position des Mauszeigers wird gebildet und dem Vektor (0,0) wird gebildet. Ist noch nicht ganz klar ob das der Ursprung der Map oder das Zentrum der Kamera ist.
            if (hit.collider != null) // Wird gepr�ft ob �berhaupt was getroffen wurde
            {
                 Interactable interactable = hit.collider.GetComponent<Interactable>(); // Gegenstand der getroffen wurde wird fokusiert. (F�r sp�tere Interaktion)
                 SetFocus(interactable);
            }
            else if(!EventSystem.current.IsPointerOverGameObject()) // Wenn Mauszeiger nicht �ber UI Element ist.
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