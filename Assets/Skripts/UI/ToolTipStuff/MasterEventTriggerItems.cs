using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasterEventTriggerItems : EventTrigger
{
    public string itemName;
    public string itemDescription;
    public Sprite tooltipSprite;


    public override void OnPointerEnter(PointerEventData data)
    {
        //if (itemName == "") return;

        TooltipScreenSpaceUIItems.ShowTooltip_Static(itemName, itemDescription, tooltipSprite);
        //showTooltip = true;
    }

    public override void OnPointerExit(PointerEventData data)
    {
        //showTooltip = false;
        TooltipScreenSpaceUIItems.HideTooltip_Static();
    }
}
