using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    //public bool isBoss;
    //public float hitboxRadius;
    //public bool isMelee;
    //public float attackRange;
    //public bool isFlying;

    //public float enemyHealth;
    //public float enemyDamage;
    //public float enemyArmor;
    //public float enemyEvade;
    //public float movementSpeed;
    public float modAttackSpeed;
    public float baseAttackSpeed = 2f;

    void Start()
    {
        currentHealth = maxHealth.GetValue();
        isAlive = true;
    }

    private void Update()
    {
        // updates Bars on Canvas
        gameObject.transform.Find("Canvas World Space").transform.GetChild(0).GetComponent<HealthBar>().SetMaxHealth((int)maxHealth.GetValue());
        gameObject.transform.Find("Canvas World Space").transform.GetChild(0).GetComponent<HealthBar>().SetHealth((int)currentHealth);

        if (currentHealth > maxHealth.GetValue())
        {
            currentHealth = maxHealth.GetValue();
        }
        
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    public override void TakeDamage(float damage)
    {
        Debug.Log("Enemy takes damage " + damage);
        base.TakeDamage(damage);
        FindObjectOfType<AudioManager>().Play("Oof");
    }

    public override void Die()
    {
        gameObject.transform.Find("Charakter").GetComponent<SpriteRenderer>().flipY = true;
        Destroy(gameObject, 1f);
        base.Die();
    }
}
