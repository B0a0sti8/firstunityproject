using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonStoneGolem : SkillPrefab
{
    [SerializeField] private GameObject stoneGolem;

    SummonerClass mySummonerClass;
    float myMinionDamageBase;

    public override void Start()
    {
        base.Start();
        myClass = "Summoner";

        hasGlobalCooldown = true;

        isCastOnSelf = true;

        ownCooldownTimeBase = 3f;

        castTimeOriginal = 1f;

        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
        myMinionDamageBase = 30f;
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
        //Debug.Log("Summoning stone Golem");
        SpawnStoneGolemServerRpc(playerReference, myMinionDamageBase);
        mySummonerClass.SummonerClass_OnMinionSummoned();

    }

    public override void StartCasting()
    {
        PlaySkillAnimation("Summoner", "Summoner_summon3");
        base.StartCasting();
    }


    [ServerRpc]
    private void SpawnStoneGolemServerRpc(NetworkObjectReference summoningPlayer, float minionDamage)
    {
        //Debug.Log("Summon Stone Golem Server RPC!");
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        float x = Random.Range(2, 3);
        float y = Random.Range(2, 3);
        float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
        float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1

        if (sumPla != null)
        {
            Vector2 posi = (Vector2)sumPla.transform.position + new Vector2(x * signx, y * signy);
            GameObject stonGo = Instantiate(stoneGolem, posi, Quaternion.identity);
            stonGo.GetComponent<NetworkObject>().Spawn();
            stonGo.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
            stonGo.GetComponent<MeleeEnemyAttackTest>().baseAttackDamage = minionDamage;

            sumPla.GetComponent<PlayerStats>().myMainMinions.Add(stonGo);

            NetworkObjectReference stoneGoRef = (NetworkObjectReference)stonGo;
            SpawnStoneGolemClientRpc(summoningPlayer, stoneGoRef);
        }
    }

    [ClientRpc]
    private void SpawnStoneGolemClientRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference stoneGoRef)
    {
        summoningPlayer.TryGet(out NetworkObject sour);
        stoneGoRef.TryGet(out NetworkObject stoGo);

        GameObject sumPla = sour.gameObject;
        stoGo.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
    }
}