using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_DeadlyThrust : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;

    public override void Start()
    {
        base.Start();
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Deal high damage with a single stab, that is woven in between your other skills. Combos of Hail of stabs. ";

        ownCooldownTimeBase = 0f;

        targetsEnemiesOnly = true;
        needsTargetEnemy = true;
        hasGlobalCooldown = false;

        skillRange = 3;
        myAreaType = AreaType.SingleTarget;

        damageBase = 300;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        float damageModified = damageBase * playerStats.dmgInc.GetValue();

        if (myWarriorClass.hasStingCombo2Buff) // Bildet combo mit WildStrike: Doppelter Schaden.
        {
            damageModified *= 3;
            myWarriorClass.hasStingCombo2Buff = false;
            PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_StingComboBuff2", false);

            DealDamage(damageModified);
        }
        else DealDamage(damageModified);
    }
}
