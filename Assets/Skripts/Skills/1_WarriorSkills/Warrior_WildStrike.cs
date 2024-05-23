using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_WildStrike : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;

    public override void Start()
    {
        base.Start();

        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Strikes your target. Doubles the damage of your next Brutal Strike within 5 seconds.";

        ownCooldownTimeBase = 0f;

        targetsEnemiesOnly = true;
        needsTargetEnemy = true;

        skillRange = 3;
        myAreaType = AreaType.SingleTarget;

        damageBase = 300;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        float damageModified = damageBase * playerStats.dmgInc.GetValue();

        DealDamage(damageModified);
        myWarriorClass.hasBrutalStrikeComboBuff = true;

        GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(PLAYER.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "Warrior_BrutalStrike_ComboBuff", "Warrior_BrutalStrike_ComboBuff", false, 5, 0, 0);
    }
}
