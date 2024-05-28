using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_WideSlash : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;
    float skillRadiusBase;

    public override void Start()
    {
        base.Start();
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "A slash, dealing damage to enemies in front of you. Makes your next Sweeping Slash within 5 seconds deal double damage. ";

        ownCooldownTimeBase = 0f;

        targetsEnemiesOnly = true;
        hasGlobalCooldown = true;

        skillRadiusBase = 5f;
        coneAOEAngle = 120;
        myAreaType = AreaType.Front;

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
        float damageModified = damageBase * playerStats.dmgInc.GetValue();

        DealDamage(damageModified);

        myWarriorClass.hasSlashCombo1Buff = true;
        PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_SlashComboBuff1", false);
        GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(PLAYER.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "Warrior_SlashComboBuff1", "Warrior_SlashComboBuff1", false, 5, 0, 0);
    }
}
