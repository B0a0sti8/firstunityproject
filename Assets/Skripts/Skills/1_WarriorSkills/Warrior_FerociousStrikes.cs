using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_FerociousStrikes : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;

    public override void Start()
    {
        base.Start();
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Strikes furiously after your target, dealing high amounts of damage. Combos from brutal strike. ";

        ownCooldownTimeBase = 0f;

        targetsEnemiesOnly = true;
        needsTargetEnemy = true;

        skillRange = 3;
        myAreaType = AreaType.SingleTarget;

        damageBase = 200;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        float damageModified = damageBase * playerStats.dmgInc.GetValue();

        if (myWarriorClass.hasStrikeCombo2Buff) // Bildet combo mit WildStrike: Doppelter Schaden.
        {
            damageModified *= 2;
            myWarriorClass.hasStrikeCombo2Buff = false;
            PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_StrikeComboBuff2", false);

            StartCoroutine(StrikeMultipleTimes(3, damageModified));
        }
        else StartCoroutine(StrikeMultipleTimes(3, damageModified));
    }

    IEnumerator StrikeMultipleTimes(int numberOfStrikes, float strikeDamage)
    {
        for (int i = 0; i < numberOfStrikes; i++)
        {
            DealDamage(strikeDamage);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
