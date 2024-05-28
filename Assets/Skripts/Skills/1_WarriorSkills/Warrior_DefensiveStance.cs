using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_DefensiveStance : SkillPrefab
{
    WarriorClass myWarriorClass;

    public override void Start()
    {
        base.Start();
        myClass = "Warrior";
        myWarriorClass = PLAYER.transform.Find("SkillManager").Find("Warrior").GetComponent<WarriorClass>();
        tooltipSkillDescription = "Takes a defensive stance, gaining toughness and increased healing, but reduced tempo and damage. ";

        hasGlobalCooldown = false;
        ownCooldownTimeBase = 3f;
        isCastOnSelf = true;
        myAreaType = AreaType.SingleTargetSelf;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_OffensiveStanceBuff", false, true);
        myWarriorClass.offensiveStanceOn = false;
        if (myWarriorClass.defensiveStanceOn)
        {
            PLAYER.GetComponent<BuffManager>().RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "Warrior_DefensiveStanceBuff", false, true);
            myWarriorClass.defensiveStanceOn = false;
        }
        else
        {
            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(PLAYER.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "Warrior_DefensiveStanceBuff", "Warrior_DefensiveStanceBuff", false, 3600, 0, 0.1f);
            myWarriorClass.defensiveStanceOn = true;
        }
    }
}
