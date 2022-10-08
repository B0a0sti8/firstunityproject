using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasterEventTriggerBuffs : EventTrigger
{
    public string buffName;
    public string buffDescription;

    //bool showTooltip = false;

    //void Update()
    //{
    //    if (showTooltip)
    //    {
    //        TooltipScreenSpaceUIBuffs.ShowTooltip_Static(buffName, buffDescription);
    //    }
    //}

    public override void OnPointerEnter(PointerEventData data)
    {
        if (buffName == "") return;

        TooltipScreenSpaceUIBuffs.ShowTooltip_Static(buffName, buffDescription);

        //showTooltip = true;
    }

    public override void OnPointerExit(PointerEventData data)
    {
        //showTooltip = false;
        TooltipScreenSpaceUIBuffs.HideTooltip_Static();
    }
}
