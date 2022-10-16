using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Overlay : MonoBehaviour
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
        talentTreeUI = ownCanvases.transform.Find("Canvas TalentTree").gameObject.GetComponent<TalentTreeUI>();
        masterChecks = ownCanvases.Find("Canvas Action Skills").GetComponent<MasterChecks>();
        questLog = ownCanvases.Find("CanvasQuestUI").Find("QuestLog").GetComponent<QuestLog>();
    }
    #endregion

    void OnInventory() // I
    { inventoryScript.OpenClose(); }

    void OnEquipmentWindow() // C
    { characterPanelScript.OpenClose(); }

    void OnPauseMenu() // Esc
    { pauseMenu.OpenPauseMenu(); masterChecks.isSkillInterrupted = true; }

    void OnDPSMeterReset() // .
    { damageMeter.DPSMeterReset(); }

    void OnSkillbook() // K
    { skillbook.OpenSkillbook(); }

    void OnKeybindMenue() // N
    { keybindManager.OpenCloseMenue(); }

    void OnClassChoiceMenue() // X
    { classChoiceUI.OpenClassChoice(); }

    void OnTalentTreeMenue() // P
    { talentTreeUI.OpenTalentTree(); }

    void OnQuestWindow() // L
    { Debug.Log("QuestWindow"); questLog.OpenClose(); }
}