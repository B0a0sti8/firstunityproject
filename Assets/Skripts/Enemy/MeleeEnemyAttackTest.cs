using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MeleeEnemyAttackTest : EnemyAttack
{
    public override void EnemyAtkEffect(GameObject target)
    {
        base.EnemyAtkEffect(target);

        DamageOrHealing.DealDamage(gameObject.GetComponent<NetworkBehaviour>(), target.GetComponent<NetworkBehaviour>(), gameObject.GetComponent<EnemyStats>().dmgModifier.GetValue(), false, false); ;
        // Attack
        // Animation
    }
}
