using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MasterEventTriggerTalent : EventTrigger
{
    public string talentName;
    public string skillDescription;
    public Sprite talentSprite;
    public string tooltipCost;
    public string tooltipSkilled;
    public string tooltipPredecessor;

    bool showTooltip = false;

    private void Start()
    {
        GetTalentInfo();
    }

    private void Update()
    {
        if (showTooltip)
        {
            TooltipScreenSpaceUITalent.ShowTooltip_Static(talentName, skillDescription, talentSprite, tooltipCost, tooltipSkilled,  tooltipPredecessor);
        }
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        if (talentName != "")
        {
            showTooltip = true;
        }
    }

    public override void OnPointerExit(PointerEventData data)
    {
        showTooltip = false;
        TooltipScreenSpaceUITalent.HideTooltip_Static();
    }

    public void GetTalentInfo()
    {
        talentName = GetComponent<Talent>().talentName;
        skillDescription = GetComponent<Talent>().talentDescription;
        talentSprite = GetComponent<Image>().sprite;
        tooltipCost = "Talent cost: " + GetComponent<Talent>().pointCost.ToString();
        tooltipSkilled = "Already skilled: " + GetComponent<Talent>().currentCount.ToString() + " / " + GetComponent<Talent>().maxCount;
        tooltipPredecessor = "Predecessor: " + GetComponent<Talent>().predecessor;
    }
}
