using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGainLife : SkillPrefab
{
    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        player.currentHealth += 20;
    }
}