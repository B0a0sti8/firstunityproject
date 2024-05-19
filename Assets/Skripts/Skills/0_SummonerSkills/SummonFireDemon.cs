using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonFireDemon : SkillPrefab
{
    float impactDamage;

    public override void Start()
    {
        impactDamage = 1200;
        myAreaType = AreaType.CirclePlacable;
        hasGlobalCooldown = true;
        skillRadius = 10f;
        targetsEnemiesOnly = true;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        foreach (GameObject target in currentTargets) Debug.Log("Target: " + target);
        DealDamage(impactDamage);
    }
}