using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Overlay : MonoBehaviour
{
    #region
    GameObject PLAYER;
    GameObject ownCanvases;

    InventoryUI inventoryUI;
    EquipmentWindowUI equipmentWindowUI;
    PauseMenu pauseMenu;
    DamageMeter damageMeter;
    SkillbookMaster skillbook;
    KeybindManager keybindManager;
    ClassChoiceUI classChoiceUI;
    TalentTreeUI talentTreeUI;
    MasterChecks masterChecks;

    void Awake()
    {
        PLAYER = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        ownCanvases = PLAYER.transform.Find("Own Canvases").gameObject;

        inventoryUI = ownCanvases.transform.Find("Canvas Inventory").gameObject.GetComponent<InventoryUI>();
        equipmentWindowUI = ownCanvases.transform.Find("Canvas Equipment").gameObject.GetComponent<EquipmentWindowUI>();
        pauseMenu = ownCanvases.transform.Find("Canvas Pause Menu").gameObject.GetComponent<PauseMenu>();
        damageMeter = ownCanvases.transform.Find("Canvas Damage Meter").gameObject.GetComponent<DamageMeter>();
        skillbook = ownCanvases.transform.Find("Canvas Skillbook").gameObject.GetComponent<SkillbookMaster>();
        keybindManager = GameObject.Find("GameManager").gameObject.GetComponent<KeybindManager>();
        classChoiceUI = ownCanvases.transform.Find("Canvas ClassChoice").gameObject.GetComponent<ClassChoiceUI>();
        talentTreeUI = ownCanvases.transform.Find("Canvas TalentTree").gameObject.GetComponent<TalentTreeUI>();
        masterChecks = PLAYER.transform.Find("Own Canvases").Find("Canvas Action Skills").GetComponent<MasterChecks>();
    }
    #endregion

    void OnInventory() // I
    { inventoryUI.OpenInventory(); }

    void OnEquipmentWindow() // C
    { equipmentWindowUI.OpenEquipmentWindow(); }

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
}