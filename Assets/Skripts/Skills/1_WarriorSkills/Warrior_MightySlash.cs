using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_MightySlash : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;
    float skillRadiusBase;
    float stunDurationBase;

    public override void Start()
    {
        base.Start();
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Stuns all enemies around you. Combos from Sweeping Slash. ";

        ownCooldownTimeBase = 0f;

        targetsEnemiesOnly = true;
        isCastOnSelf = true;

        skillRadiusBase = 5f;
        myAreaType = AreaType.CircleAroundTarget;

        damageBase = 100;
        stunDurationBase = 2;
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
        float stunDuration = stunDurationBase * playerStats.skillDurInc.GetValue();

        if (myWarriorClass.hasSlashCombo2Buff)// Bildet combo mit WideSlash: Doppelter Schaden.
        {
            damageModified *= 2;
            stunDuration *= 2;
            myWarriorClass.hasSlashCombo2Buff = false;

            PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_SlashCombo2Buff", false);


        }
        foreach (GameObject target in currentTargets)
        {
            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(target.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "StunnedEffectOnEnemies", "StunnedEffectOnEnemies", false, stunDuration, 0, 0);
        }

        DealDamage(damageModified);
    }
}
