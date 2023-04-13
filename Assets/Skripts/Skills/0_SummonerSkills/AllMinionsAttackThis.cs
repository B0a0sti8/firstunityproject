using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AllMinionsAttackThis : SkillPrefab
{
    public override void Start()
    {
        base.Start();
        myClass = "Summoner";

        skillRange = 30f;

        hasOwnCooldown = true;
        hasGlobalCooldown = false;
        ownCooldownTimeBase = 3f;
    }
    public override void Update()
    {
        tooltipSkillDescription = "All Minions attack this Target!";

        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        PLAYER.GetComponent<PlayerStats>().myMinions.ForEach(k => k.GetComponent<MinionPetAI>().isInFight = true);
    }
}
