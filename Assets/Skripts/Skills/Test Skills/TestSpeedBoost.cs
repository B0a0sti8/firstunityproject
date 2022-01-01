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

    public Buff speedBoost;
    public override void SkillEffect()
    {
        base.SkillEffect();

        // AddBuff
        Debug.Log("AddBuff(speedBoost)");
        PLAYER.GetComponent<BuffManager>().AddBuff(speedBoost);
    }
}