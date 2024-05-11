using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree1Talent4_DarkBoltDamageIncrease : Talent
{
    private SummonerClass mySummonerClass;
    private float darkboltDamageInc;

    protected override void Awake()
    {
        darkboltDamageInc = 0.05f;
        talentName = "Dark Bolt Damage Increase";
        talentDescription = " Increases the damage of Darkbolt by "
            + (darkboltDamageInc * 100).ToString() + " / "
            + (2 * darkboltDamageInc * 100).ToString() + " / "
            + (3 * darkboltDamageInc * 100).ToString() + " / "
            + (4 * darkboltDamageInc * 100).ToString() + " / "
            + (5 * darkboltDamageInc * 100).ToString()
            + " %.";
        maxCount = 5;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();

        // Setze Modifier in der Summoner Klasse.
        mySummonerClass.darkBoltDamageModifier = 1 + darkboltDamageInc * currentCount;
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();

        // Setze Modifier in der Summoner Klasse.
        mySummonerClass.darkBoltDamageModifier = 1 + darkboltDamageInc * currentCount;
    }
}