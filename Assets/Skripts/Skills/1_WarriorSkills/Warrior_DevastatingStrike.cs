using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_DevastatingStrike : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;
    private float debuffValueBase;

    public override void Start()
    {
        base.Start();
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Brutally strikes your target, increasing its damage taken from all sources. ";

        ownCooldownTimeBase = 0f;

        targetsEnemiesOnly = true;
        needsTargetEnemy = true;

        skillRange = 3;
        myAreaType = AreaType.SingleTarget;

        damageBase = 500;
        debuffValueBase = 0.1f;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        float damageModified = damageBase * playerStats.dmgInc.GetValue();

        DealDamage(damageModified);
        float debuffValue = debuffValueBase * playerStats.debuffInc.GetValue();

        GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(currentTargets[0].GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "Warrior_DevastatingStrikeDebuff", "Warrior_DevastatingStrikeDebuff", false, 8, 0, debuffValue);
    }
}
