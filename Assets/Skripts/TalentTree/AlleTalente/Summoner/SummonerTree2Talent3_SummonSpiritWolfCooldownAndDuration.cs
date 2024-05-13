using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree2Talent3_SummonSpiritWolfCooldownAndDuration : Talent
{
    private SummonSpiritWolfOnSkill mySummonSpiritWolfSkill;
    private SummonerClass mySummonerClass;
    private float summonSpiritWolfDuration;

    protected override void Awake()
    {
        
        summonSpiritWolfDuration = 5f;
        talentName = "Spirit Wolf Cooldown and Buff duration";
        talentDescription = "The duration of the buff is increased by  " +
            (1 * summonSpiritWolfDuration).ToString() + " / " +
            (2 * summonSpiritWolfDuration).ToString() + " / " +
            (3 * summonSpiritWolfDuration).ToString() +
            " seconds. Additionally, the cooldown is reduced by 7 / 14 / 21 seconds.";
        maxCount = 3;
        pointCost = 3;
        base.Awake();
        mySummonSpiritWolfSkill = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonSpiritWolfOnSkill>();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();

        mySummonerClass.summonerSummonSpiritWolfOnSkillDurationInc = (float)(currentCount * summonSpiritWolfDuration);
        mySummonSpiritWolfSkill.SetMyCooldown(60 - 7 * currentCount);
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();
        mySummonerClass.summonerSummonSpiritWolfOnSkillDurationInc = (float)(currentCount * summonSpiritWolfDuration);
        mySummonSpiritWolfSkill.SetMyCooldown(60 - 7 * currentCount);
    }

    public override void FindMyPredecessor()
    {
        base.FindMyPredecessor();
        myPredecessorTalent = transform.parent.Find("Talent (1)(Clone)").gameObject; // Summon Spirit ist der predecessor
    }
}