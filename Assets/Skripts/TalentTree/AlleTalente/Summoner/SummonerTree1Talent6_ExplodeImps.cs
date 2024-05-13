using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree1Talent6_ExplodeImps : Talent
{
    private GameObject mySkill;
    private SummonerClass mySummonerClass;
    private float expImpDamageMod;
    protected override void Awake()
    {
        expImpDamageMod = 0.2f;
        talentName = "Explode Imps";
        talentDescription = "Allows you to throw your imps on target enemy and makes them explode. Higher stages increase damage by " + ((int)(expImpDamageMod*100)).ToString() + " % each.";
        maxCount = 3;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        // Füge Skill Hinzu
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("SummonerSkills").Find("ExplodeImps").gameObject;
        mySkill.GetComponent<Button>().enabled = true;
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();

        // Für höhere Stufen: Erhöhe Schaden des Skills.
        mySummonerClass.ExplodingImpsDamageModifier = (float)( 1 + (currentCount - 1) * expImpDamageMod);

    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();
        // Entferne Skill
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("SummonerSkills").Find("ExplodeImps").gameObject;
        if (currentCount == 0)
        {
            mySkill.GetComponent<Button>().enabled = false;
        }
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
        mySummonerClass.ExplodingImpsDamageModifier = (float)(1 + (currentCount - 1) * expImpDamageMod);

    }
}