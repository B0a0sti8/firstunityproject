using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGainLife : SkillPrefabOld
{
    public float healing = 75f;

    public override void Start()
    {
        ownCooldownTimeBase = 10f;

        base.Start();
    }

    public override void Update()
    {
        tooltipSkillDescription = "Gain <color=green>" + healing + " Life</color>\n" +
            "Oooh yeaaah!";

        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        DoHealing(healing);
    }
}