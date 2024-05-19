using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree3Talent2_FireDemonExplosionCooldown : Talent
{
    private GameObject mySkill;
    private SummonerClass mySummonerClass;
    float damageModifier = 0.1f;
    float coolDownDec = 1f;

    protected override void Awake()
    {
        talentName = "Fire Demon Strength";
        talentDescription = "Reduces the fire demon explosion cooldown by "
            + (1 * coolDownDec).ToString() + " / "
            + (2 * coolDownDec).ToString() + " / "
            + (3 * coolDownDec).ToString() + " seconds and increases its damage by "
            + (1 * damageModifier * 100).ToString() + " / "
            + (2 * damageModifier * 100).ToString() + " / "
            + (3 * damageModifier * 100).ToString() + " %. ";

        predecessor = "Summon Fire Demon";
        maxCount = 3;
        pointCost = 2;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        mySummonerClass.fireDemonDamageModifier = currentCount * damageModifier;
        mySummonerClass.fireDemonExplosionCooldown = 10 - coolDownDec * currentCount;
    }

    public override void RemoveActiveTalentEffectAfterPointCountReduced()
    {
        base.RemoveActiveTalentEffectAfterPointCountReduced();
        mySummonerClass.fireDemonDamageModifier = currentCount * damageModifier;
        mySummonerClass.fireDemonExplosionCooldown = 10 - coolDownDec * currentCount;
    }

    public override void FindMyPredecessor()
    {
        base.FindMyPredecessor();
        myPredecessorTalent = transform.parent.Find("Talent(Clone)").gameObject; // Summon Astral Snake ist der predecessor
    }
}