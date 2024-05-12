using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree2Talent0_StunEnemiesAroundSummoner : Talent
{
    private GameObject mySkill;
    private SummonerClass mySummonerClass;
    private float stunDurationInc;

    protected override void Awake()
    {
        stunDurationInc = 0.5f;
        talentName = "Explode Imps";
        talentDescription = "Stuns enemies around the Summoner for  "
            + (stunDurationInc * 100).ToString() + " / "
            + (2 * stunDurationInc * 100).ToString() + " / "
            + (3 * stunDurationInc * 100).ToString() + " / "
            + (4 * stunDurationInc * 100).ToString() + " / "
            + (5 * stunDurationInc * 100).ToString()
            + " %.";
        maxCount = 5;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        // Füge Skill Hinzu
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("SummonerSkills").Find("StunEnemiesAroundSummoner").gameObject;
        mySkill.GetComponent<Button>().enabled = true;
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();

        // Für höhere Stufen: Erhöhe Dauer des Stuns
        mySummonerClass.summonerAoEStunDuration = (float)(stunDurationInc * currentCount);

    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();
        // Entferne Skill
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("SummonerSkills").Find("StunEnemiesAroundSummoner").gameObject;
        if (currentCount == 0)
        {
            mySkill.GetComponent<Button>().enabled = false;
        }
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
        mySummonerClass.summonerAoEStunDuration = (float)(stunDurationInc * currentCount);

    }
}