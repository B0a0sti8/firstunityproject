using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDropHandler, IEndDragHandler
{
    TextMeshProUGUI buttonText;
    Image buttonImage;

    GameObject PLAYER;
    GameObject skillManager;

    public string skillName;
    public string className;

    SkillPrefab buttonSkill;

    public bool skillAvailable;



    void Start()
    {
        buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        buttonImage = GetComponent<Image>();
        PLAYER = transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
        skillManager = PLAYER.transform.Find("SkillManager").gameObject;

        //if (skillName == "") return;
        ////if (className == "") return;
        //FindMatchingSkill();
        //UpdateButton();
        //MasterETStuffAssignment(buttonSkill);
    }

    void Update()
    {
        if (skillName == "")
        {
            buttonSkill = null;
            buttonImage.sprite = Resources.Load<Sprite>("SkillSprites/EmptySlot");
            buttonText.text = "";
            buttonImage.color = new Color32(255, 255, 255, 255);
            return;
        }
        //if (className == "") return;
        FindMatchingSkill();
        UpdateButton();
        MasterETStuffAssignment(buttonSkill);
    }

    void FindMatchingSkill()
    {
        SkillPrefab[] skills = skillManager.transform.Find("BlueMage").GetComponents<SkillPrefab>();

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].GetType().ToString() == skillName) // Script-Name == skillName
            {
                buttonSkill = skills[i];
            }
        }

        buttonImage.sprite = Resources.Load<Sprite>("SkillSprites/" + buttonSkill.GetType().ToString());
    }

    void UpdateButton()
    {
        if (buttonSkill.ownCooldownTimeLeft > 0)
        {
            buttonText.text = Mathf.Round(buttonSkill.ownCooldownTimeLeft).ToString();
            buttonImage.color = new Color32(120, 120, 120, 255);
        }
        else
        {
            if (buttonSkill.ownCooldownActive)
            {
                buttonText.text = "";
                if (!buttonSkill.hasGlobalCooldown || (buttonSkill.masterChecks.masterGCTimeLeft <= 0))
                {
                    buttonImage.color = new Color32(255, 255, 255, 255);
                }
            }
        }

        if (buttonSkill.hasGlobalCooldown)
        {
            if (buttonSkill.masterChecks.masterGCTimeLeft > 0)
            {
                buttonImage.color = new Color32(120, 120, 120, 255);
            }
            else
            {
                if (buttonSkill.ownCooldownTimeLeft <= 0)
                {
                    buttonImage.color = new Color32(255, 255, 255, 255);
                }
            }
        }
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

    public void UseSkillOnClick()
    {
        if (buttonSkill == null) return;

        if (skillAvailable)
        {
            buttonSkill.StartSkillChecks();
        }
        else
        {
            Debug.Log("You don't have that skill yet.");
        }
    }

    public void OnPointerClick(PointerEventData eventData) // Wenn Button geklickt wird ausgeführt (nur Mausklick)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.handSkillName != "")
            {
                skillName = HandScript.MyInstance.handSkillName;
                HandScript.MyInstance.handSkillName = "";
            }
            else
            {
                UseSkillOnClick();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (HandScript.MyInstance.handSkillName != "") return;
        if (skillName == "") return;
        if (HandScript.MyInstance.actionButtonDragOn)
        {
            HandScript.MyInstance.handButtonSwap = gameObject;
            HandScript.MyInstance.handSkillName = skillName;
            skillName = "";
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (HandScript.MyInstance.handSkillName == "") return;
        if (HandScript.MyInstance.actionButtonDragOn)
        {
            if (HandScript.MyInstance.handButtonSwap != null) // doesn't trigger when dragging from Skillbook to ActionButton
            {
                HandScript.MyInstance.handButtonSwap.GetComponent<ActionButton>().skillName = skillName;
            }
        }
        skillName = HandScript.MyInstance.handSkillName;
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").gameObject.GetComponent<SkillbookMaster>().UpdateCurrentSkills();
    }

    public void OnEndDrag(PointerEventData eventData) // triggers right after OnDrop
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (HandScript.MyInstance.handSkillName == "") return;
        HandScript.MyInstance.handButtonSwap = null;
        HandScript.MyInstance.handSkillName = "";
    }
}


//public Button MyButton { get; private set; }

//MyButton = GetComponent<Button>();

//if (HandScript.MyInstance.MyMoveable != null)
//{
//    UpdateVisual();
//}
//else
//{
//    UseSkillOnClick();
//}

//public void UpdateVisual()
//{
//    buttonImage.sprite = HandScript.MyInstance.Put().MyIcon;
//    buttonImage.color = Color.white;
//}

//private IUseable useable;
//if(useable != null)
//{
//    useable.Use();
//}