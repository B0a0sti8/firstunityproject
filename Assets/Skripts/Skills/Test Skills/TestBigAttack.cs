using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBigAttack : SkillPrefab
{
    public float damage = 400f;

    public override void Start()
    {
        ownCooldownTimeBase = 20f;

        base.Start();
    }

    public override void Update()
    {
        tooltipSkillDescription = "Deal <color=orange>" + damage + " Damage</color> to any target.";

        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        
        Debug.Log("Activate BigAttack: " + damage + " Damage");
        DealDamage(damage);
    }
}