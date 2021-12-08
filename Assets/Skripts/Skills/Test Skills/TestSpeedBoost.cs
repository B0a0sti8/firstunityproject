using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpeedBoost : SkillPrefab
{
    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        //Debug.Log("TestSpeedBoost: + 5 Movement");
        playerController._Speed += 5;
        StartCoroutine(Wait(10));
        IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
            playerController._Speed -= 5;
        }
    }
}
