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
using Unity.Netcode;

public class InteractionCharacter : NetworkBehaviour // Sorry Marcus, ist echt nicht schön gecoded. xD
{
    public Interactable focus;

    GameObject[] potentialEnemies;
    GameObject[] viableEnemies;
    float[] enemyDistances;
    float maxFocusRange = 20f;

    void Update()
    {
        if (!IsOwner) { return; }


        if (focus != null)
        {
            if (focus.gameObject.layer == LayerMask.NameToLayer("Enemy") || focus.gameObject.layer == LayerMask.NameToLayer("Action"))
            {
                if (!focus.GetComponent<CharacterStats>().isAlive.Value)
                {
                    focus = null;
                }
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)  // Wenn rechte Maustaste gedrückt
        {
            if (!EventSystem.current.IsPointerOverGameObject()) // Wenn Mauszeiger nicht über UI Element ist.
            {
                RemoveFocus();
            }

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero); // Die Mausposition (ursprünglich in Pixel) wird in Weltkoordinaten übersetzt (Unity transform z.B.)
                                                                                                                                    // Eine Linie zwischen der Position des Mauszeigers wird gebildet und dem Vektor (0,0) wird gebildet. Ist noch nicht ganz klar ob das der Ursprung der Map oder das Zentrum der Kamera ist.
            if (hit.collider != null)
            {
                Debug.Log("Hab Was");
                if (hit.collider.GetComponent<Interactable>() != null)
                {
                    Debug.Log("Ist Interactable, setze Focus");
                    SetFocus(hit.collider.GetComponent<Interactable>());
                }
            }


            //CheckIfHitInteractableServerRpc(this, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        }

        


        
    }

    //[ServerRpc]
    //public void CheckIfHitInteractableServerRpc(NetworkBehaviourReference characterBox, Vector2 origin, ServerRpcParams serverRpcParams = default)
    //{
    //    var clientId = serverRpcParams.Receive.SenderClientId;
    //    ClientRpcParams clientRpcParams = new ClientRpcParams
    //    {
    //        Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { clientId } }
    //    };

    //    NetworkBehaviourReference interactable;
    //    interactable = characterBox;
    //    DebugLogClientRpc("ServerRpc", clientRpcParams);
    //    RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero); // Die Mausposition (ursprünglich in Pixel) wird in Weltkoordinaten übersetzt (Unity transform z.B.)
    //                                                                // Eine Linie zwischen der Position des Mauszeigers wird gebildet und dem Vektor (0,0) wird gebildet. Ist noch nicht ganz klar ob das der Ursprung der Map oder das Zentrum der Kamera ist.
    //    DebugLogClientRpc(origin.ToString());
    //    if (hit.collider != null) // Wird geprüft ob überhaupt was getroffen wurde
    //    {
    //        DebugLogClientRpc("Hab was!!", clientRpcParams);
    //        if (hit.collider.GetComponent<Interactable>() != null)
    //        {
    //            DebugLogClientRpc("Ist ein Interactable", clientRpcParams);
    //            interactable = hit.collider.GetComponent<NetworkBehaviourReference>(); // Gegenstand der getroffen wurde wird fokusiert. (Für spätere Interaktion)
    //        }
    //    }

    //    CheckIfHitInteractableClientRpc(interactable, clientRpcParams);
    //}

    //[ClientRpc]
    //public void CheckIfHitInteractableClientRpc(NetworkBehaviourReference interactable, ClientRpcParams clientRpcParams = default)
    //{
    //    Debug.Log("ClientRpc");
    //    interactable.TryGet<Interactable>(out Interactable interactableFocus);
    //    SetFocus(interactableFocus);
    //}

    //[ClientRpc]
    //public void DebugLogClientRpc(string message, ClientRpcParams clientRpcParams = default)
    //{
    //    Debug.Log(message);
    //}

    //RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero); // Die Mausposition (ursprünglich in Pixel) wird in Weltkoordinaten übersetzt (Unity transform z.B.)
    //        foreach(RaycastHit2D h in hits)                                                                                                                        // Eine Linie zwischen der Position des Mauszeigers wird gebildet und dem Vektor (0,0) wird gebildet. Ist noch nicht ganz klar ob das der Ursprung der Map oder das Zentrum der Kamera ist.
    //        {
    //            Debug.Log(h);
    //            if (h.collider != null)
    //            {
    //                Debug.Log("Found Collider");
    //                Interactable inter = h.collider.GetComponent<Interactable>();
    //                if (inter != null)
    //                {
    //                    Debug.Log("Found Interactable, break");
    //                    SetFocus(inter);
    //                    break;
    //                }
    //            }
    //        }

    public void SetFocus (Interactable newFocus) 
    {
        if (newFocus == null)
        { return; }

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
                (newFocus as Vendor).window = transform.Find("Own Canvases").Find("CanvasVendorWindow").Find("VendorWindow").GetComponent<VendorWindow>();
            }
            else if (newFocus is QuestGiver)
            {
                (newFocus as QuestGiver).window = transform.Find("Own Canvases").Find("CanvasQuestUI").Find("QuestGiverWindow").GetComponent<QuestGiverWindow>();
            }
        }
        
        if (newFocus != null)
        {
            newFocus.OnFocused(transform);
        }
    }

    void RemoveFocus () // Entfernt Fokus des Charakters
    {
        if (!IsOwner) { return; }

        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
    }

    public void AutoFocus() // TAB
    {
        if (!IsOwner) { return; }

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
            if (enemyDist <= maxFocusRange && potE.GetComponent<EnemyStats>().isAlive.Value == true && focus != potE.gameObject.GetComponent<Interactable>()) // does NOT check if in sight
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