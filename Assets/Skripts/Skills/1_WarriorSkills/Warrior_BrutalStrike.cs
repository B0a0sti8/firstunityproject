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
        if (myWarriorClass.hasBrutalStrikeComboBuff) { damageModified *= 2; myWarriorClass.hasBrutalStrikeComboBuff = false; }  // Bildet combo mit WildStrike: Doppelter Schaden.

        DealDamage(damageModified);
        PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_BrutalStrike_ComboBuff", false);
    }
}
