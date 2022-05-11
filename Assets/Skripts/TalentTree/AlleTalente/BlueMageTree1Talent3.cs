using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueMageTree1Talent3 : Talent
{
    private GameObject mySkill;
    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("BlueMageSkills").Find("TestAttack2").gameObject;
        mySkill.GetComponent<Button>().enabled = true;
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
        // Füge Skill Hinzu
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("BlueMageSkills").Find("TestAttack2").gameObject;
        mySkill.GetComponent<Button>().enabled = false;
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
        // Entferne Skill
    }
}
