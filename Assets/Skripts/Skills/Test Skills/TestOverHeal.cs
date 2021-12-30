using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOverHeal : SkillPrefab
{
    public float overhealing = 50f;

    public override void MasterETStuff()
    {
        skillDescription = "Gain <color=green>" + overhealing + " Health</color>\n" +
            "No Overheal at the moment tho";
        base.MasterETStuff();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        
        //playerStats.maxHealth.GetValue() += 50;
        playerStats.currentHealth += overhealing;
        //StartCoroutine(Wait(30));
        //IEnumerator Wait(float time)
        //{
        //    yield return new WaitForSeconds(time);
        //    player.maxHealth -= 50;
        //}
    }
}