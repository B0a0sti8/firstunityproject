using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_BrutalStrike : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;

    public override void Start()
    {
        base.Start();
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Strikes your target. Deals double damage, when you used WildStrike in the last 5 seconds. ";

        ownCooldownTimeBase = 0f;

        targetsEnemiesOnly = true;
        needsTargetEnemy = true;

        skillRange = 3;
        myAreaType = AreaType.SingleTarget;

        damageBase = 250;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        
        float damageModified = damageBase * playerStats.dmgInc.GetValue();

        PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_StrikeComboBuff2", false);
        if (myWarriorClass.hasStrikeCombo1Buff) // Bildet combo mit WildStrike: Doppelter Schaden.
        { 
            damageModified *= 2; 
            myWarriorClass.hasStrikeCombo1Buff = false;
            myWarriorClass.hasStrikeCombo2Buff = true;
            PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_StrikeComboBuff1", false);
            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(PLAYER.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "Warrior_StrikeComboBuff2", "Warrior_StrikeComboBuff2", false, 5, 0, 0);
        }
        DealDamage(damageModified);
    }
}
