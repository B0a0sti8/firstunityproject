using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharPanelButtonScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EquipmentType equipType;
    [SerializeField] private Image icon;

    [SerializeField] private Equipment equip;
    public bool onRemoved = false;

    MasterEventTriggerItems masterETItems;
    PlayerStats playerStats;
    HandScript myHandScript;
    CharacterPanelScript myCharPanel;

    public Equipment MyEquip { get => equip; }

    private void Awake()
    {
        masterETItems = GetComponent<MasterEventTriggerItems>();
        playerStats = transform.parent.parent.parent.parent.parent.GetComponent<PlayerStats>();
        myHandScript = transform.parent.parent.parent.parent.Find("Canvas Hand").Find("Hand Image").GetComponent<HandScript>();
        myCharPanel = transform.parent.parent.GetComponent<CharacterPanelScript>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (myHandScript.MyMoveable is Equipment)
            {
                Equipment tmp = (Equipment)myHandScript.MyMoveable;

                if (tmp.MyEquipmentType == this.equipType)
                {
                    EquipStuff(tmp);
                }
            }
            else if (myHandScript.MyMoveable == null && MyEquip != null)
            {
                myHandScript.TakeMoveable(MyEquip);
                myCharPanel.MySelectedButton = this;
                icon.color = Color.grey;
            }
        }
    }

    public void EquipStuff(Equipment equipment)
    {

        equipment.Remove();

        if (MyEquip != null)
        {
            if (MyEquip != equipment)
            {
                Debug.Log(equipment);
                Debug.Log(equipment.MySlot);
                Debug.Log(MyEquip);
                equipment.MySlot.AddItem(MyEquip);
            }

            TooltipScreenSpaceUIItems.HideTooltip_Static();
            TooltipScreenSpaceUIItems.ShowTooltip_Static(MyEquip.tooltipItemName, MyEquip.tooltipItemDescription, null);
        }
        else
        {
            TooltipScreenSpaceUIItems.HideTooltip_Static();
        }

        icon.enabled = true;
        icon.sprite = equipment.MyIcon;
        icon.color = Color.white;
        this.equip = equipment;
        this.MyEquip.MyCharButton = this;

        if (MyEquip != null)
        {
            TooltipScreenSpaceUIItems.HideTooltip_Static();
            TooltipScreenSpaceUIItems.ShowTooltip_Static(MyEquip.tooltipItemName, MyEquip.tooltipItemDescription, null);
        }

        if (myHandScript.MyMoveable == (equipment as IMoveable))
        {
            myHandScript.Drop();
        }


        playerStats.ReloadEquipMainStats();

    }

    public void DequipStuff()
    {
        icon.color = Color.white;
        icon.enabled = false;
        MyEquip.MyCharButton = null;
        equip = null;
        onRemoved = true;
        playerStats.ReloadEquipMainStats();
    }

    public void MasterETStuffAssignment()
    {
        masterETItems.itemName = MyEquip.tooltipItemName;

        masterETItems.itemDescription = MyEquip.tooltipItemDescription;

        //masterETItems.buffSprite = Resources.Load<Sprite>("Sprites/BuffSprites/" + buffName);
    }

    private void Update()
    {
        if (MyEquip == null)
        {
            if (onRemoved)
            {
                onRemoved = false;
                masterETItems.itemName = "";
                masterETItems.itemDescription = "";
            }

            return;
        }

        MasterETStuffAssignment();
    }
}
