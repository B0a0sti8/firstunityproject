// Dieses Skript sorgt daf�r dass bei Rechtsklick �berpr�ft wird, ob auf ein Interaktives Objekt geklickt wurde
// z.B. Items, Truhen, H�ndler etc.
// Eng verbunden mit "Interactable"-Skript
// Verantwortlicher f�r den Quatsch: Basti

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionCharacter : MonoBehaviour // Sorry Marcus, ist echt nicht sch�n gecoded. xD
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
        if (Mouse.current.leftButton.wasPressedThisFrame)       // Wenn linke Maustaste gedr�ckt
        {//!!!!!!!!!!!!
            //RemoveFocus();      // Nichts mehr anvisieren (H�ndler, Gegner, Item etc. k�nnen anvisiert werden)
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)  // Wenn rechte Maustaste gedr�ckt
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero); // Die Mausposition (urspr�nglich in Pixel) wird in Weltkoordinaten �bersetzt (Unity transform z.B.)
                                                                                                                                    // Eine Linie zwischen der Position des Mauszeigers wird gebildet und dem Vektor (0,0) wird gebildet. Ist noch nicht ganz klar ob das der Ursprung der Map oder das Zentrum der Kamera ist.
            // Debug.DrawRay(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), new Vector3(10f, 10f, 0f), Color.blue, 20f); Anzeige f�r Referenz-Ray

            if (hit.collider != null)   // Wird gepr�ft ob �berhaupt was getroffen wurde
            {
                 Interactable interactable = hit.collider.GetComponent<Interactable>();          // Gegenstand der getroffen wurde wird fokusiert. (F�r sp�tere Interaktion)
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
