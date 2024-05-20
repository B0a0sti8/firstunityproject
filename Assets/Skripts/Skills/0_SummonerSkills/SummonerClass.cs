using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SummonerClass : NetworkBehaviour
{
    Transform PLAYER;
    PlayerStats playerStats;

    public float ExplodingImpsDamageModifier;

    public float minionSummoned_CastSpeedModifier;

    public float darkBoltDamageModifier;
    public bool doesDarkboltIncreaseMinionLifetime;
    public float darkBoltLifeTimeIncrease;

    public float increasedMinionDuration;
    public float increasedMinionDamage;

    public float summonerAoEStunDuration;

    public float summonerSummonSpiritWolfOnSkillChance;
    public float summonerSummonSpiritWolfOnSkillDurationInc;
    //public float summonerSummonSpiritWolfOnSkillCooldown;
    public float summonerSummonSpiritWolfOnSkillWolfDamageInc;
    public float summonerSummonSpiritWolfOnSkillWolfDurationInc;

    public int increasedInsectSummon;

    public bool astralSnakeHasDoT;
    public float astralSnakeDotMod;
    public int astralSnakeAdditionalBounces;
    public bool astralSnakeHasDebuff;

    public bool hasCastSpeedOnMinionSummonedTalent;
    public bool hasAdditionalMainMinion1;
    public bool hasAdditionalMainMinion2;

    public bool buffMainMinionDmgAndHealIsConsuming;
    public float buffMainMinionDmgAndHealIsConsumingCooldownDec;
    public float buffMainMinionDmgAndHealIsConsumingDurationInc;

    public float fireDemonExplosionCooldown;
    public float fireDemonDamageModifier;

    public float spiderCount;
    public float spiderSlowEffect;

    public int assembleTheMinionsCount;


    private void Awake()
    {
        PLAYER = transform.parent.parent;
        playerStats = PLAYER.GetComponent<PlayerStats>();

        increasedMinionDuration = 0;
        increasedMinionDamage = 0;

        summonerAoEStunDuration = 0;

        summonerSummonSpiritWolfOnSkillChance = 0;
        summonerSummonSpiritWolfOnSkillDurationInc = 0;
        summonerSummonSpiritWolfOnSkillWolfDamageInc = 0;
        summonerSummonSpiritWolfOnSkillWolfDurationInc = 0;

        buffMainMinionDmgAndHealIsConsuming = true;
        buffMainMinionDmgAndHealIsConsumingDurationInc = 0;
        buffMainMinionDmgAndHealIsConsumingCooldownDec = 0;

        astralSnakeAdditionalBounces = 0;
        astralSnakeHasDoT = false;
        astralSnakeDotMod = 0f;
        astralSnakeHasDebuff = true;

        ExplodingImpsDamageModifier = 1;

        darkBoltDamageModifier = 1;
        doesDarkboltIncreaseMinionLifetime = false;
        darkBoltLifeTimeIncrease = 0;

        hasCastSpeedOnMinionSummonedTalent = false;
        hasAdditionalMainMinion1 = false;
        hasAdditionalMainMinion2 = false;

        fireDemonExplosionCooldown = 10f;
        fireDemonDamageModifier = 0f;

        spiderCount = 5;
        spiderSlowEffect = -0.1f;

        assembleTheMinionsCount = 2;
}

    public void SummonerClass_OnMinionSummoned()
    {
        //Debug.Log("Has Summoned Minion");
        if (hasCastSpeedOnMinionSummonedTalent)
        {
            PLAYER.GetComponent<PlayerStats>().actionSpeed.AddModifierAdd(minionSummoned_CastSpeedModifier);
            StartCoroutine(CastSpeedForminionSummonedStop(10, minionSummoned_CastSpeedModifier));
        }
    }

    public void SummonerClass_IncreaseLivingMinionDuration(float additionalLifetime)
    {
        foreach (GameObject minion in playerStats.myMinions)
        {
            SummonerClass_IncreaseLivingMinionDurationServerRpc(minion.GetComponent<NetworkObject>(), additionalLifetime);
        }
    }

    [ServerRpc]
    public void SummonerClass_IncreaseLivingMinionDurationServerRpc(NetworkObjectReference minionref, float additionalLifetime)
    {
        SummonerClass_IncreaseLivingMinionDurationClientRpc(minionref, additionalLifetime);
    }

    [ClientRpc]
    public void SummonerClass_IncreaseLivingMinionDurationClientRpc(NetworkObjectReference minionref, float additionalLifetime)
    {
        minionref.TryGet(out NetworkObject minion);
        HasLifetime myLifetime = minion.GetComponent<HasLifetime>();
        if (myLifetime != null)
        {
            myLifetime.maxLifetime += additionalLifetime;
        }
    }

    public IEnumerator CastSpeedForminionSummonedStop(float time, float mod)
    {
        //Debug.Log("Has Summoned Minion. Removing Buff");

        yield return new WaitForSeconds(time);
        PLAYER.GetComponent<PlayerStats>().actionSpeed.RemoveModifierAdd(mod);
    }
}
