using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyStats : CharacterStats, IPunObservable
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
    public Stat mastery; // 0 - Inf

    public int groupNumber;

    [HideInInspector]
    public bool enemyUIHealthActive = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Reihenfolge der gesendeten und empfangenen Komponenten muss gleich sein
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else if (stream.IsReading)
        {
            currentHealth = (float)stream.ReceiveNext();
        }
    }

    void Start()
    {
        currentHealth = maxHealth.GetValue();
        isAlive = true;
    }

    public override void Update()
    {
        base.Update();

        // updates Bars on Canvas
        gameObject.transform.Find("Canvas World Space").transform.Find("HealthBar").GetComponent<HealthBar>().SetMaxHealth((int)maxHealth.GetValue());
        gameObject.transform.Find("Canvas World Space").transform.Find("HealthBar").GetComponent<HealthBar>().SetHealth((int)currentHealth);

        if (enemyUIHealthActive)
        {
            gameObject.transform.Find("Canvas UI").transform.Find("HealthBar").GetComponent<HealthBar>().SetMaxHealth((int)maxHealth.GetValue());
            gameObject.transform.Find("Canvas UI").transform.Find("HealthBar").GetComponent<HealthBar>().SetHealth((int)currentHealth);
        }

        if (currentHealth > maxHealth.GetValue())
        {
            currentHealth = maxHealth.GetValue();
        }
        
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    [PunRPC] public override void TakeDamage(float damage, int missRandomRange, int critRandomRange, float critChance, float critMultiplier)
    {
        Debug.Log("Enemy takes damage " + damage);
        base.TakeDamage(damage, missRandomRange, critRandomRange, critChance, critMultiplier);
        FindObjectOfType<AudioManager>().Play("Oof");
    }

    [PunRPC] public override void GetHealing(float healing, int critRandomRange, float critChance, float critMultiplier)
    {
        Debug.Log("Enemy gets healing " + healing);
        base.GetHealing(healing, critRandomRange, critChance, critMultiplier);
        FindObjectOfType<AudioManager>().Play("Oof");
    }

    public override void Die()
    {
        gameObject.transform.Find("Charakter").GetComponent<SpriteRenderer>().flipY = true;
        Destroy(gameObject, 1f);
        base.Die();
    }
}
