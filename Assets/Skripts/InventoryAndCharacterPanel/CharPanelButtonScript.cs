using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharPanelButtonScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EquipmentType equipType;
    private Equipment equip;

    [SerializeField] private Image icon;

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
        }
    }

    public void EquipStuff(Equipment equipment)
    {

        equipment.Remove();

        if (equip != null)
        {

            equipment.MySlot.AddItem(equip);
            TooltipScreenSpaceUIItems.HideTooltip_Static();
            TooltipScreenSpaceUIItems.ShowTooltip_Static(equip.tooltipItemName, equip.tooltipItemDescription, null);
            //equipment.MySlot.MasterETStuffAssignment();
        }
        else
        {
            TooltipScreenSpaceUIItems.HideTooltip_Static();
        }

        icon.enabled = true;
        icon.sprite = equipment.MyIcon;
        this.equip = equipment;

        if (HandScript.MyInstance.MyMoveable == (equipment as IMoveable))
        {
            HandScript.MyInstance.Drop();
        }
        
    }
}
