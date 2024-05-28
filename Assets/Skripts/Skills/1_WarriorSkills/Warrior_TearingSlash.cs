using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_TearingSlash : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float damageBase;
    float skillRadiusBase;

    public override void Start()
    {
        base.Start();
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "";

        ownCooldownTimeBase = 0f;

        targetsEnemiesOnly = true;
        isCastOnSelf = true;

        skillRadiusBase = 5f;
        myAreaType = AreaType.CircleAroundTarget;

        damageBase = 200;
    }

    public override void StartCasting()
    {
        skillRadius = skillRadiusBase + myWarriorClass.meleeSkillsRadiusIncrease;
        //targetSnapShot = PLAYER;

        base.StartCasting();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        float damageModified = damageBase * playerStats.dmgInc.GetValue();

        if (myWarriorClass.hasTearingSlashBleed)
        {
            foreach (GameObject target in currentTargets)
            {
                GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(target.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "Warrior_TearingSlashDoT", "Warrior_TearingSlashDoT", true, 9, 3, damageModified / 3);
            }
        }

        DealDamage(damageModified);
    }
}
