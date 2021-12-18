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

    [PunRPC]
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
