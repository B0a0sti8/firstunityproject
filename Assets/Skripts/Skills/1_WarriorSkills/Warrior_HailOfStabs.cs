using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_HailOfStabs : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;

    public override void Start()
    {
        base.Start();
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Stabs your target multiple times. Combos of sting. ";

        ownCooldownTimeBase = 0f;

        targetsEnemiesOnly = true;
        needsTargetEnemy = true;
        hasGlobalCooldown = false;

        skillRange = 3;
        myAreaType = AreaType.SingleTarget;

        damageBase = 100;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        float damageModified = damageBase * playerStats.dmgInc.GetValue();

        if (myWarriorClass.hasStingCombo1Buff) // Bildet combo mit WildStrike: Doppelter Schaden.
        {
            damageModified *= 2;
            myWarriorClass.hasStingCombo1Buff = false;
            myWarriorClass.hasStingCombo2Buff = true;
            PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_StingComboBuff1", false);
            PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_StingComboBuff2", false);

            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(PLAYER.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "Warrior_StingComboBuff2", "Warrior_StingComboBuff2", false, 5, 0, 0);

            StartCoroutine(StabMultipleTimes(3, damageModified));
        }
        else DealDamage(damageModified);
    }

    IEnumerator StabMultipleTimes(int numberOfStabs, float stabDamage)
    {
        for (int i = 0; i < numberOfStabs; i++)
        {
            DealDamage(stabDamage);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
