using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestHeal : SkillPrefab
{
    public float heal = 100f;

    public override void Start()
    {
        base.Start();
        castTimeOriginal = 2;
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