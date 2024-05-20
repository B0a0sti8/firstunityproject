using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonSpiders : SkillPrefab
{
    [SerializeField] private GameObject mySpiderPrefab;

    SummonerClass mySummonerClass;
    float myMinionDamageBase;
    float spiderCount;
    float spiderDurationBase;

    public override void Start()
    {
        base.Start();
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
        myClass = "Summoner";

        hasGlobalCooldown = true;
        needsTargetEnemy = true;
        targetsEnemiesOnly = true;
        ownCooldownTimeBase = 60f;
        castTimeOriginal = 2f;
        myAreaType = AreaType.CircleAroundTarget;
        skillRadius = 4f;
        skillRange = 10f;

        myMinionDamageBase = 5f;
        spiderDurationBase = 10;
    }

    public override void Update()
    {
        tooltipSkillDescription = "Summons Spiders in an area around target enemy, slowing and attacking foes.";
        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        NetworkObjectReference playerReference = (NetworkObjectReference)PLAYER;
        float myMinionDamage = myMinionDamageBase * (1+ mySummonerClass.increasedMinionDamage) * playerStats.dmgInc.GetValue();
        float spiderDuration = spiderDurationBase * playerStats.skillDurInc.GetValue();
        float spiderSlowEffect = mySummonerClass.spiderSlowEffect * playerStats.debuffInc.GetValue();
        spiderCount = mySummonerClass.spiderCount;

        int targetCount = currentTargets.Count;
        for (int i = 0; i < (int)spiderCount; i++)
        {
            int myRand = Random.Range(0, targetCount);
            Vector3 targetPositionPrelim = currentTargets[myRand].transform.position;

            SpawnStoneGolemServerRpc(playerReference, myMinionDamage, targetPositionPrelim, spiderDuration, spiderSlowEffect, i);
            mySummonerClass.SummonerClass_OnMinionSummoned();
        }
    }

    public override void StartCasting()
    {
        PlaySkillAnimation("Summoner", "Summoner_summon3");
        base.StartCasting();
    }

    [ServerRpc]
    private void SpawnStoneGolemServerRpc(NetworkObjectReference summoningPlayer, float minionDamage, Vector3 myTargetPosition, float myMinionDuration, float mySpiderSlowEffect, int randomSeed)
    {
        //Debug.Log("Summon Stone Golem Server RPC!");
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        float x = Random.Range(2f, 3f);
        float y = Random.Range(2f, 3f);
        float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
        float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1

        if (sumPla != null)
        {
            Vector2 posi = (Vector2)myTargetPosition + new Vector2(x * signx, y * signy);
            GameObject spiderling = Instantiate(mySpiderPrefab, posi, Quaternion.identity);
            spiderling.GetComponent<NetworkObject>().Spawn();
            spiderling.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
            spiderling.GetComponent<MeleeEnemyAttackTest>().baseAttackDamage = minionDamage;
            spiderling.GetComponent<HasLifetime>().maxLifetime = myMinionDuration;
            spiderling.transform.Find("Skills").GetComponent<SpiderSlowEffect>().slowValue = mySpiderSlowEffect;
            sumPla.GetComponent<PlayerStats>().myMainMinions.Add(spiderling);

            NetworkObjectReference spiderlingRef = (NetworkObjectReference)spiderling;
            SpawnStoneGolemClientRpc(summoningPlayer, spiderlingRef, randomSeed);
        }
    }

    [ClientRpc]
    private void SpawnStoneGolemClientRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference spiderlingRef, int randomSeed)
    {
        summoningPlayer.TryGet(out NetworkObject sour);
        spiderlingRef.TryGet(out NetworkObject spiderling);

        spiderling.GetComponent<MinionPetAI>().GetRandomTargetNearby(randomSeed);
        spiderling.GetComponent<MinionPetAI>().isInFight = true;

        GameObject sumPla = sour.gameObject;
        spiderling.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
    }
}