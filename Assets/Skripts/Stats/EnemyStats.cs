using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyStats : CharacterStats, IPunObservable
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
        baseDamage = 10f;
        baseAttackSpeed = 2f;
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

    [PunRPC] public override void TakeDamage(float damage, int aggro, bool isCrit)
    {
        Debug.Log("Enemy takes damage " + damage);
        base.TakeDamage(damage, aggro, isCrit);
        FindObjectOfType<AudioManager>().Play("Oof");
    }

    [PunRPC] public override void GetHealing(float healing, bool isCrit)
    {
        Debug.Log("Enemy gets healing " + healing);
        base.GetHealing(healing, isCrit);
        //FindObjectOfType<AudioManager>().Play("Oof");
    }

    public void TakeDamageRPC(float damage, int aggro, bool isCrit, GameObject source)
    {
        Debug.Log("Deal Damage");
        if (view.IsMine)
        {
            Debug.Log("Deal Damage 1");
            view.RPC("TakeDamage", RpcTarget.All, damage, aggro, isCrit);
        }
        gameObject.GetComponent<EnemyAI>().aggroTable[source] += aggro;
    }

    public void TakeHealingRPC(float healing, bool isCrit, GameObject source)
    {
        if (view.IsMine)
        {
            view.RPC("GetHealing", RpcTarget.All, healing, isCrit);
        }
    }

    public override void Die()
    {
        gameObject.transform.Find("Charakter").GetComponent<SpriteRenderer>().flipY = true;
        GameObject[] players = GetComponent<EnemyAI>().potentialTargets;
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
