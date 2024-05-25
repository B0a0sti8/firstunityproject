using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Input_Gameplay : NetworkBehaviour
{
    #region
    Transform PLAYER;
    Transform ownCanvases;

    PlayerController playerController;
    PlayerStats playerStats;
    InteractionCharacter interactionCharacter;
    CameraFollow cameraFollow;
    LevelLoader levelLoader;
    MasterChecks masterChecks;
    InventoryScript inventoryScript;

    void Awake()
    {
        PLAYER = transform.parent.parent;
        ownCanvases = PLAYER.transform.Find("Own Canvases");
        // levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

        playerController = PLAYER.GetComponent<PlayerController>();
        playerStats = PLAYER.GetComponent<PlayerStats>();
        interactionCharacter = PLAYER.GetComponent<InteractionCharacter>();
        cameraFollow = PLAYER.GetComponent<CameraFollow>();
        masterChecks = ownCanvases.Find("Canvas Action Skills").GetComponent<MasterChecks>();

        inventoryScript = ownCanvases.Find("Canvas Inventory").Find("Inventory").GetComponent<InventoryScript>();
    }
    #endregion

    void Update()
    {
        // levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    void OnMovement(InputValue value) // WASD
    { if (IsOwner) { playerController.Movement(value); masterChecks.isSkillInterrupted = true; } }

    void OnTakeDamage() // SPACE
    { if (IsOwner) { playerStats.TakeDamageSpace(); } }

    void OnAutoFocus() // TAB
    { if (IsOwner) { interactionCharacter.AutoFocus(); } }

    void OnZoom() // Scroll/Y
    { if (IsOwner) { cameraFollow.CameraZoom(); } }

    //void OnChangeScene() // O
    //{ levelLoader.ChangeScene(); }

    void OnGoToHub() // H
    { if (IsOwner) { levelLoader.GoToHub(); } }

    void OnAddBag() // B
    {
        if (!IsOwner) { return; }

        inventoryScript.CheatCodeAddBag();
        playerStats.CheatCodeAddXP();
        //PLAYER.transform.Find("GameManager").GetComponent<PlayerSaveLoad>().Save();
    }

    // Nur zum Debuggen. Wird Später auf nen Button im Hauptmenü gelegt oder so
    //void OnLoadDebug() // V
    //{ if (IsOwner) { PLAYER.transform.Find("GameManager").GetComponent<PlayerSaveLoad>().Load(); Debug.Log("Load1"); } }
}