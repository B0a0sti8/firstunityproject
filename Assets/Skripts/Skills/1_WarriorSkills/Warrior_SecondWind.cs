using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_SecondWind : SkillPrefab
{
    WarriorClass myWarriorClass;
    private float baseHealing;

    public override void Start()
    {
        base.Start();
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Using a short break in your battle, you gather strength to keep fighting";

        ownCooldownTimeBase = 0f;

        isCastOnSelf = true;

        myAreaType = AreaType.SingleTargetSelf;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        baseHealing = myWarriorClass.secondWindPercentMissingHeal * (playerStats.maxHealth.GetValue() - playerStats.currentHealth.Value);
        float healingModified = baseHealing * playerStats.healInc.GetValue();

        DoHealing(healingModified);
    }
}