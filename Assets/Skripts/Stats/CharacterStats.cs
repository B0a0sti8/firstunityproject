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

    public void TakeDamage(float damage, int aggro, bool isCrit, NetworkBehaviourReference nBref)
    {
        if (IsOwner)
        {
            TakeDamageServerRpc(damage, aggro, isCrit, nBref);
        }
    }

    [ServerRpc]
    public virtual void TakeDamageServerRpc(float damage, int aggro, bool isCrit, NetworkBehaviourReference source)
    {
        TakeDamageClientRpc(damage, aggro, isCrit, source);
    }

    [ClientRpc]
    public virtual void TakeDamageClientRpc(float damage, int aggro, bool isCrit, NetworkBehaviourReference source)
    {
        if (IsOwner)
        {
            currentHealth.Value -= damage;

            DamagePopup.Create(gameObject.transform.position, (int)damage, false, isCrit);
        }

        //GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += damage;
        //FindObjectOfType<AudioManager>().Play("Oof");

        // AggroManagement später ... 
        //source.TryGet<NetworkBehaviour>(out NetworkBehaviour sourc);
        //gameObject.GetComponent<EnemyAI>().aggroTable[sourc.gameObject] += aggro;
    }

    public virtual void TakeHealing(float healing, bool isCrit, NetworkBehaviourReference nBref)
    {
        if (IsOwner)
        {
            currentHealth.Value += healing;
            GetHealingServerRpc(healing, isCrit, nBref);
        }
        
    }

    [ServerRpc]
    public virtual void GetHealingServerRpc(float healing, bool isCrit, NetworkBehaviourReference source)
    {
        Debug.Log("ServerRpc");
        GetHealingClientRpc(healing, isCrit, source);
    }

    [ClientRpc]
    public virtual void GetHealingClientRpc(float healing, bool isCrit, NetworkBehaviourReference source)
    {
        Debug.Log("ClientRpc");
        currentHealth.Value += healing;

        DamagePopup.Create(gameObject.transform.position, (int)healing, true, isCrit);


        // AggroManagement später ... 
        //source.TryGet<NetworkBehaviour>(out NetworkBehaviour sourc);
        //gameObject.GetComponent<EnemyAI>().aggroTable[sourc.gameObject] += aggro;
    }

    public virtual void Start()
    {
        if (!IsOwner) { return; }
        healthBarWorldCanv = gameObject.transform.Find("Canvas World Space").transform.Find("HealthBar").GetComponent<HealthBar>();
        currentHealth.OnValueChanged += (float previousValue, float newValue) => { OnHealthChange(); };

        OnHealthChange();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public void OnHealthChange()
    {
        if (!IsOwner) { return; }
        int cH = (int)this.currentHealth.Value;
        int mH = (int)this.maxHealth.GetValue();
        NetworkBehaviourReference nBref = this;
        HealthChangedServerRpc(cH, mH, nBref);

    }

    [ServerRpc]
    public void HealthChangedServerRpc(int cuHe, int maHe, NetworkBehaviourReference nBrf)
    {
        HealthChangedClientRpc(cuHe, maHe, nBrf);
    }

    [ClientRpc]
    public void HealthChangedClientRpc(int cuHe, int maHe, NetworkBehaviourReference nBrf)
    {
        nBrf.TryGet<CharacterStats>(out CharacterStats cStat);
        HealthBar heBa = cStat.transform.Find("Canvas World Space").Find("HealthBar").GetComponent<HealthBar>();
        heBa.SetMaxHealth(maHe);
        heBa.SetHealth(cuHe);
    }


    public virtual void Update()
    {

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
