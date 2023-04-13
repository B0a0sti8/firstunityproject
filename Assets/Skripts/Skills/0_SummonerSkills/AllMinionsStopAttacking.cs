using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AllMinionsStopAttacking : SkillPrefab
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
        tooltipSkillDescription = "All Minions stop attacking!";

        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        PLAYER.GetComponent<PlayerStats>().myMinions.ForEach(k => k.GetComponent<MinionPetAI>().isInFight = false);
    }
}
