using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree4Talent3_DarkBoltImpChance : Talent
{
    private GameObject mySkill;
    private SummonerClass mySummonerClass;

    protected override void Awake()
    {
        talentName = "Dark Bolt Imp Chance";
        talentDescription = "Dark Bolt has a Chance of 10 / 20 / 30 % to summon an imp. ";

        maxCount = 3;
        pointCost = 2;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        // Füge Skill Hinzu
        mySummonerClass.darkBoltChanceToSpawnImp = 0.1f * currentCount;
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffectAfterPointCountReduced();
        // Entferne Skill
        mySummonerClass.darkBoltChanceToSpawnImp = 0.1f * currentCount;
    }
}