using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree2Talent4_IncreasedBuffEffect : Talent
{
    private SummonerClass mySummonerClass;
    private float buffEffectInc;

    protected override void Awake()
    {
        buffEffectInc = 0.1f;
        talentName = "Increased Buff Effect";
        talentDescription = " Increases the effectiveness of your buffs by "
            + ((int)buffEffectInc * 100).ToString() + " / "
            + ((int)2 * buffEffectInc * 100).ToString() + " / "
            + ((int)3 * buffEffectInc * 100).ToString() + " / "
            + ((int)4 * buffEffectInc * 100).ToString() + " / "
            + ((int)5 * buffEffectInc * 100).ToString()
            + " %.";
        maxCount = 5;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        if (currentCount >= 2)
        {
            statSkript.buffInc.RemoveModifierMultiply(0.2f * (currentCount - 1));
        }
        statSkript.buffInc.AddModifierMultiply(0.2f * currentCount);
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();
        Debug.Log("Trying to Remove Additional Buff Effect");
        if (currentCount >= 1)
        {
            Debug.Log("Current Count higher than 1");
            statSkript.buffInc.RemoveModifierMultiply(0.2f * currentCount);
        }
    }
}