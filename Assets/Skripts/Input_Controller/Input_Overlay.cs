using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Input_Overlay : NetworkBehaviour
{
    #region
    GameObject PLAYER;
    Transform ownCanvases;

    InventoryScript inventoryScript;
    CharacterPanelScript characterPanelScript;
    PauseMenu pauseMenu;
    DamageMeter damageMeter;
    SkillbookMaster skillbook;
    KeybindManager keybindManager;
    ClassChoiceUI classChoiceUI;
    TalentTreeUI talentTreeUI;
    MasterChecks masterChecks;
    QuestLog questLog;

    void Awake()
    {
        PLAYER = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        ownCanvases = PLAYER.transform.Find("Own Canvases");

        inventoryScript = ownCanvases.transform.Find("Canvas Inventory").Find("Inventory").GetComponent<InventoryScript>();
        characterPanelScript = ownCanvases.transform.Find("CanvasCharacterPanel").Find("CharacterPanel").gameObject.GetComponent<CharacterPanelScript>();
        pauseMenu = ownCanvases.transform.Find("Canvas Pause Menu").gameObject.GetComponent<PauseMenu>();
        damageMeter = ownCanvases.transform.Find("Canvas Damage Meter").gameObject.GetComponent<DamageMeter>();
        skillbook = ownCanvases.transform.Find("Canvas Skillbook").gameObject.GetComponent<SkillbookMaster>();
        keybindManager = GameObject.Find("GameManager").gameObject.GetComponent<KeybindManager>();
        classChoiceUI = ownCanvases.transform.Find("Canvas ClassChoice").gameObject.GetComponent<ClassChoiceUI>();
        talentTreeUI = ownCanvases.transform.Find("CanvasTalentTree").gameObject.GetComponent<TalentTreeUI>();
        masterChecks = ownCanvases.Find("Canvas Action Skills").GetComponent<MasterChecks>();
        questLog = ownCanvases.Find("CanvasQuestUI").Find("QuestLog").GetComponent<QuestLog>();
    }
    #endregion

    void OnInventory() // I
    { if (IsOwner) { inventoryScript.OpenClose(); } }

    void OnEquipmentWindow() // C
    { if (IsOwner) { characterPanelScript.OpenClose(); } }

    void OnPauseMenu() // Esc
    { if (IsOwner) { pauseMenu.OpenPauseMenu(); masterChecks.isSkillInterrupted = true; } }

    void OnDPSMeterReset() // .
    { if (IsOwner) { damageMeter.DPSMeterReset(); } }

    void OnSkillbook() // K
    { if (IsOwner) { skillbook.OpenSkillbook(); } }

    void OnKeybindMenue() // N
    { if (IsOwner) { keybindManager.OpenCloseMenue(); } }

    void OnClassChoiceMenue() // X
    { if (IsOwner) { classChoiceUI.OpenClassChoice(); } }

    void OnTalentTreeMenue() // P
    { if (IsOwner) { talentTreeUI.OpenTalentTree(); } }

    void OnQuestWindow() // L
    { if (IsOwner) { questLog.OpenClose(); } }
}