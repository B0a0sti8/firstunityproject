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
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<float> maxHealthServer = new NetworkVariable<float>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

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
        //Debug.Log("TakeDamage");
        NetworkObjectReference targetPosition = gameObject;
        TakeDamageServerRpc(damage, aggro, isCrit, nBref, targetPosition);
        OnHealthChange();
    }

    [ServerRpc(RequireOwnership = false)]
    public virtual void TakeDamageServerRpc(float damage, int aggro, bool isCrit, NetworkBehaviourReference source, NetworkObjectReference targetPosition)
    {
        //Debug.Log("TakeDamageServerRpc");
        currentHealth.Value -= damage;
        TakeDamageClientRpc(damage, aggro, isCrit, source, targetPosition);
    }

    [ClientRpc]
    public virtual void TakeDamageClientRpc(float damage, int aggro, bool isCrit, NetworkBehaviourReference source, NetworkObjectReference targetPosition)
    {
        Debug.Log("TakeDamageClientRpc");
        targetPosition.TryGet(out NetworkObject g);
        DamagePopup.Create(g.transform.position, (int)damage, false, isCrit);

        //GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += damage;
        //FindObjectOfType<AudioManager>().Play("Oof");

        // AggroManagement später ... 
        //source.TryGet<NetworkBehaviour>(out NetworkBehaviour sourc);
        //gameObject.GetComponent<EnemyAI>().aggroTable[sourc.gameObject] += aggro;
    }

    public virtual void TakeHealing(float healing, bool isCrit, NetworkBehaviourReference nBref)
    {
        NetworkObjectReference targetPosition = gameObject;
        GetHealingServerRpc(healing, isCrit, nBref, targetPosition);
        OnHealthChange();
    }

    [ServerRpc(RequireOwnership = false)]
    public virtual void GetHealingServerRpc(float healing, bool isCrit, NetworkBehaviourReference source, NetworkObjectReference targetPosition)
    {
        Debug.Log("TakeHealingServerRpc");
        currentHealth.Value += healing;
        GetHealingClientRpc(healing, isCrit, source, targetPosition);
    }

    [ClientRpc]
    public virtual void GetHealingClientRpc(float healing, bool isCrit, NetworkBehaviourReference source, NetworkObjectReference targetPosition)
    {
        Debug.Log("TakeHealingClientRpc");
        targetPosition.TryGet(out NetworkObject g);
        DamagePopup.Create(g.transform.position, (int)healing, true, isCrit);


        // AggroManagement später ... 
        //source.TryGet<NetworkBehaviour>(out NetworkBehaviour sourc);
        //gameObject.GetComponent<EnemyAI>().aggroTable[sourc.gameObject] += aggro;
    }

    public virtual void Start()
    {
        if (!IsOwner) { return; }

        healthBarWorldCanv = transform.Find("Canvas World Space").transform.Find("HealthBar").GetComponent<HealthBar>();
        currentHealth.OnValueChanged += (float previousValue, float newValue) => { OnHealthChange(); };
        maxHealthServer.OnValueChanged += (float previousValue, float newValue) => { OnHealthChange(); };

        OnHealthChange();

        SetMultiplayerMaxHealthServerRpc(maxHealth.GetValue());
        SetCurrentHealthServerRpc(maxHealth.GetValue());
    }

    [ServerRpc]
    public void SetCurrentHealthServerRpc(float healthValue)
    {
        currentHealth.Value = healthValue;
    }

    [ServerRpc]
    public void SetMultiplayerMaxHealthServerRpc(float healthValue)
    {
         maxHealthServer.Value = healthValue;
    }

    public void OnHealthChange()
    {
        //Debug.Log("HealthChange1");
        if (!IsOwner) { return; }

        //Debug.Log("HealthChange3");
        int cH = (int)this.currentHealth.Value;
        int mH = (int)this.maxHealthServer.Value;
        NetworkBehaviourReference nBref = this;
        //Debug.Log(cH);
        //Debug.Log(mH);
        HealthChangedServerRpc(cH, mH, nBref);

    }

    [ServerRpc(RequireOwnership = false)]
    public void HealthChangedServerRpc(int cuHe, int maHe, NetworkBehaviourReference nBrf)
    {
        HealthChangedClientRpc(cuHe, maHe, nBrf);
    }

    [ClientRpc]
    public void HealthChangedClientRpc(int cuHe, int maHe, NetworkBehaviourReference nBrf)
    {
        //Debug.Log("HealthChange!");
        nBrf.TryGet<CharacterStats>(out CharacterStats cStat);
        HealthBar heBa = cStat.transform.Find("Canvas World Space").Find("HealthBar").GetComponent<HealthBar>();
        heBa.SetMaxHealth(maHe);
        heBa.SetHealth(cuHe);
    }

    public virtual void Update()
    {
        if (!IsOwner)
        { return; }

        if (maxHealthServer.Value != maxHealth.GetValue())
        {
            Debug.Log("Updating Max Health");
            Debug.Log(maxHealthServer.Value);
            Debug.Log(maxHealth.GetValue());
            SetMultiplayerMaxHealthServerRpc(maxHealth.GetValue());
        }
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
