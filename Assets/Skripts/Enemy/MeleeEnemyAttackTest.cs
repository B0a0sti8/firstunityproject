using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttackTest : EnemyAttack
{
    public override void EnemyAtkEffect(GameObject target)
    {
        base.EnemyAtkEffect(target);

        int missRandom = Random.Range(1, 100);
        int critRandom = Random.Range(1, 100);
        float critChance =  gameObject.GetComponent<EnemyStats>().critChance.GetValue();
        float critMultiplier = gameObject.GetComponent<EnemyStats>().critMultiplier.GetValue();
        target.GetComponent<PlayerStats>().TakeDamage(gameObject.GetComponent<EnemyStats>().mastery.GetValue(), missRandom, critRandom, critChance, critMultiplier);
        // Attack
        // Animation
    }
}
