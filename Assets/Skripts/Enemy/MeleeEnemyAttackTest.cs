using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttackTest : EnemyAttack
{
    public override void EnemyAtkEffect(GameObject target)
    {
        base.EnemyAtkEffect(target);
        target.GetComponent<PlayerStats>().TakeDamage(gameObject.GetComponent<EnemyStats>().damage.GetValue());
        // Attack
        // Animation
    }
}
