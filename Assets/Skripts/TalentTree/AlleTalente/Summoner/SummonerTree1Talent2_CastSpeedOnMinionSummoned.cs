using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree1Talent2_CastSpeedOnMinionSummoned : Talent
{
    private SummonerClass mySummonerClass;
    private float actionSpeedIncPerMin;

    protected override void Awake()
    {
        actionSpeedIncPerMin = 0.2f;
        talentName = "Cast Speed On Minion Summoned";
        talentDescription = "Increases action speed by "
            + (actionSpeedIncPerMin * 100).ToString() + " / "
            + (2 * actionSpeedIncPerMin * 100).ToString() + " / "
            + (3 * actionSpeedIncPerMin * 100).ToString() 
            + " % for each minion summoned in the past 10 seconds";
        maxCount = 3;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        // Setzte Bool in Summoner Klasse, gib Modifier
        mySummonerClass.hasCastSpeedOnMinionSummonedTalent = true;
        mySummonerClass.minionSummoned_CastSpeedModifier = actionSpeedIncPerMin * currentCount;
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffectAfterPointCountReduced();

        // Setzte Bool in Summoner Klasse, gib Modifier
        if (currentCount == 0)
        {
            mySummonerClass.hasCastSpeedOnMinionSummonedTalent = false;
        }

        mySummonerClass.minionSummoned_CastSpeedModifier = actionSpeedIncPerMin * currentCount;
    }
}
