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
    }
    #endregion

    void OnInventory()
    { inventoryUI.OpenInventory(); }

    void OnEquipmentWindow()
    { equipmentWindowUI.OpenEquipmentWindow(); }

    void OnPauseMenu()
    { pauseMenu.OpenPauseMenu(); }

    void OnDPSMeterReset()
    { damageMeter.DPSMeterReset(); }

    void OnSkillbook()
    { skillbook.OpenSkillbook(); }

    void OnKeybindMenue()
    { keybindManager.OpenCloseMenue(); }
}