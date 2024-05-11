using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerTree4Talent5_IncreasedMinionDuration : Talent
{
    private SummonerClass mySummonerClass;
    private float minionDurationInc;

    protected override void Awake()
    {
        minionDurationInc = 0.6f;
        talentName = "Increased Minion Duration";
        talentDescription = "Increases duration of secondary minions by "
            + (minionDurationInc).ToString() + " / "
            + (2 * minionDurationInc).ToString() + " / "
            + (3 * minionDurationInc).ToString() + " / "
            + (4 * minionDurationInc).ToString() + " / "
            + (5 * minionDurationInc).ToString()
            + " seconds.";
        maxCount = 5;
        pointCost = 3;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        if (currentCount >= 2)
        {
            mySummonerClass.increasedMinionDuration -= (minionDurationInc * currentCount-1);
        }
        mySummonerClass.increasedMinionDuration += minionDurationInc * currentCount;
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();
        mySummonerClass.increasedMinionDuration -= minionDurationInc * currentCount;
    }
}