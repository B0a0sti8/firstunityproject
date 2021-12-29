using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGainLifeHot : SkillPrefab
{
    [Header("Hot-Stats")]
    public float hotTime = 2f;
    public int hotHealing = 5;
    public float buffDuration = 10f;

    public override void MasterETStuff()
    {
        skillDescription = "HOT:\n" +
            "<color=#f00>" + hotHealing + " Health</color> every <color=yellow>" + hotTime + "s</color>\n" +
            "Duration: <color=yellow>" + buffDuration + "s</color>";
        base.MasterETStuff();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        StartCoroutine(Hot(hotTime, hotHealing));
    }

    IEnumerator Hot(float time, int healing)
    {
        for (int i = 0; i < healing; i++)
        {
            yield return new WaitForSeconds(time);
            playerStats.currentHealth += healing;
        }
    }
}