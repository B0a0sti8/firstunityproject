using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestGainLife : SkillPrefab
{
    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        playerStats.currentHealth += 20;
    }
}