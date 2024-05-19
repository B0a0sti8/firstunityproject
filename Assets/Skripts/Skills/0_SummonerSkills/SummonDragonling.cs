using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonDragonling : SkillPrefab
{
    [SerializeField] private GameObject dragonling;

    SummonerClass mySummonerClass;
    float myMinionDamageBase;

    public override void Start()
    {
        base.Start();
        myClass = "Summoner";

        hasGlobalCooldown = true;

        isCastOnSelf = true;

        hasOwnCooldown = true;
        ownCooldownTimeBase = 3f;

        castTimeOriginal = 1f;

        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
        myMinionDamageBase = 40;
    }

    public override void Update()
    {
        tooltipSkillDescription = "Summons a Dragonling that helps you destroy things.";

        base.Update();
    }

    public override void ConditionCheck()
    {
        //if (transform.GetComponent<SummonerClass>().myMainSummonerMinions.Count >= transform.GetComponent<SummonerClass>().maxNrOfMainSummonerMinions)
        //{ Debug.Log("Zu viele Summoner Begleiter!"); return; }

        if (playerStats.myMainMinions.Count >= playerStats.maxNrOfMainMinions)
        { Debug.Log("Zu viele Begleiter!"); return; }
        base.ConditionCheck();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        NetworkObjectReference playerReference = (NetworkObjectReference)PLAYER;
        Debug.Log("Summoning Dragonling");
        SpawnDragonlingServerRpc(playerReference, myMinionDamageBase);
        mySummonerClass.SummonerClass_OnMinionSummoned();

    }

    public override void StartCasting()
    {
        PlaySkillAnimation("Summoner", "Summoner_summon3");
        base.StartCasting();
    }


    [ServerRpc]
    private void SpawnDragonlingServerRpc(NetworkObjectReference summoningPlayer, float minionDamage)
    {
        Debug.Log("Summon Dragonling Server RPC!");
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        float x = Random.Range(2, 3);
        float y = Random.Range(2, 3);
        float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
        float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1

        if (sumPla != null)
        {
            Vector2 posi = (Vector2)sumPla.transform.position + new Vector2(x * signx, y * signy);
            GameObject dragonl = Instantiate(dragonling, posi, Quaternion.identity);
            dragonl.GetComponent<NetworkObject>().Spawn();
            dragonl.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
            dragonl.GetComponent<MeleeEnemyAttackTest>().baseAttackDamage = minionDamage;

            sumPla.GetComponent<PlayerStats>().myMainMinions.Add(dragonl);
            //transform.GetComponent<SummonerClass>().myMainSummonerMinions.Add(dragonl);

            NetworkObjectReference dragonlingRef = (NetworkObjectReference)dragonl;
            SpawnDragonlingClientRpc(summoningPlayer, dragonlingRef);
            // SpawnDragonlingClientRpc(summoningPlayer, clientRpcParams);
        }
    }

    [ClientRpc]
    private void SpawnDragonlingClientRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference dragonling, ClientRpcParams clientRpcParams = default)
    {
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        dragonling.TryGet(out NetworkObject drag);
        GameObject drago = drag.gameObject;

        drago.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
        //sumPla.GetComponent<PlayerStats>().myMainMinions.Add(drago);
    }

}
