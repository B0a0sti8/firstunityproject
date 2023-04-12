using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillbookButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    string skillName;
    HandScript myHandScript;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            myHandScript.handSkillName = skillName;
        }
    }

    public void OnEndDrag(PointerEventData eventData) // triggers right after OnDrop (from ActionButton)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (myHandScript.handSkillName != "")
            {
                myHandScript.handButtonSwap = null;
                myHandScript.handSkillName = "";
            }
        }
    }

    GameObject PLAYER;
    GameObject skillManager;
    SkillPrefab buttonSkill;
    void Start()
    {
        PLAYER = transform.parent.parent.parent.parent.parent.parent.gameObject;
        skillManager = PLAYER.transform.Find("SkillManager").gameObject;

        myHandScript = transform.parent.parent.parent.parent.parent.Find("Canvas Hand").Find("Hand Image").GetComponent<HandScript>();

        string myClassName = transform.parent.name.Substring(0, transform.parent.name.Length - 6);
        SkillPrefab[] skills = skillManager.transform.Find(myClassName).GetComponents<SkillPrefab>();

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].GetType().ToString() == skillName) // Script-Name == skillName
            {
                buttonSkill = skills[i];
            }
        }

        MasterETStuffAssignment(buttonSkill);
    }

    void MasterETStuffAssignment(SkillPrefab skill)
    {
        skill.masterET = GetComponent<MasterEventTrigger>();

        skill.tooltipSkillName = skill.GetType().ToString();

        if (skill.tooltipSkillDescription == "")
        {
            skill.tooltipSkillDescription = "?GoodsInfo?";
        }

        skill.tooltipSkillSprite = gameObject.GetComponent<Image>().sprite;

        if (skill.hasGlobalCooldown)
        {
            skill.tooltipSkillType = "Weaponskill (<color=yellow>" + skill.masterChecks.masterGCTimeModified.ToString().Replace(",", ".") + "s</color>)";
        }
        else if (skill.isSuperInstant)
        {
            skill.tooltipSkillType = "Super-Instant";
        }
        else
        {
            skill.tooltipSkillType = "Instant";
        }

        if (skill.hasOwnCooldown)
        {
            skill.tooltipSkillCooldown = "Cooldown: <color=yellow>" + skill.ownCooldownTimeModified.ToString().Replace(",", ".") + "s</color>";
        }

        if (skill.needsMana)
        {
            skill.tooltipSkillCosts = "Mana: <color=#00ffffff>" + skill.manaCost.ToString().Replace(",", ".") + "</color>";
        }

        skill.tooltipSkillRange = "Range: <color=yellow>" + skill.skillRange.ToString().Replace(",", ".") + "m</color>";

        skill.tooltipSkillRadius = "Radius: <color=yellow>" + skill.skillRadius.ToString().Replace(",", ".") + "m</color>";


        skill.masterET.skillName = skill.tooltipSkillName;
        skill.masterET.skillDescription = skill.tooltipSkillDescription;
        skill.masterET.skillSprite = skill.tooltipSkillSprite;
        skill.masterET.skillType = skill.tooltipSkillType;
        skill.masterET.skillCooldown = skill.tooltipSkillCooldown;
        skill.masterET.skillCosts = skill.tooltipSkillCosts;
        skill.masterET.skillRange = skill.tooltipSkillRange;
        skill.masterET.skillRadius = skill.tooltipSkillRadius;
    }
}


//private Sprite icon;
//public Sprite MyIcon
//{
//    get
//    {
//        return icon;
//    }
//}

//private void Start()
//{
//    //icon = GetComponent<Image>().sprite;
//}

//public void OnPointerClick(PointerEventData eventData)
//{
//    if (eventData.button == PointerEventData.InputButton.Left)
//    {
//        Debug.Log("Button clicked: " + skillName);
//        //HandScript.MyInstance.TakeMoveable(Skillbook.MyInstance.GetSkill(skillName));
//        //HandScript.MyInstance.TakeMoveable(this);
//        HandScript.MyInstance.handSkillName = skillName;
//    }
//}