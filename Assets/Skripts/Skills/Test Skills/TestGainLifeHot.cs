using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGainLifeHot : SkillPrefab
{
    [Header("Hot-Stats")]
    public float instantHealing = 50f;
    public float tickTime = 2f;
    public int tickHealing = 5;
    public float duration = 10f;

    public Sprite buffImage;
    HoTBuff buff = new HoTBuff();

    public override void Start()
    {
        ownCooldownTimeBase = 2f;

        base.Start();
    }

    public override void Update()
    {
        tooltipSkillDescription = "Gain <color=green>" + instantHealing + " Life</color>.\n" +
            "HOT:\n" +
            "<color=green>" + tickHealing + " Health</color> every <color=yellow>" + tickTime + "s</color>\n" +
            "Duration: <color=yellow>" + duration + "s</color>";

        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        DoHealing(instantHealing);

        Buff clone = buff.Clone();
        PLAYER.GetComponent<BuffManager>().AddBuff(clone, buffImage, duration, tickTime, tickHealing);
    }
}