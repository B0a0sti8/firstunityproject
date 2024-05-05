using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree1Talent2_CastSpeedOnMinionSummoned : Talent
{
    private SummonerClass mySummonerClass;

    protected override void Awake()
    {
        talentName = "Cast Speed On Minion Summoned";
        maxCount = 3;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();

        // Setzte Bool in Summoner Klasse, gib Modifier
        mySummonerClass.hasCastSpeedOnMinionSummonedTalent = true;
        mySummonerClass.minionSummoned_CastSpeedModifier = 0.2f * currentCount;
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffectAfterPointCountReduced();

        // Setzte Bool in Summoner Klasse, gib Modifier
        if (currentCount == 0)
        {
            mySummonerClass.hasCastSpeedOnMinionSummonedTalent = false;
        }

        mySummonerClass.minionSummoned_CastSpeedModifier = 0.2f * currentCount;
    }
}
