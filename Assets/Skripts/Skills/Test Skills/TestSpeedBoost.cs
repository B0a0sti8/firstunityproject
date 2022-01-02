using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpeedBoost : SkillPrefab
{
    public float duration = 3f;
    SpeedBoostBuff buff = new SpeedBoostBuff();
    public Sprite buffImage;

    public override void MasterETStuff()
    {
        skillDescription = "Boosts your <color=lightblue>Speed</color> stat for some time";
        base.MasterETStuff();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        // AddBuff
        Buff clone = buff.Clone();
        PLAYER.GetComponent<BuffManager>().AddBuff(clone, duration, buffImage);
    }
}