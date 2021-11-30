// Dieses Skript sorgt dafür dass bei Rechtsklick überprüft wird, ob auf ein Interaktives Objekt geklickt wurde
// z.B. Items, Truhen, Händler etc.
// Eng verbunden mit "Interactable"-Skript
// Verantwortlicher für den Quatsch: Basti

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionCharacter : MonoBehaviour // Sorry Marcus, ist echt nicht schön gecoded. xD
{
    Camera cam;
    public Interactable focus;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;      // Verpacke die Kamera in Variable
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)       // Wenn linke Maustaste gedrückt
        {//!!!!!!!!!!!!
            //RemoveFocus();      // Nichts mehr anvisieren (Händler, Gegner, Item etc. können anvisiert werden)
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)  // Wenn rechte Maustaste gedrückt
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero); // Die Mausposition (ursprünglich in Pixel) wird in Weltkoordinaten übersetzt (Unity transform z.B.)
                                                                                                                                    // Eine Linie zwischen der Position des Mauszeigers wird gebildet und dem Vektor (0,0) wird gebildet. Ist noch nicht ganz klar ob das der Ursprung der Map oder das Zentrum der Kamera ist.
            // Debug.DrawRay(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), new Vector3(10f, 10f, 0f), Color.blue, 20f); Anzeige für Referenz-Ray

            if (hit.collider != null)   // Wird geprüft ob überhaupt was getroffen wurde
            {
                 Interactable interactable = hit.collider.GetComponent<Interactable>();          // Gegenstand der getroffen wurde wird fokusiert. (Für spätere Interaktion)
                 SetFocus(interactable);
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
