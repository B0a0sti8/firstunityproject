using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOverHeal : SkillPrefab
{
    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        //playerStats.maxHealth.GetValue() += 50;
        playerStats.currentHealth += 50;
        //StartCoroutine(Wait(30));
        //IEnumerator Wait(float time)
        //{
        //    yield return new WaitForSeconds(time);
        //    player.maxHealth -= 50;
        //}
    }
}