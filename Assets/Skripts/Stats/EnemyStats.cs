using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public bool isBoss;
    public float hitboxRadius;
    public bool isMelee;
    public float attackRange;
    public bool isFlying;

    public float enemyHealth;
    public float enemyDamage;
    public float enemyArmor;
    public float enemyEvade;
    public float movementSpeed;
    public float attackSpeed;


    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();
    }
}
