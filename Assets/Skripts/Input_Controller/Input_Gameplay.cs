using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Gameplay : MonoBehaviour
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
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

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
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    void OnMovement(InputValue value) // WASD
    { playerController.Movement(value); masterChecks.isSkillInterrupted = true; }

    void OnTakeDamage() // SPACE
    { playerStats.TakeDamageSpace(); }

    void OnAutoFocus() // TAB
    { interactionCharacter.AutoFocus(); }

    void OnZoom() // Scroll/Y
    { cameraFollow.CameraZoom(); }

    void OnChangeScene() // O
    { levelLoader.ChangeScene(); }

    void OnGoToHub() // H
    { levelLoader.GoToHub(); }

    void OnAddBag() // B
    {
        inventoryScript.CheatCodeAddBag();
        playerStats.CheatCodeAddXP();
        PLAYER.transform.Find("GameManager").GetComponent<SaveManager>().Save();
    }

    void OnLoadDebug() // V
    { PLAYER.transform.Find("GameManager").GetComponent<SaveManager>().Load(); Debug.Log("Load1"); }
}