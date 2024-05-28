using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_Sting : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;

    public override void Start()
    {
        base.Start();
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Swiftly stings your target. Can be used in between other skills. ";

        ownCooldownTimeBase = 0f;

        targetsEnemiesOnly = true;
        needsTargetEnemy = true;
        hasGlobalCooldown = false;

        skillRange = 3;
        myAreaType = AreaType.SingleTarget;

        damageBase = 150;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        float damageModified = damageBase * playerStats.dmgInc.GetValue();
        DealDamage(damageModified);

        myWarriorClass.hasStingCombo1Buff = true;
        PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_StingComboBuff1", false);
        GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(PLAYER.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "Warrior_StingComboBuff1", "Warrior_StingComboBuff1", false, 5, 0, 0);
    }
}
