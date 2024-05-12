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

    private PlayerTargetInfoUI myTargetUI;

    private void Start()
    {
        myTargetUI = transform.Find("Own Canvases").Find("CanvasHealth").Find("TargetHealth").GetComponent<PlayerTargetInfoUI>();
    }

    void Update()
    {
        if (!IsOwner) { return; }

        if(EventSystem.current.IsPointerOverGameObject()) return;

        if (true)
        {

        }

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

    public void SetFocus (Interactable newFocus) 
    {
        if (newFocus == null)
        { return; }

        if (newFocus != focus)
        {
            if (focus != null)
            {
                focus.OnDefocused(); // Legt Fokus des Charakters auf getroffenen interaktiven Gegenstand
                RemoveFocus();
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

        myTargetUI.UpdateMyUI();

    }

    void RemoveFocus () // Entfernt Fokus des Charakters
    {
        if (!IsOwner) { return; }

        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;

        myTargetUI.OnTargetLost();
        //myTargetUI.UpdateMyUI();
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

    public void GetCurrentTargetForMultiplayer()
    {
        GetCurrentTargetForMultiplayerServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void GetCurrentTargetForMultiplayerServerRpc()
    {
        GetCurrentTargetForMultiplayerClientRpc();
    }

    [ClientRpc]
    public void GetCurrentTargetForMultiplayerClientRpc()
    {
        if (!IsOwner) return;
        Debug.Log("Ich bin der owner. Aktualisiere Fokus.");
        SendCurrentTargetForMultiplayerServerRpc(focus.gameObject.GetComponent<NetworkObject>());
    }

    [ServerRpc]
    public void SendCurrentTargetForMultiplayerServerRpc(NetworkObjectReference myCurrentFocusRef)
    {
        Debug.Log("Ich bin der server. Aktualisiere Fokus.");
        SendCurrentTargetForMultiplayerClientRpc(myCurrentFocusRef);
    }

    [ClientRpc]
    public void SendCurrentTargetForMultiplayerClientRpc(NetworkObjectReference myCurrentFocusRef)
    {
        if (IsOwner) return;
        Debug.Log("Das sollten alle außer der Owner kriegen.");
        myCurrentFocusRef.TryGet(out NetworkObject myCurrentFocus);
        focus = myCurrentFocus.gameObject.GetComponent<Interactable>();
    }
}