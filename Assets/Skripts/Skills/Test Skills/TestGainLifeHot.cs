using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGainLifeHot : SkillPrefab
{
    [Header("Hot-Stats")]
    public float hotTime = 2f;
    public int hotHealing = 5;
    public float duration = 10f;

    public override void MasterETStuff()
    {
        skillDescription = "HOT:\n" +
            "<color=green>" + hotHealing + " Health</color> every <color=yellow>" + hotTime + "s</color>\n" +
            "Duration: <color=yellow>" + duration + "s</color>";
        base.MasterETStuff();
    }

    public Buff hot;
    public override void SkillEffect()
    {
        base.SkillEffect();

        Debug.Log("AddBuff(hot)");
        PLAYER.GetComponent<BuffManager>().AddBuff(hot, duration);

        //StartCoroutine(Hot(hotTime, hotHealing));
    }

    //IEnumerator Hot(float time, int healing)
    //{
    //    for (int i = 0; i < buffDuration / hotTime; i++)
    //    {
    //        yield return new WaitForSeconds(time);
    //        playerStats.currentHealth += healing;
    //    }
    //}
}