using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MeleeEnemyAttackTest : EnemyAttack
{
    private void Start()
    {
        baseAttackDamage = 50;
    }
    public override void EnemyAtkEffect(GameObject target)
    {
        base.EnemyAtkEffect(target);

        //Debug.Log(gameObject.GetComponent<EnemyStats>().actionSpeed.GetValue());

        DamageOrHealing.DealDamage(gameObject.GetComponent<NetworkBehaviour>(), target.GetComponent<NetworkBehaviour>(), 50, false, false); 
        // Attack
        // Animation
    }
}
