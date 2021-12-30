using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpeedBoost : SkillPrefab
{
    public override void MasterETStuff()
    {
        skillDescription = "Boosts your <color=lightblue>Speed</color> stat for some time";
        base.MasterETStuff();
    }

    public Buff testBuff1;
    public override void SkillEffect()
    {
        base.SkillEffect();
       
        //Debug.Log("TestSpeedBoost: + 5 Movement");
        //playerController._Speed += 5;
        //StartCoroutine(Wait(10));
        //IEnumerator Wait(float time)
        //{
        //    yield return new WaitForSeconds(time);
        //    playerController._Speed -= 5;
        //}

        // AddBuff
        PLAYER.GetComponent<BuffManager>().AddBuff(testBuff1);
    }
}