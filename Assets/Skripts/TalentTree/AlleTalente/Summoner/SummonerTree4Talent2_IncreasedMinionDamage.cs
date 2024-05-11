using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerTree4Talent2_IncreasedMinionDamage : Talent
{
    private SummonerClass mySummonerClass;
    private float minionDamageInc;

    protected override void Awake()
    {
        minionDamageInc = 0.06f;

        talentName = "Increased Minion Duration";
        talentDescription = "Increases the damage of all minions by "
            + (minionDamageInc*100).ToString() + " / "
            + (2 * minionDamageInc * 100).ToString() + " / "
            + (3 * minionDamageInc * 100).ToString() + " / "
            + (4 * minionDamageInc * 100).ToString() + " / "
            + (5 * minionDamageInc * 100).ToString()
            + " %.";
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
            mySummonerClass.increasedMinionDamage -= minionDamageInc * (currentCount-1);
        }
        mySummonerClass.increasedMinionDamage += minionDamageInc * currentCount;
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();
        mySummonerClass.increasedMinionDamage -= minionDamageInc * currentCount;
    }
}