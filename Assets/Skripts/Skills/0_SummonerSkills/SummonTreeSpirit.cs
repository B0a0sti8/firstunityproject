using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonTreeSpirit : SkillPrefab
{
    [SerializeField] private GameObject treeSpirit;

    SummonerClass mySummonerClass;

    public override void Start()
    {
        base.Start();
        myClass = "Summoner";

        hasGlobalCooldown = true;

        isSelfCast = true;

        hasOwnCooldown = true;
        ownCooldownTimeBase = 3f;

        castTimeOriginal = 1f;
        isSkillChanneling = false;

        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void Update()
    {
        tooltipSkillDescription = "Summons a Stone Golem that takes damage for you.";

        base.Update();
    }

    public override void ConditionCheck()
    {
        if (playerStats.myMainMinions.Count >= playerStats.maxNrOfMainMinions)
        { Debug.Log("Zu viele Begleiter!"); return; }
        base.ConditionCheck();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        NetworkObjectReference playerReference = (NetworkObjectReference)PLAYER;
        //Debug.Log("Summoning tree Spirit");
        SpawnTreeSpiritServerRpc(playerReference);
        mySummonerClass.SummonerClass_OnMinionSummoned();

    }

    public override void StartCasting()
    {
        PlaySkillAnimation("Summoner", "Summoner_summon3");
        base.StartCasting();
    }


    [ServerRpc]
    private void SpawnTreeSpiritServerRpc(NetworkObjectReference summoningPlayer, ServerRpcParams serverRpcParams = default)
    {
        //Debug.Log("Summon Tree Spirit Server RPC!");
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        float x = Random.Range(2, 3);
        float y = Random.Range(2, 3);
        float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
        float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1

        if (sumPla != null)
        {
            Vector2 posi = (Vector2)sumPla.transform.position + new Vector2(x * signx, y * signy);
            GameObject treeSpir = Instantiate(treeSpirit, posi, Quaternion.identity);
            treeSpir.GetComponent<NetworkObject>().Spawn();
            treeSpir.GetComponent<MinionPetAI>().myMaster = sumPla.transform;

            sumPla.GetComponent<PlayerStats>().myMainMinions.Add(treeSpir);

            NetworkObjectReference treeSpirRef = (NetworkObjectReference)treeSpir;
            SpawnStoneGolemClientRpc(summoningPlayer, treeSpirRef);
        }
    }

    [ClientRpc]
    private void SpawnStoneGolemClientRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference treeSpirRef, ClientRpcParams clientRpcParams = default)
    {
        summoningPlayer.TryGet(out NetworkObject sour);
        treeSpirRef.TryGet(out NetworkObject treeSpir);

        GameObject sumPla = sour.gameObject;
        treeSpir.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
    }
}