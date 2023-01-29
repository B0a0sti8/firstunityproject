using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterStats : MonoBehaviourPunCallbacks
{
    public PhotonView view;
    public bool isAlive;
    public bool isCurrentlyCasting = false;

    [SerializeField]
    private string type;
    public string MyType { get => type; set => type = value; }

    #region Stats
    // Stats
    [Header("Health")]
    public Stat maxHealth; // 0 - Inf
    public float currentHealth;

    [Header("Main Stats")] public Stat armor; // 0 - 100 bzw. -Inf - 100 /// 30 -> Erlittener Schaden um 30% verringert

    [Header("Side Stats")]
    public Stat movementSpeed; // 0 - Inf bzw. 1 - Inf
    public Stat actionSpeed; // 0 - 90 bzw. -Inf - 100 /// 30 -> (Global)Cooldown ist 30% kürzer (10s -> 7s)
    public Stat critChance; // 0(%) - 100(%) /// 30 -> 30% Wahrscheinlichkeit auf Crit
    public Stat critMultiplier; // 100(%) - Inf(%) /// 130 -> Angriff macht 130% Schaden
    public Stat evadeChance;

  
    #endregion

    public virtual void TakeDamage(float damage, int aggro, bool isCrit)
    {
        currentHealth -= damage;

        if (view.IsMine)
        {
            DamagePopup.Create(gameObject.transform.position, (int)damage, false, isCrit);
            //GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += damage;
            FindObjectOfType<AudioManager>().Play("Oof");
        }
    }

    public virtual void GetHealing(float healing, bool isCrit)
    {
        currentHealth += healing;

        if (view.IsMine)
        {
            DamagePopup.Create(gameObject.transform.position, (int)healing, true, isCrit);
            //GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += damage;
            //FindObjectOfType<AudioManager>().Play("Oof");
        }
    }

    public virtual void Update()
    {
        if (currentHealth <= 0 && isAlive == true)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        FindObjectOfType<AudioManager>().Play("OoOof");
        //Debug.Log("He dead");
        isAlive = false;
        // To be overwritten in Child Class
    }
}
