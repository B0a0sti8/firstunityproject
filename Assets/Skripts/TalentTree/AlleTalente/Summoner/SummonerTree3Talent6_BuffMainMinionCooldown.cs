using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree3Talent6_BuffMainMinionCooldown : Talent
{
    private BuffMainMinionDamageAndHealing buffMainMinionsSkill;
    private SummonerClass mySummonerClass;

    protected override void Awake()
    {
        talentName = "Cooldown Reduction";
        talentDescription = "The cooldown of the skill is reduced by 30 seconds. ";
        predecessor = "Summoners Rage";

        maxCount = 1;
        pointCost = 10;
        base.Awake();
        buffMainMinionsSkill = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<BuffMainMinionDamageAndHealing>();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        buffMainMinionsSkill.SetMyCooldown(120 - 30);
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();
        buffMainMinionsSkill.SetMyCooldown(120 - 0);
    }

    public override void FindMyPredecessor()
    {
        base.FindMyPredecessor();
        myPredecessorTalent = transform.parent.Find("Talent (4)(Clone)").gameObject; // Buff main minion ist der predecessor
    }
}