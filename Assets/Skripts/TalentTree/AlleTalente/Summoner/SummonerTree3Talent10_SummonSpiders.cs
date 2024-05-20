using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree3Talent10_SummonSpiders : Talent
{
    private GameObject mySkill;
    private SummonerClass mySummonerClass;
    private float slowEffectPerSpider = 0.02f;

    protected override void Awake()
    {
        talentName = "Summon Spiders";
        talentDescription = "Summons "
            + "1 / 2 / 3 / 4 / 5"
            + " spiders that attack and slow enemies around them by "
            + (1 * slowEffectPerSpider * 100).ToString() + " / "
            + (2 * slowEffectPerSpider * 100).ToString() + " / "
            + (3 * slowEffectPerSpider * 100).ToString() + " / "
            + (4 * slowEffectPerSpider * 100).ToString() + " / "
            + (5 * slowEffectPerSpider * 100).ToString() + " % each. "
            + "Multiple slow instances per enemy add up.";

        maxCount = 5;
        pointCost = 1;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        // Füge Skill Hinzu
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("SummonerSkills").Find("SummonSpiders").gameObject;
        mySkill.GetComponent<Button>().enabled = true;
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
        mySummonerClass.spiderCount = currentCount;
        mySummonerClass.spiderSlowEffect = currentCount * slowEffectPerSpider;
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffectAfterPointCountReduced();
        // Entferne Skill
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("SummonerSkills").Find("SummonSpiders").gameObject;
        if (currentCount == 0) mySkill.GetComponent<Button>().enabled = false;

        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
        mySummonerClass.spiderCount = currentCount;
        mySummonerClass.spiderSlowEffect = currentCount * 0.02f;
    }
}