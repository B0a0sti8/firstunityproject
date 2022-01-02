using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpeedBoost : SkillPrefab
{
    public float duration = 3f;
    public float value = 5f;

    public Sprite buffImage;
    SpeedBoostBuff buff = new SpeedBoostBuff();

    public override void MasterETStuff()
    {
        skillDescription = "Boosts your <color=lightblue>Speed</color> by <color=lightblue>" + value + "</color> for <color=yellow>" + duration + "s</color>";
        base.MasterETStuff();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        // AddBuff
        Buff clone = buff.Clone();
        PLAYER.GetComponent<BuffManager>().AddBuff(clone, buffImage, duration, value);
    }
}