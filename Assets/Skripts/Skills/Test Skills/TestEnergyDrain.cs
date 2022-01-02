using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnergyDrain : SkillPrefab
{
    public float damage = 200f;

    public override void MasterETStuff()
    {
        skillDescription = "Deal <color=orange>" + damage + " damage</color> to any target.\n" +
            "<color=green>Gain Life</color> equal to half the damage dealt.";

        base.MasterETStuff();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        
        Debug.Log("Activate EnergyDrain: " + damage + " Damage");
        DealDamage(damage);
        playerStats.currentHealth += damage / 2;

        GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += damage;
    }
}