using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree1Talent0_IncreasedActionSpeed : Talent
{
    private SummonerClass mySummonerClass;

    protected override void Awake()
    {
        talentName = "Increased Action Speed";
        maxCount = 5;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        if (currentCount >= 2)
        {
            statSkript.actionSpeed.RemoveModifierMultiply(0.2f * (currentCount - 1));
        }
        statSkript.actionSpeed.AddModifierMultiply(0.2f * currentCount);
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();
        Debug.Log("Trying to Remove Additional Action Speed");
        if (currentCount >= 1)
        {
            Debug.Log("Current Count higher than 1"); 
            statSkript.actionSpeed.RemoveModifierMultiply(0.2f * currentCount);
        }
    }
}