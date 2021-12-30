using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestGainLife : SkillPrefab
{
    public float healing = 75f;

    public override void MasterETStuff()
    {
        skillDescription = "Gain <color=green>" + healing + " Life</color>\n" +
            "Oooh yeaaah!\n" +
            "\n" +
            "OwnCooldownTimeLeft: <color=yellow>" + ownCooldownTimeLeft + "</color>";

        base.MasterETStuff();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        
        playerStats.currentHealth += healing;
    }
}