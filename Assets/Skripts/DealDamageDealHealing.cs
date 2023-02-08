using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageOrHealing
{
    private static EnemyStats enemyStats;
    private static PlayerStats playerStats;

    private static float tempDamage;
    private static int trueDamage;

    private static bool isCrit=false; 
    private static int aggro;

    private static float tempHealing;
    private static int trueHealing;


    public static float DealDamage(GameObject source, GameObject target, float baseDamge, bool isMagical=false, bool isUnavoidable=false)
    {
        // Stats des Angreifers
        if(source.GetComponent<EnemyStats>() != null)                    // Source = Enemy
        {
            enemyStats = source.GetComponent<EnemyStats>();

            tempDamage = baseDamge * (1 + enemyStats.dmgModifier.GetValue());

            int critRandom = Random.Range(1, 100);          
            if (critRandom <= enemyStats.critChance.GetValue())         // Crit?
            { 
                tempDamage *= enemyStats.critMultiplier.GetValue();
                isCrit = true;
            }
        }
        else if (source.GetComponent<PlayerStats>() != null)            // Source = Player
        {
            playerStats = source.GetComponent<PlayerStats>();
            tempDamage = baseDamge * (1 + playerStats.dmgInc.GetValue());

            int critRandom = Random.Range(1, 100);                      // Crit?
            if (critRandom <= playerStats.critChance.GetValue())
            { 
                tempDamage *= playerStats.critMultiplier.GetValue();
                isCrit = true;
            }

            aggro = (int)Mathf.Round(tempDamage);
            if (playerStats.MyIsTank == true)
            { aggro *= 3; }
        }

        // Stats des Ziels
        if (target.GetComponent<EnemyStats>() != null)                   // Target = Enemy
        {
            enemyStats = target.GetComponent<EnemyStats>();
            tempDamage *= (100 - enemyStats.armor.GetValue()) / 100;

            if (!isUnavoidable)
            {
                int evadeRandom = Random.Range(1, 100);                      // Crit?
                if (evadeRandom <= enemyStats.evadeChance.GetValue())
                { tempDamage = 0f; }
            }

            trueDamage = (int)Mathf.Round(tempDamage);
            enemyStats.TakeDamageRPC(trueDamage, aggro, isCrit, source);                              // IM MULTIPLAYER: INFO MUSS AN ALLE GESENDET WERDEN!
        }
        else if (target.GetComponent<PlayerStats>() != null)            // Target = Player
        {
            playerStats = target.GetComponent<PlayerStats>();
            tempDamage *= (100 - playerStats.armor.GetValue()) / 100;

            if (isMagical)
            { tempDamage *= (100 - playerStats.magRed.GetValue()) / 100; }
            else
            { tempDamage *= (100 - playerStats.physRed.GetValue()) / 100; }

            if (!isUnavoidable)
            {
                int evadeRandom = Random.Range(1, 100);                      // Crit?
                if (evadeRandom <= playerStats.evadeChance.GetValue())
                { tempDamage = 0f; }

                int blockRandom = Random.Range(1, 100);                      // Crit?
                if (blockRandom <= playerStats.blockChance.GetValue())
                { tempDamage = 0f; }
            }

            trueDamage = (int)Mathf.Round(tempDamage);
            playerStats.TakeDamageRPC(trueDamage, 0, isCrit, source);                              // IM MULTIPLAYER: INFO MUSS AN ALLE GESENDET WERDEN!
        }

        enemyStats = null;
        playerStats = null;
        isCrit = false;
        return trueDamage;
    }

    public static float DoHealing(GameObject source, GameObject target, float baseHealing)
    {
        // Stats des Heilers
        if (source.GetComponent<EnemyStats>() != null)                    // Source = Enemy
        {
            enemyStats = source.GetComponent<EnemyStats>();

            tempHealing = baseHealing;

            int critRandom = Random.Range(1, 100);
            if (critRandom <= enemyStats.critChance.GetValue())         // Crit?
            { 
                tempHealing *= enemyStats.critMultiplier.GetValue();
                isCrit = true;
            }
        }
        else if (source.GetComponent<PlayerStats>() != null)            // Source = Player
        {
            playerStats = source.GetComponent<PlayerStats>();
            tempHealing = baseHealing * (1 + playerStats.healInc.GetValue());

            int critRandom = Random.Range(1, 100);                      // Crit?
            if (critRandom <= playerStats.critChance.GetValue())
            { 
                tempHealing *= playerStats.critMultiplier.GetValue();
                isCrit = true;
            }
        }
        else
        {
            tempHealing = baseHealing;
        }

        // Stats des Ziels
        if (target.GetComponent<EnemyStats>() != null)                   // Target = Enemy
        {
            enemyStats = source.GetComponent<EnemyStats>();

            trueHealing = (int)Mathf.Round(tempHealing);
            enemyStats.TakeHealingRPC(trueHealing, isCrit, source);                              // IM MULTIPLAYER: INFO MUSS AN ALLE GESENDET WERDEN!
        }
        else if (target.GetComponent<PlayerStats>() != null)            // Target = Player
        {
            playerStats = target.GetComponent<PlayerStats>();
            tempHealing *= (1 + playerStats.incHealInc.GetValue());

            trueHealing = (int)Mathf.Round(tempHealing);
            playerStats.TakeHealingRPC(trueHealing, isCrit, source);                              // IM MULTIPLAYER: INFO MUSS AN ALLE GESENDET WERDEN!
        }

        enemyStats = null;
        playerStats = null;
        isCrit = false;
        return trueHealing;
    }
}