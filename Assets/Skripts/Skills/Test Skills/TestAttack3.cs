using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack3 : SkillPrefab
{
    public float baseDamage = 100f;
    public float comboDamage = 150f;

    public override void MasterETStuff()
    {
        tooltipSkillDescription = "Deal <color=orange>" + baseDamage + " Damage</color> to any target.\n" +
            "<color=lightblue>Combo - TestAttack2:</color> Deal <color=orange>" + comboDamage + " Damage</color> instead.";

        base.MasterETStuff();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        
        Debug.Log("Attack3: " + baseDamage + " or " + comboDamage + " Damage");
        DealDamage(comboDamage);
        //GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += damage;
    }
}