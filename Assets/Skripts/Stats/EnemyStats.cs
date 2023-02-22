using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class EnemyStats : CharacterStats
{
    //Dictionary<>
    // public bool isBoss;
    // public float hitboxRadius;
    // public bool isMelee;
    // public float attackRange;
    // public bool isFlying;

    // public float enemyHealth;
    // public float enemyDamage;
    // public float enemyArmor;
    // public float enemyEvade;
    // public float movementSpeed;
    // public float modAttackSpeed;
    public float baseAttackSpeed = 2f;
    [SerializeField] public float baseDamage = 10f;
    public Stat dmgModifier; // 0 - Inf

    public int XPForPlayer;

    public int groupNumber;

    [HideInInspector]
    public bool enemyUIHealthActive = false;



    void Start()
    {
        currentHealth.Value = maxHealth.GetValue();
        isAlive.Value = true;
        baseDamage = 10f;
        baseAttackSpeed = 2f;
    }

    public override void Update()
    {
        base.Update();

        if (currentHealth.Value <= 0 && isAlive.Value == true)
        {
            Die();
        }




        if (enemyUIHealthActive)
        {
            gameObject.transform.Find("Canvas UI").transform.Find("HealthBar").GetComponent<HealthBar>().SetMaxHealth((int)maxHealth.GetValue());
            gameObject.transform.Find("Canvas UI").transform.Find("HealthBar").GetComponent<HealthBar>().SetHealth((int)currentHealth.Value);
        }

        if (currentHealth.Value > maxHealth.GetValue())
        {
            currentHealth.Value = maxHealth.GetValue();
        }

        if (currentHealth.Value < 0)
        {
            currentHealth.Value = 0;
        }
    }

    [ServerRpc]
    public override void TakeDamageServerRpc(float damage, int aggro, bool isCrit)
    {
        Debug.Log("Enemy takes damage " + damage);
        base.TakeDamageServerRpc(damage, aggro, isCrit);
        FindObjectOfType<AudioManager>().Play("Oof");
    }

    [ServerRpc]
    public override void GetHealingServerRpc(float healing, bool isCrit)
    {
        Debug.Log("Enemy gets healing " + healing);
        base.GetHealingServerRpc(healing, isCrit);
        //FindObjectOfType<AudioManager>().Play("Oof");
    }

    public void TakeDamage(float damage, int aggro, bool isCrit, GameObject source)
    {
        Debug.Log("Deal Damage");
        if (IsOwner)
        {
            Debug.Log("Deal Damage 1");
            TakeDamageServerRpc(damage, aggro, isCrit);
        }
        gameObject.GetComponent<EnemyAI>().aggroTable[source] += aggro;
    }

    public void TakeHealing(float healing, bool isCrit, GameObject source)
    {
        if (IsOwner)
        {
            GetHealingServerRpc(healing, isCrit);
        }
    }

    public override void Die()
    {
        gameObject.transform.Find("Charakter").GetComponent<SpriteRenderer>().flipY = true;
        GameObject[] players = GetComponent<EnemyAI>().aggroTable.Keys.ToArray();
        if (players != null)
        {
            foreach (GameObject p in players)
            {
                p.GetComponent<StuffManagerScript>().OnKillConfirmed(this);
                p.GetComponent<PlayerStats>().GainXP(XPForPlayer);
            }
        }
        Destroy(gameObject, 1f);
        base.Die();


    }
}
