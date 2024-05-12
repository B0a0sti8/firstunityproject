using UnityEngine;
using UnityEngine.InputSystem;

public class TalentTreeUI : MonoBehaviour
{
    public GameObject talentTreeUI;

    void Start()
    {
        talentTreeUI = transform.Find("TalentTreeWindow").gameObject;
        //talentTreeUI = gameObject;
        talentTreeUI.SetActive(false);
    }

    public void OpenTalentTree()
    {
        Debug.Log("Talent Tree An/Aus");
        if (talentTreeUI.activeSelf)
        {
            transform.parent.Find("Canvas Tooltips").Find("TooltipScreenSpaceUI_Talents").GetComponent<TooltipScreenSpaceUITalent>().HideTooltip();
        }
        talentTreeUI.SetActive(!talentTreeUI.activeSelf);
    }
}