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

        foreach (var mn in PLAYER.GetComponent<PlayerStats>().myMinions)
        {
            mn.TryGet(out NetworkObject minio);
            MinionPetAI minion = minio.GetComponent<MinionPetAI>();
            minion.isInFight = true;
            minion.ForceAggroToTarget(PLAYER.GetComponent<InteractionCharacter>().focus.transform);
        }

        foreach (var mn in PLAYER.GetComponent<PlayerStats>().myMainMinions)
        {
            mn.TryGet(out NetworkObject minio);
            MinionPetAI minion = minio.GetComponent<MinionPetAI>();
            minion.isInFight = true;
            if (PLAYER.GetComponent<InteractionCharacter>().focus.transform!= null)
            {
                minion.ForceAggroToTarget(PLAYER.GetComponent<InteractionCharacter>().focus.transform);
            }
        }
        //PLAYER.GetComponent<PlayerStats>().myMinions.ForEach(k => k.GetComponent<MinionPetAI>().isInFight = true);
    }
}
