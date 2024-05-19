using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpeedBoost : SkillPrefab
{
    public float duration;
    public float value;

    public Sprite buffImage;
    SpeedBoostBuff buff = new SpeedBoostBuff();
    AttackSpeedBoostBuff buff2 = new AttackSpeedBoostBuff();

    public override void Start()
    {
        ownCooldownTimeBase = 0.2f;
        duration = 10f;
        value = 5f;

        base.Start();
    }

    public override void Update()
    {
        tooltipSkillDescription = "Boosts your <color=lightblue>MovementSpeed</color> by <color=lightblue>" + value + "</color> for <color=yellow>" + duration + "s</color>\n" +
            "Boosts your <color=lightblue>AttackSpeed</color> by <color=lightblue>" + value + "</color> for <color=yellow>" + duration + "s</color>";

        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        Buff clone = buff.Clone();
        PLAYER.GetComponent<BuffManager>().AddBuff(clone, buffImage, duration, value);

        Buff clone2 = buff2.Clone();
        PLAYER.GetComponent<BuffManager>().AddBuff(clone2, buffImage, duration, value);
    }
}