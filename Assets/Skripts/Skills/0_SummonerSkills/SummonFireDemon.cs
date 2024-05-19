using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class SummonFireDemon : SkillPrefab
{
    float impactDamage;
    public GameObject myFireImpactAnim;
    public GameObject myFireDemonPrefab;
    float myMinionDamageBase;
    SummonerClass mySummonerClass;
    float fireDemonDuration;
    float demonExplosionCooldown;

    public override void Start()
    {
        impactDamage = 1200;
        myAreaType = AreaType.CirclePlacable;
        hasGlobalCooldown = true;
        skillRadius = 6f;
        targetsEnemiesOnly = true;
        castTimeOriginal = 1.5f;
        myMinionDamageBase = 20f;
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
        fireDemonDuration = 15f;
    }

    public override void SkillEffect()
    {
        demonExplosionCooldown = mySummonerClass.fireDemonExplosionCooldown;
        float minionDamageMod = myMinionDamageBase + playerStats.dmgInc.GetValue() * (1 + mySummonerClass.fireDemonDamageModifier);

        base.SkillEffect();
        foreach (GameObject target in currentTargets) Debug.Log("Target: " + target);
        DealDamage(impactDamage);
        Vector3 myPosition = circleAim;
        myPosition.z = 0.2f;
        FireImpactServerRpc(myPosition);

        NetworkObjectReference playerReference = (NetworkObjectReference)PLAYER;
        SpawnFireDemonServerRpc(playerReference, minionDamageMod, myPosition, fireDemonDuration * playerStats.skillDurInc.GetValue(), demonExplosionCooldown);
        mySummonerClass.SummonerClass_OnMinionSummoned();
    }

    [ServerRpc]
    private void SpawnFireDemonServerRpc(NetworkObjectReference summoningPlayer, float minionDamage, Vector3 spawnPosition, float minionDuration, float explosionCooldown)
    {
        //Debug.Log("Summon Stone Golem Server RPC!");
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        if (sumPla != null)
        {
            GameObject fireDe = Instantiate(myFireDemonPrefab, spawnPosition, Quaternion.identity);
            fireDe.GetComponent<NetworkObject>().Spawn();
            fireDe.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
            fireDe.GetComponent<MeleeEnemyAttackTest>().baseAttackDamage = minionDamage;
            fireDe.transform.Find("Skills").GetComponent<FireDemonAoESkill>().baseDamage = minionDamage*10;
            fireDe.GetComponent<MinionPetAI>().isInFight = true;
            fireDe.GetComponent<HasLifetime>().maxLifetime = minionDuration;
            fireDe.transform.Find("Skills").GetComponent<FireDemonAoESkill>().cooldown = explosionCooldown;
            //fireDe.GetComponent<MinionPetAI>().GetRandomTargetNearby();

            sumPla.GetComponent<PlayerStats>().myMinions.Add(fireDe);

            NetworkObjectReference fireDeRef = (NetworkObjectReference)fireDe;
            SpawnFireDemonClientRpc(summoningPlayer, fireDeRef, explosionCooldown);
        }
    }

    [ClientRpc]
    private void SpawnFireDemonClientRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference fireDeRef, float explosionCooldown)
    {
        summoningPlayer.TryGet(out NetworkObject sour);
        fireDeRef.TryGet(out NetworkObject fireDe);

        GameObject sumPla = sour.gameObject;
        fireDe.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
        fireDe.transform.Find("Skills").GetComponent<FireDemonAoESkill>().cooldown = explosionCooldown;
        fireDe.GetComponent<MinionPetAI>().isInFight = true;
    }

    [ServerRpc]
    public void FireImpactServerRpc(Vector3 myPosition)
    {
        FireImpactClientRpc(myPosition);
    }

    [ClientRpc]
    public void FireImpactClientRpc(Vector3 myPosition) 
    {
        GameObject myFireImpact = Instantiate(myFireImpactAnim, myPosition, Quaternion.identity);
        myFireImpact.transform.localScale *= 3;
        foreach (ParticleSystem ps in myFireImpact.transform.GetComponentsInChildren<ParticleSystem>())
        {
            var myMain = ps.main;
            myMain.simulationSpeed = 0.5f;
        }
    }
}