using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerTree3Talent5_BuffMainMinionDuration : Talent
{
    private SummonerClass mySummonerClass;
    private float durationInc;
    private float consumeCooldown;

    protected override void Awake()
    {
        durationInc = 0.5f;
        consumeCooldown = 0.5f;

        talentName = "Consuming for Buff Duration";
        talentDescription = "During the buff, your main minions will consume Imps and Spirit Wolfes every "
            + (9 - consumeCooldown).ToString() + " / "
            + (9 - 2 * consumeCooldown).ToString() + " / "
            + (9 - 3 * consumeCooldown).ToString() + " / "
            + (9 - 4 * consumeCooldown).ToString() + " / "
            + (9 - 5 * consumeCooldown).ToString() 
            + " seconds" +
        ", to increase their buff duration by " 
            +(durationInc).ToString() + " / "
            + (2 * durationInc).ToString() + " / "
            + (3 * durationInc).ToString() + " / "
            + (4 * durationInc).ToString() + " / "
            + (5 * durationInc).ToString()
            + " %.";

        predecessor = "Summoners Rage";
        maxCount = 5;
        pointCost = 2;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        mySummonerClass.buffMainMinionDmgAndHealIsConsuming = true;
        mySummonerClass.buffMainMinionDmgAndHealIsConsumingDurationInc = currentCount * durationInc;
        mySummonerClass.buffMainMinionDmgAndHealIsConsumingCooldownDec = 9 - currentCount * consumeCooldown;
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffect();
        mySummonerClass.buffMainMinionDmgAndHealIsConsuming = false;
        mySummonerClass.buffMainMinionDmgAndHealIsConsumingDurationInc = 0;
        mySummonerClass.buffMainMinionDmgAndHealIsConsumingCooldownDec = 0;
    }

    public override void FindMyPredecessor()
    {
        base.FindMyPredecessor();
        myPredecessorTalent = transform.parent.Find("Talent (4)(Clone)").gameObject; // Buff main minion ist der predecessor
    }
}