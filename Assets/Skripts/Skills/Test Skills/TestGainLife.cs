using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestGainLife : SkillPrefab
{
    public override void MasterETStuff()
    {
        skillDescription = "Gain <color=#f00>Life</color>\nOooh yeaaah!\nOwnCooldownTimeLeft: <color=yellow>" + ownCooldownTimeLeft + "</color>";
        base.MasterETStuff();
    }

    //public override void Start()
    //{
    //    skillDescription = "Gain <color=#f00>Life</color>\nOooh yeaaah!";

    //    base.Start();
    //}

    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        playerStats.currentHealth += 20;
    }
}