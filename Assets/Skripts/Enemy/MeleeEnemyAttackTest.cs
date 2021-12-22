using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttackTest : EnemyAttack
{
    public override void EnemyAtkEffect(GameObject target)
    {
        base.EnemyAtkEffect(target);

        int missRandom = Random.Range(0, 100);
        target.GetComponent<PlayerStats>().TakeDamage(gameObject.GetComponent<EnemyStats>().damage.GetValue(), missRandom);
        // Attack
        // Animation
    }
}
