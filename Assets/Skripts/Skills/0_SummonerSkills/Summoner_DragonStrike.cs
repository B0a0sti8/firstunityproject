using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Summoner_DragonStrike : SkillPrefab
{
    SummonerClass mySummonerClass;
    float buffDuration;
    float mainStatBaseValueAddBuff;

    public override void Start()
    {
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
        animationTime = 5f;
        base.Start();
        myClass = "Summoner";
        tooltipSkillDescription = "Ultimate Spell: Sacrifice your minions to buff other players and yourself.";
        isUltimateSpell = true;
        ultimateSpellName = "The Great Sacrifice";
        isCastOnSelf = true;

        buffDuration = 15;

        mainStatBaseValueAddBuff = 1;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        int buffCounter = 0;

        for (int i = playerStats.myMinions.Count - 1; i >= 0; i--)
        {
            GameObject myMinion = playerStats.myMinions[i];
            DespawnMinionServerRpc(myMinion.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>());
            buffCounter++;
        }

        foreach (GameObject myHelperPlayer in myUltimateSpellHelpers)
        {
            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(myHelperPlayer.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "TheGreatSacrificeBuff", "TheGreatSacrificeBuff", false, buffDuration, 0, buffCounter * mainStatBaseValueAddBuff);
        }
    }

    [ServerRpc]
    public void DespawnMinionServerRpc(NetworkObjectReference minionRef, NetworkObjectReference summonerRef)
    {
        minionRef.TryGet(out NetworkObject minion);
        summonerRef.TryGet(out NetworkObject summoner);
        summoner.gameObject.GetComponent<PlayerStats>().myMinions.Remove(minion);
        minion.gameObject.GetComponent<MinionPetAI>().isInFight = false;
        minion.GetComponent<HasLifetime>().startingTime = minion.GetComponent<HasLifetime>().maxLifetime;
    }
}
