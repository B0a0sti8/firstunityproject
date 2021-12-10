using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttackTest : EnemyAttack
{
    public override void EnemyAtkEffect(GameObject target)
    {
        base.EnemyAtkEffect(target);
        Debug.Log("OH YEEAH! :3 " + target);
        //target.GetComponent<Player>().TakeDamage(20);
        target.GetComponent<PlayerStats>().TakeDamage(gameObject.GetComponent<EnemyStats>().damage.GetValue());
        // Attack
        // Animation
    }
}
