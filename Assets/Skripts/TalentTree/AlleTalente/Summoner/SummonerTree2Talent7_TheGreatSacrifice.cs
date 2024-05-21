using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree2Talent7_TheGreatSacrifice : Talent
{
    private GameObject mySkill;
    private SummonerClass mySummonerClass;

    protected override void Awake()
    {
        talentName = "Ultimate Spell: The Great Sacrifice";
        talentDescription = "Sacrifices all your minions to grant 20 % more Mastery, Toughness, Intellect, Charisma and Tempo to all helping Players. ";

        maxCount = 1;
        pointCost = 5;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        // Füge Skill Hinzu
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("SummonerSkills").Find("Summoner_TheGreatSacrifice").gameObject;
        mySkill.GetComponent<Button>().enabled = true;
        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffectAfterPointCountReduced();
        // Entferne Skill
        mySkill = PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").Find("Skillbook").Find("Classes").Find("SummonerSkills").Find("Summoner_TheGreatSacrifice").gameObject;
        if (currentCount == 0) mySkill.GetComponent<Button>().enabled = false;

        PLAYER.transform.Find("Own Canvases").Find("Canvas Skillbook").GetComponent<SkillbookMaster>().UpdateCurrentSkills();
    }
}