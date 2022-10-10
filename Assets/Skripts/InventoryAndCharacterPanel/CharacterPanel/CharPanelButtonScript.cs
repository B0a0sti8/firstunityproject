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

    public Equipment MyEquip { get => equip; }

    private void Awake()
    {
        masterETItems = GetComponent<MasterEventTriggerItems>();
        playerStats = transform.parent.parent.parent.parent.parent.gameObject.GetComponent<PlayerStats>();
        Debug.Log(playerStats);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable is Equipment)
            {
                Equipment tmp = (Equipment)HandScript.MyInstance.MyMoveable;

                if (tmp.MyEquipmentType == this.equipType)
                {
                    EquipStuff(tmp);
                }
            }
            else if (HandScript.MyInstance.MyMoveable == null && MyEquip != null)
            {
                HandScript.MyInstance.TakeMoveable(MyEquip);
                CharacterPanelScript.MyInstance.MySelectedButton = this;
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

        if (HandScript.MyInstance.MyMoveable == (equipment as IMoveable))
        {
            HandScript.MyInstance.Drop();
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
