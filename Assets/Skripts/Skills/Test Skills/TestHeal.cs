using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHeal : SkillPrefab
{
    public float heal = 100f;

    public override void Start()
    {
        base.Start();
        castTimeOriginal = 2;
        needsTargetAlly = true;
        canSelfCastIfNoTarget = true;
        skillRadius = 10f;
        skillRange = 10f;
        isAOECircle = true;
}

    public override void Update()
    {
        tooltipSkillDescription = "Heal <color=orange>" + heal + " Damage</color> to any target.";

        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        DoHealing(heal);
    }
}