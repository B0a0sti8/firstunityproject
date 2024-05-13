using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree2Talent1_SummonSpiritWolfOnSkill : Talent
{
    private GameObject mySkill;
    private SummonerClass mySummonerClass;
    private float spiritWolfChance;
    protected override void Awake()
    {
        spiritWolfChance = 0.1f;
        talentName = "Spirit Wolf";
        talentDescription = "Grants a buff to one player. Each skill, cast by that player has a " +
            (1 * spiritWolfChance * 100).ToString() + " / " +
            (2 * spiritWolfChance * 100).ToString() + " / " +
            (3 * spiritWolfChance * 100).ToString() + " / " +
            (4 * spiritWolfChance * 100).ToString() + " / " +
            (5 * spiritWolfChance * 100).ToString() + " % " +
            "chance to summon a spirit wolf. This counts as one of your minions.";
        maxCount = 5;
        pointCost = 2;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        // Füge Skill Hinzu
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("SummonerSkills").Find("SummonSpiritWolfOnSkill").gameObject;
        mySkill.GetComponent<Button>().enabled = true;
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();

        // Für höhere Stufen: Erhöhe Schaden des Skills.
        mySummonerClass.summonerSummonSpiritWolfOnSkillChance = (float)(1 + (currentCount - 1) * spiritWolfChance);

    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();
        // Entferne Skill
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("SummonerSkills").Find("SummonSpiritWolfOnSkill").gameObject;
        if (currentCount == 0)
        {
            mySkill.GetComponent<Button>().enabled = false;
        }
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
        mySummonerClass.summonerSummonSpiritWolfOnSkillChance = (float)(1 + (currentCount - 1) * spiritWolfChance);

    }
}