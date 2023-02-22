using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterStats : NetworkBehaviour
{
    public NetworkVariable<bool> isAlive = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //public bool isAlive = true;
    public bool isCurrentlyCasting = false;

    [SerializeField]
    private string type;
    public string MyType { get => type; set => type = value; }

    #region Stats
    // Stats
    [Header("Health")]
    public Stat maxHealth; // 0 - Inf
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Main Stats")] public Stat armor; // 0 - 100 bzw. -Inf - 100 /// 30 -> Erlittener Schaden um 30% verringert

    [Header("Side Stats")]
    public Stat movementSpeed; // 0 - Inf bzw. 1 - Inf
    public Stat actionSpeed; // 0 - 90 bzw. -Inf - 100 /// 30 -> (Global)Cooldown ist 30% kürzer (10s -> 7s)
    public Stat critChance; // 0(%) - 100(%) /// 30 -> 30% Wahrscheinlichkeit auf Crit
    public Stat critMultiplier; // 100(%) - Inf(%) /// 130 -> Angriff macht 130% Schaden
    public Stat evadeChance;


    #endregion

    HealthBar healthBarWorldCanv;

    [ServerRpc]
    public virtual void TakeDamageServerRpc(float damage, int aggro, bool isCrit)
    {
        currentHealth.Value -= damage;

        if (IsOwner)
        {
            DamagePopup.Create(gameObject.transform.position, (int)damage, false, isCrit);
            //GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += damage;
            FindObjectOfType<AudioManager>().Play("Oof");
        }
    }

    [ServerRpc]
    public virtual void GetHealingServerRpc(float healing, bool isCrit)
    {
        currentHealth.Value += healing;

        if (IsOwner)
        {
            DamagePopup.Create(gameObject.transform.position, (int)healing, true, isCrit);
        }
    }

    public virtual void Start()
    {
        healthBarWorldCanv = gameObject.transform.Find("Canvas World Space").transform.Find("HealthBar").GetComponent<HealthBar>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public virtual void Update()
    {
        // updates Bars on Canvas
        UpdateWorldCanvasHealthBarClientRpc();
    }

    [ClientRpc]
    void UpdateWorldCanvasHealthBarClientRpc()
    {
        healthBarWorldCanv.SetMaxHealth((int)maxHealth.GetValue());
        healthBarWorldCanv.SetHealth((int)currentHealth.Value);
    }

    public virtual void Die()
    {
        if (!IsOwner) { return; }

        FindObjectOfType<AudioManager>().Play("OoOof");
        //Debug.Log("He dead");
        isAlive.Value = false;
        // To be overwritten in Child Class
    }
}
