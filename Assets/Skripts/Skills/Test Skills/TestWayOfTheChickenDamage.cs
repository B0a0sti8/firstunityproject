using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWayOfTheChickenDamage : SkillPrefab
{
    [Header("Hot-Stats")]
    public float damage = 100f;
    public float tickTime = 1f;
    public int tickDamage = 5;
    public float duration = 10f;

    public Sprite buffImage;
    TheWayOfTheChickenDamageDebuff buff = new TheWayOfTheChickenDamageDebuff();

    

    public override void Start()
    {
        base.Start();
        isAOEFrontCone = true;
        needsTargetAlly = false;
        needsTargetEnemy = false;
        targetsEnemiesOnly = true;
        skillRadius = 5;
    }

    public override void Update()
    {
        tooltipSkillDescription = "Deal <color=orange>" + damage + " Damage</color> to all targets in front of you.";

        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        Debug.Log("Attack1: " + damage + " Damage");
        DealDamage(damage);

        Buff clone = buff.Clone();

        for (int i = 0; i < currentTargets.Count; i++)
        {
            currentTargets[i].GetComponent<BuffManagerNPC>().AddBuff(clone, buffImage, duration, tickTime, tickDamage);
        }
    }
}
