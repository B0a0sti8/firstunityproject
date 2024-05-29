using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorTree1Talent0_OffensiveStance : Talent
{
    private GameObject mySkill;
    private WarriorClass myWarriorClass;

    protected override void Awake()
    {
        talentName = "Offensive Stance";
        talentDescription = "Takes an offensive Stance, increasing damage and tempo, but reducing toughness and healing.  Can only have one stance. ";
        maxCount = 1;
        pointCost = 2;
        base.Awake();
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        // Füge Skill Hinzu
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("WarriorSkills").Find("Warrior_OffensiveStance").gameObject;
        mySkill.GetComponent<Button>().enabled = true;
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();
        // Entferne Skill
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("WarriorSkills").Find("Warrior_OffensiveStance").gameObject;
        if (currentCount == 0) mySkill.GetComponent<Button>().enabled = false;

        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
    }
}