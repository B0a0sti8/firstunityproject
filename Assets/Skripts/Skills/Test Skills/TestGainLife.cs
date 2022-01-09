using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGainLife : SkillPrefab
{
    public float healing = 75f;

    public override void Start()
    {
        ownCooldownTimeBase = 10f;

        base.Start();
    }

    public override void MasterETStuff()
    {
        tooltipSkillDescription = "Gain <color=green>" + healing + " Life</color>\n" +
            "Oooh yeaaah!";

        base.MasterETStuff();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        DoHealing(healing);
    }
}