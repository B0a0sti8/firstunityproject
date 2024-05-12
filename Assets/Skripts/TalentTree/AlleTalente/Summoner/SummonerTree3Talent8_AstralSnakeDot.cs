using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree3Talent8_AstralSnakeDot : Talent
{
    private GameObject mySkill;
    private SummonerClass mySummonerClass;
    float myAstralSnakeDotMod;

    protected override void Awake()
    {
        myAstralSnakeDotMod = 0.2f;
        talentName = "Astral Snake DoT";
        talentDescription = "The astral snake additionaly deals damage over time. (Only single instance per enemy)";
        predecessor = "Summon Astral Snake";

        maxCount = 5;
        pointCost = 2;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();

    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        mySummonerClass.astralSnakeHasDoT = true;
        mySummonerClass.astralSnakeDotMod = currentCount * myAstralSnakeDotMod;
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffectAfterPointCountReduced();

        mySummonerClass.astralSnakeHasDoT = false;
        mySummonerClass.astralSnakeDotMod = currentCount * myAstralSnakeDotMod;
    }

    public override void FindMyPredecessor()
    {
        base.FindMyPredecessor();
        myPredecessorTalent = transform.parent.Find("Talent (7)(Clone)").gameObject; // Summon Astral Snake ist der predecessor
    }
}