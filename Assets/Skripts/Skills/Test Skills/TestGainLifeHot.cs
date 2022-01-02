using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGainLifeHot : SkillPrefab
{
    [Header("Hot-Stats")]
    public float tickTime = 2f;
    public int tickHealing = 5;
    public float duration = 10f;
    public Sprite buffImage;
    HoTBuff buff = new HoTBuff();

    public override void MasterETStuff()
    {
        skillDescription = "HOT:\n" +
            "<color=green>" + tickHealing + " Health</color> every <color=yellow>" + tickTime + "s</color>\n" +
            "Duration: <color=yellow>" + duration + "s</color>";
        base.MasterETStuff();
    }

    public Buff hot;
    public override void SkillEffect()
    {
        base.SkillEffect();
        Debug.Log("AddBuff(hot)");
        Buff clone = buff.Clone();
        PLAYER.GetComponent<BuffManager>().AddBuff(clone, duration, buffImage, tickTime, tickHealing);
    }
}