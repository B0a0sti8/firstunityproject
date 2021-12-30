using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasterEventTrigger : EventTrigger
{
    public string skillName;
    public string skillDescription;
    public Sprite skillSprite;
    public string skillType;
    public string skillCooldown;
    public string skillCosts;
    public string skillRange;
    public string skillRadius;

    bool showTooltip = false;

    private void Update()
    {
        if (showTooltip)
        {
            TooltipScreenSpaceUIAdvanced.ShowTooltip_Static(skillName, skillDescription, skillSprite, skillType, 
                skillCooldown, skillCosts, skillRange, skillRadius);
        }
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        showTooltip = true;
        //TooltipScreenSpaceUIAdvanced.ShowTooltip_Static(timer.ToString(), skillDescription, skillSprite, skillType, skillCooldown, skillCosts);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        showTooltip = false;
        TooltipScreenSpaceUIAdvanced.HideTooltip_Static();
    }

    //public override void OnPointerEnter(PointerEventData data)
    //{
    //    System.Func<string> getTooltipTextFunc = () =>
    //    {
    //        return skillName;
    //    };
    //    TooltipScreenSpaceUIAdvanced.ShowTooltip_Static(getTooltipTextFunc);
    //}
}
