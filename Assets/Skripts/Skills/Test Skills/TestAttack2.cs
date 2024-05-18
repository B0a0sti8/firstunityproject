using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack2 : SkillPrefabOld
{
    public float baseDamage = 100f;
    public float comboDamage = 120f;

    public override void Update()
    {
        tooltipSkillDescription = "Deal <color=orange>" + baseDamage + " Damage</color> to any target.\n" +
            "<color=lightblue>Combo - TestAttack1:</color> Deal <color=orange>" + comboDamage + " Damage</color> instead.";

        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        Debug.Log("Attack2: " + baseDamage + " or " + comboDamage + " Damage");
        DealDamage(comboDamage);
    }
}