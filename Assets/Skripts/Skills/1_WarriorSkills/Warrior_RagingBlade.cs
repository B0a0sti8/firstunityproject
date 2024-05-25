using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_RagingBlade : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;
    float skillRadiusBase;

    public override void Start()
    {
        base.Start();
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Whirling your blade around you, you deal damage to all enemies in range. Teleport to target enemy and deals double damage, when you used Sweeping Slash in the last 5 seconds. ";

        ownCooldownTimeBase = 0f;

        needsTargetEnemy = true;
        targetsEnemiesOnly = true;
        canSelfCastIfNoTarget = true;

        skillRadiusBase = 5f;
        skillRange = 10f;
        myAreaType = AreaType.CircleAroundTarget;

        damageBase = 200;
    }

    public override void StartCasting()
    {
        skillRadius = skillRadiusBase + myWarriorClass.meleeSkillsRadiusIncrease;
        base.StartCasting();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        TeleportToTargetEnemy();

        float damageModified = damageBase * playerStats.dmgInc.GetValue();
        if (myWarriorClass.hasSweepingSlashComboBuff) { damageModified *= 2; myWarriorClass.hasSweepingSlashComboBuff = false; } // Bildet combo mit WideSlash: Doppelter Schaden.

        DealDamage(damageModified);
        //PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_SweepingSlash_ComboBuff", false);

    }

    public void TeleportToTargetEnemy()
    {
        // Erzeugt zufällige Koordinaten
        float x = Random.Range(0.5f, 1.5f);
        float y = Random.Range(0.5f, 1.5f);
        float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
        float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
        Vector2 posi = new Vector2();

        if (targetSnapShot != null) posi = (Vector2)targetSnapShot.transform.position + new Vector2(x * signx, y * signy);
        else posi = (Vector2)PLAYER.transform.position + new Vector2(x * signx, y * signy);
       
        PLAYER.transform.position = posi;
    }
}
