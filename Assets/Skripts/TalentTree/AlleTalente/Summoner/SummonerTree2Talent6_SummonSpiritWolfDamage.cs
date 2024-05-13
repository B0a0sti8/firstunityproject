using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree2Talent6_SummonSpiritWolfDamage : Talent
{
    private SummonerClass mySummonerClass;
    private float summonSpiritWolfDamage;
    private float summonSpiritWolfDuration;

    protected override void Awake()
    {
        summonSpiritWolfDamage = 0.1f;
        summonSpiritWolfDuration = 2f;
        talentName = "Spirit Wolf damage and wolf duration";
        talentDescription = "The wolf damage is increased by  " +
            (1 * summonSpiritWolfDamage * 100).ToString() + " / " +
            (2 * summonSpiritWolfDamage * 100).ToString() + " / " +
            (3 * summonSpiritWolfDamage * 100).ToString() + 
            " %. Additionally, the wolf duration is increased by " +
            (1 * summonSpiritWolfDuration).ToString() + " / " +
            (2 * summonSpiritWolfDuration).ToString() + " / " +
            (3 * summonSpiritWolfDuration).ToString() +
            " seconds.";
        maxCount = 3;
        pointCost = 3;
        base.Awake();

        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();

        mySummonerClass.summonerSummonSpiritWolfOnSkillWolfDamageInc = (float)(currentCount * summonSpiritWolfDamage);
        mySummonerClass.summonerSummonSpiritWolfOnSkillWolfDurationInc = (float)(currentCount * summonSpiritWolfDuration);
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();
        mySummonerClass.summonerSummonSpiritWolfOnSkillWolfDamageInc = (float)(currentCount * summonSpiritWolfDamage);
        mySummonerClass.summonerSummonSpiritWolfOnSkillWolfDurationInc = (float)(currentCount * summonSpiritWolfDuration);
    }

    public override void FindMyPredecessor()
    {
        base.FindMyPredecessor();
        myPredecessorTalent = transform.parent.Find("Talent (1)(Clone)").gameObject; // Summon Spirit ist der predecessor
    }
}