using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerTree4Talent1_AdditionalMainMinion2 : Talent
{
    private SummonerClass mySummonerClass;

    protected override void Awake()
    {
        talentName = "Additional Main Minion 2";
        maxCount = 1;
        pointCost = 5;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        mySummonerClass.hasAdditionalMainMinion2 = true;
        statSkript.HandleResetMinionCount();
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();
        mySummonerClass.hasAdditionalMainMinion2 = false;
        statSkript.HandleResetMinionCount();
    }
}