using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonerTree2Talent2_IncreasedSkillDuration : Talent
{
    private SummonerClass mySummonerClass;
    private float durationInc;

    protected override void Awake()
    {
        durationInc = 0.2f;
        talentName = "Increased Skill Duration";
        talentDescription = " Increases action speed by "
         + (durationInc * 100).ToString() + " / "
         + (2 * durationInc * 100).ToString() + " / "
         + (3 * durationInc * 100).ToString() + " / "
         + (4 * durationInc * 100).ToString() + " / "
         + (5 * durationInc * 100).ToString()
         + " %.";
        maxCount = 5;
        base.Awake();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        Debug.Log("Trying to Add Additional Skill Duration");
        if (currentCount >= 2)
        {
            statSkript.skillDurInc.RemoveModifierMultiply(durationInc * (currentCount - 1));
        }
        statSkript.skillDurInc.AddModifierMultiply(durationInc * currentCount);
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();
        Debug.Log("Trying to Remove Additional Skill Duration");
        if (currentCount >= 1)
        {
            Debug.Log("Current Count higher than 1");
            statSkript.skillDurInc.RemoveModifierMultiply(durationInc* currentCount);
        }
    }
}