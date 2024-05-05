using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree1Talent4_DarkBoltDamageIncrease : Talent
{
    private SummonerClass mySummonerClass;

    protected override void Awake()
    {
        talentName = "Dark Bolt Damage Increase";
        maxCount = 5;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();

        // Setze Modifier in der Summoner Klasse.
        mySummonerClass.darkBoltDamageModifier = 1 + 0.05f * currentCount;
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();

        // Setze Modifier in der Summoner Klasse.
        mySummonerClass.darkBoltDamageModifier = 1 + 0.05f * currentCount;
    }
}