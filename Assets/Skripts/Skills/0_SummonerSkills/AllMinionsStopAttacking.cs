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

        //foreach (var mn in PLAYER.GetComponent<PlayerStats>().myMinions)
        //{
        //    mn.TryGet(out NetworkObject minio);
        //    minio.GetComponent<MinionPetAI>().isInFight = false;
        //}

        //PLAYER.GetComponent<PlayerStats>().myMinions.ForEach(k => k.GetComponent<MinionPetAI>().isInFight = false);

        foreach (var mn in PLAYER.GetComponent<PlayerStats>().myMinions)
        {
            mn.TryGet(out NetworkObject minio);
            MinionPetAI minion = minio.GetComponent<MinionPetAI>();
            minion.isInFight = false;
            minion.DisableForcedAggro();
        }

        foreach (var mn in PLAYER.GetComponent<PlayerStats>().myMainMinions)
        {
            mn.TryGet(out NetworkObject minio);
            MinionPetAI minion = minio.GetComponent<MinionPetAI>();
            minion.isInFight = false;
            minion.DisableForcedAggro();
        }
    }
}
