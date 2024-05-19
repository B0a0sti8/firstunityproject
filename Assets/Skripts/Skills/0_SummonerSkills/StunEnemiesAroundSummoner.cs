using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class StunEnemiesAroundSummoner : SkillPrefab
{
    public float buffDuration;

    public Sprite buffImage;
    StunnedEffectOnEnemies buff = new StunnedEffectOnEnemies();
    SummonerClass mySummonerClass;

    public override void Start()
    {
        base.Start();

        hasGlobalCooldown = true;
        ownCooldownTimeBase = 10f;

        isCastOnSelf = true;

        castTimeOriginal = 0f;

        myAreaType = AreaType.CircleAroundTarget;
        isCastOnSelf = true;
        skillRadius = 10f;

        needsTargetAlly = false;
        needsTargetEnemy = false;
        targetsEnemiesOnly = true;

        tooltipSkillDescription = "Stuns all enemies in a circle around you";

        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        buffDuration = mySummonerClass.summonerAoEStunDuration;

        buffDuration *= (playerStats.buffInc.GetValue() - 1) / 2 + 1;
        buffDuration *= (playerStats.skillDurInc.GetValue() - 1) / 2 + 1;

        foreach (GameObject enemy in currentTargets)
        {
            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(enemy.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "StunnedEffectOnEnemies", "StunnedEffectOnEnemies", false, buffDuration, 0, 0);
        }
    }
}
