using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_WideSlash : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;
    float skillRadiusBase;

    public override void Start()
    {
        base.Start();

        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();

        ownCooldownTimeBase = 0f;
        hasGlobalCooldown = false;

        needsTargetEnemy = false;
        targetsEnemiesOnly = true;

        skillRadiusBase = 3f;
        coneAOEAngle = 120;
        myAreaType = AreaType.Front;

        damageBase = 200;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        skillRadius = skillRadiusBase + myWarriorClass.meleeSkillsRadiusIncrease;
        float damageModified = damageBase;
        DealDamage(damageModified);
    }
}
