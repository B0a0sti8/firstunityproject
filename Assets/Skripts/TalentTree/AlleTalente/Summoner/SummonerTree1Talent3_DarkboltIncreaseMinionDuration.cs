using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree1Talent3_DarkboltIncreaseMinionDuration : Talent
{
    private SummonerClass mySummonerClass;
    private float darkboltMinionDurationInc;

    protected override void Awake()
    {
        darkboltMinionDurationInc = 0.1f;
        talentName = "Dark Bolt Minion duration increase";
        talentDescription = "Darkbolt increases the duration of all your minions by "
            + (darkboltMinionDurationInc).ToString() + " / "
            + (2 * darkboltMinionDurationInc).ToString() + " / "
            + (3 * darkboltMinionDurationInc).ToString() + " / "
            + (4 * darkboltMinionDurationInc).ToString() + " / "
            + (5 * darkboltMinionDurationInc).ToString()
            + " seconds.";
        maxCount = 5;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();

        // Setze Modifier in der Summoner Klasse.
        mySummonerClass.doesDarkboltIncreaseMinionLifetime = true;
        mySummonerClass.darkBoltLifeTimeIncrease = darkboltMinionDurationInc * currentCount;
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();

        // Setze Modifier in der Summoner Klasse.
        mySummonerClass.doesDarkboltIncreaseMinionLifetime = false;
        mySummonerClass.darkBoltLifeTimeIncrease = darkboltMinionDurationInc * currentCount;
    }
}