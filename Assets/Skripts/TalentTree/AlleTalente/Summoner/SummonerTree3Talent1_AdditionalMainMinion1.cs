using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerTree3Talent1_AdditionalMainMinion1 : Talent
{
    private SummonerClass mySummonerClass;

    protected override void Awake()
    {
        talentName = "Additional Main Minion 1";
        maxCount = 1;
        pointCost = 5;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        mySummonerClass.hasAdditionalMainMinion1 = true;
        statSkript.HandleResetMinionCount();
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();
        mySummonerClass.hasAdditionalMainMinion1 = false;
        statSkript.HandleResetMinionCount();
    }
}