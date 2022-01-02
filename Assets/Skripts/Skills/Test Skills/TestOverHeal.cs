using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOverHeal : SkillPrefab
{
    public float duration = 30f;
    public float value = 100f; // Overhealing

    public Sprite buffImage;
    MaxHealthBuff buff = new MaxHealthBuff();

    public override void MasterETStuff()
    {
        skillDescription = "Gain <color=green>+" + value + " Max-Health</color> for <color=yellow>" + duration + "s</color>";
        base.MasterETStuff();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        Buff clone = buff.Clone();
        PLAYER.GetComponent<BuffManager>().AddBuff(clone, buffImage, duration, value);
    }
}