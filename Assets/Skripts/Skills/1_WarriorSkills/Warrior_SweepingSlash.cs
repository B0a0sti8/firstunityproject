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
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Whirling your blade around you, you deal damage to all enemies in range. Deals double damage, when you used WildSlash in the last 5 seconds. ";

        ownCooldownTimeBase = 0f;

        needsTargetEnemy = false;
        targetsEnemiesOnly = true;
        isCastOnSelf = true;

        skillRadiusBase = 5f;
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

        PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_SlashComboBuff2", false);
        if (myWarriorClass.hasSlashCombo1Buff) // Bildet combo mit WideSlash: Doppelter Schaden.
        { 
            damageModified *= 2; 
            myWarriorClass.hasSlashCombo1Buff = false;
            myWarriorClass.hasSlashCombo2Buff = true;
            PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_SlashComboBuff1", false);
            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(PLAYER.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "Warrior_SlashComboBuff2", "Warrior_SlashComboBuff2", false, 5, 0, 0);
        }

        DealDamage(damageModified);
    }
}
