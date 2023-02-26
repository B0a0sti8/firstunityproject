using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttackTest : EnemyAttack
{
    public override void EnemyAtkEffect(GameObject target)
    {
        base.EnemyAtkEffect(target);

        DamageOrHealing.DealDamage(gameObject, target, gameObject.GetComponent<EnemyStats>().dmgModifier.GetValue(), false, false);
        // Attack
        // Animation
    }
}
