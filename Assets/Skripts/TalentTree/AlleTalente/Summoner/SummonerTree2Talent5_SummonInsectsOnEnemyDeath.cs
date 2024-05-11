using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree2Talent5_SummonInsectsOnEnemyDeath : Talent
{
    private GameObject mySkill;
    private SummonerClass mySummonerClass;

    protected override void Awake()
    {
        talentName = "Summon Insects on Enemy Death";
        talentDescription = "Inflicts a Debuff on target Enemy. When that enemy dies, insects spawn from its body, fighting for you.";
        maxCount = 3;
        pointCost = 3;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();


    }
    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        // Füge Skill Hinzu
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("SummonerSkills").Find("SummonInsectsOnEnemyDeath").gameObject;
        mySkill.GetComponent<Button>().enabled = true;
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();

        // Für höhere Stufen: Erhöhe Insekten die gespawnt werden.
        mySummonerClass.increasedInsectSummon = Mathf.Max((int)(currentCount - 1), 0);
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffectAfterPointCountReduced();
        // Entferne Skill
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("SummonerSkills").Find("SummonInsectsOnEnemyDeath").gameObject;
        if (currentCount == 0)
        {
            mySkill.GetComponent<Button>().enabled = false;
        }
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
        mySummonerClass.increasedInsectSummon = Mathf.Max((int)(currentCount - 1), 0);
    }
}