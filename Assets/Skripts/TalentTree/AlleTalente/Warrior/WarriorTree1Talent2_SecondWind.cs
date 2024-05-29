using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorTree1Talent2_SecondWind : Talent
{
    private GameObject mySkill;
    private WarriorClass myWarriorClass;

    protected override void Awake()
    {
        talentName = "Second Wind";
        talentDescription = "Grants access to second wind, a self heal for 15% of your missing health. ";
        maxCount = 1;
        pointCost = 2;
        base.Awake();
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        // Füge Skill Hinzu
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("WarriorSkills").Find("Warrior_SecondWind").gameObject;
        mySkill.GetComponent<Button>().enabled = true;
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();
        // Entferne Skill
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("WarriorSkills").Find("Warrior_SecondWind").gameObject;
        if (currentCount == 0) mySkill.GetComponent<Button>().enabled = false;

        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
    }
}