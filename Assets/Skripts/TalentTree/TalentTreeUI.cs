using UnityEngine;
using UnityEngine.InputSystem;

public class TalentTreeUI : MonoBehaviour
{
    public GameObject talentTreeUI;

    public bool isLoadingTalentTree;

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
        if (talentTreeUI.activeSelf) talentTreeUI.GetComponent<TalentTree>().UpdateTalentPointText();
    }

    public void LateUpdate()
    {
        if (talentTreeUI.GetComponent<TalentTree>().checkAfterReset)
        {
            talentTreeUI.GetComponent<TalentTree>().HasToCheckAfterReset();
        }

        if (isLoadingTalentTree)
        {
            Debug.Log("Doing Something!");
            isLoadingTalentTree = false;
            talentTreeUI.GetComponent<TalentTree>().AutoSkillWhenLoading2();
        }
    }
}