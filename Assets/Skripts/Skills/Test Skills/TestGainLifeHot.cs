using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGainLifeHot : SkillPrefab
{
    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        StartCoroutine(Hot(2f, 5));
    }

    IEnumerator Hot(float time, int healing)
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(time);
            player.currentHealth += healing;
        }
    }
}