using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree1Talent0_IncreasedActionSpeed : Talent
{
    private SummonerClass mySummonerClass;
    private float actionSpeedModifier;

    protected override void Awake()
    {
        actionSpeedModifier = 0.2f;
        talentName = "Increased Action Speed";
        talentDescription = " Increases action speed by " 
            + ((int)actionSpeedModifier * 100).ToString() + " / " 
            + ((int)2 * actionSpeedModifier * 100).ToString() + " / " 
            + ((int)3 * actionSpeedModifier * 100).ToString() + " / " 
            + ((int)4 * actionSpeedModifier * 100).ToString() + " / "
            + ((int)5 * actionSpeedModifier * 100).ToString() 
            +  " %.";
        maxCount = 5;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        if (currentCount >= 2)
        {
            statSkript.actionSpeed.RemoveModifierMultiply(actionSpeedModifier * (currentCount - 1));
        }
        statSkript.actionSpeed.AddModifierMultiply(actionSpeedModifier * currentCount);
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();
        Debug.Log("Trying to Remove Additional Action Speed");
        if (currentCount >= 1)
        {
            Debug.Log("Current Count higher than 1"); 
            statSkript.actionSpeed.RemoveModifierMultiply(actionSpeedModifier * currentCount);
        }
    }
}