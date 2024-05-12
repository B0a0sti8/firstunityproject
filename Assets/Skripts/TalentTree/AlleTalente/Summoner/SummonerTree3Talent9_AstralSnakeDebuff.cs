using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree3Talent9_AstralSnakeDebuff : Talent
{
    private GameObject mySkill;
    private SummonerClass mySummonerClass;

    protected override void Awake()
    {
        talentName = "Astral Snake Debuff";
        talentDescription = "The astral snake additionaly increases damage taken. (Only single instance per enemy)";
        predecessor = "Summon Astral Snake";

        maxCount = 1;
        pointCost = 10;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        mySummonerClass.astralSnakeHasDebuff = true;
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffectAfterPointCountReduced();
        mySummonerClass.astralSnakeHasDebuff = false;
    }

    public override void FindMyPredecessor()
    {
        base.FindMyPredecessor();
        myPredecessorTalent = transform.parent.Find("Talent (7)(Clone)").gameObject; // Summon Astral Snake ist der predecessor
    }
}