using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Gameplay : MonoBehaviour
{
    #region
    GameObject PLAYER;
    GameObject ownCanvases;

    PlayerController playerController;
    PlayerStats playerStats;
    InteractionCharacter interactionCharacter;
    CameraFollow cameraFollow;
    LevelLoader levelLoader;

    void Awake()
    {
        PLAYER = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        ownCanvases = PLAYER.transform.Find("Own Canvases").gameObject;
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

        playerController = PLAYER.GetComponent<PlayerController>();
        playerStats = PLAYER.GetComponent<PlayerStats>();
        interactionCharacter = PLAYER.GetComponent<InteractionCharacter>();
        cameraFollow = PLAYER.GetComponent<CameraFollow>();
    }
    #endregion

    void OnMovement(InputValue value)
    { playerController.Movement(value); }

    void OnTakeDamage() // SPACE
    { playerStats.TakeDamageSpace(); }

    void OnAutoFocus() // TAB
    { interactionCharacter.AutoFocus(); }

    void OnZoom()
    { cameraFollow.CameraZoom(); }

    void OnChangeScene()
    { levelLoader.ChangeScene(); }

    void OnGoToHub()
    { levelLoader.GoToHub(); }
}