using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_SweepingSlash : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;
    float skillRadiusBase;

    public override void Start()
    {
        base.Start();

        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Whirling your blade around you, you deal damage to all enemies in range. Deals double damage, when you used WildSlash in the last 5 seconds. ";

        ownCooldownTimeBase = 0f;

        needsTargetEnemy = false;
        targetsEnemiesOnly = true;
        isCastOnSelf = true;

        skillRadiusBase = 5f;
        coneAOEAngle = 120;
        myAreaType = AreaType.CircleAroundTarget;

        damageBase = 150;
    }

    public override void StartCasting()
    {
        skillRadius = skillRadiusBase + myWarriorClass.meleeSkillsRadiusIncrease;
        base.StartCasting();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        float damageModified = damageBase * playerStats.dmgInc.GetValue();
        if (myWarriorClass.hasSweepingSlashComboBuff) { damageModified *= 2; myWarriorClass.hasSweepingSlashComboBuff = false; } // Bildet combo mit WideSlash: Doppelter Schaden.

        DealDamage(damageModified);
        PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_SweepingSlash_ComboBuff", false);
    }
}
