using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BuffManager : NetworkBehaviour
{
    public delegate void OnBuffsChanged();
    public OnBuffsChanged onBuffsChangedCallback;
    BuffDebuffUIWorldCanv buffDebuffUIWorldCanv;

    [SerializeField] public List<Buff> buffs = new List<Buff>();
    List<Buff> newBuffs = new List<Buff>();
    List<Buff> expiredBuffs = new List<Buff>();


    public void AddBuffProcedure(NetworkBehaviourReference target, NetworkBehaviourReference source, string buffName, string buffImageName, bool isDamageOrHealing, float duration, float tickTime, float value)
    {
         AddBuffServerRpc(target, source, buffName, buffImageName, isDamageOrHealing, duration, tickTime, value);
    }

    [ServerRpc(RequireOwnership = false)]
    void AddBuffServerRpc(NetworkBehaviourReference target, NetworkBehaviourReference source, string buffName, string buffImageName, bool isDamageOrHealing, float duration, float tickTime, float value)
    {
        AddBuffClientRpc(target, source, buffName, buffImageName, isDamageOrHealing, duration, tickTime, value);
                
        target.TryGet<NetworkBehaviour>(out NetworkBehaviour tar);
        source.TryGet<NetworkBehaviour>(out NetworkBehaviour sor);

        var buff = BuffMasterManager.MyInstance.ListOfAllBuffs[buffName];
        Buff clone = buff.Clone();
        clone.buffSource = sor.gameObject;
        Sprite buffIcon = Resources.Load<Sprite>("BuffDebuffSprites/" + buffImageName);

        if (isDamageOrHealing)
        { tar.gameObject.GetComponent<BuffManager>().AddBuff(clone, buffIcon, duration, tickTime, value); }
        else
        { tar.gameObject.GetComponent<BuffManager>().AddBuff(clone, buffIcon, duration, value); }
        
    }

    [ClientRpc]
    void AddBuffClientRpc(NetworkBehaviourReference target, NetworkBehaviourReference source, string buffName, string buffImageName, bool isDamageOrHealing, float duration, float tickTime, float value)
    {
        if (IsHost) { return; }
     
        target.TryGet<NetworkBehaviour>(out NetworkBehaviour tar);
        source.TryGet<NetworkBehaviour>(out NetworkBehaviour sor);

        var buff = BuffMasterManager.MyInstance.ListOfAllBuffs[buffName];
        Buff clone = buff.Clone();
        clone.buffSource = sor.gameObject;
        Sprite buffIcon = Resources.Load<Sprite>("BuffDebuffSprites/" + buffImageName);

        if (isDamageOrHealing)
        { tar.gameObject.GetComponent<BuffManager>().AddBuff(clone, buffIcon, duration, tickTime, value); }
        else
        { tar.gameObject.GetComponent<BuffManager>().AddBuff(clone, buffIcon, duration, value); }
    }

   


    public void AddBuff(Buff buff, Sprite buffImage, float duration, float value)
    {
        newBuffs.Add(buff);
        buff.icon = buffImage;
        buff.duration = duration;
        buff.value = value;
        buff.StartBuffUI();

        if (IsServer) { buff.StartBuffEffect(gameObject.GetComponent<PlayerStats>()); }
        
    }

    public void AddBuff(Buff buff, Sprite buffImage, float duration, float tickTime, float tickValue)
    {
        Debug.Log("BuffManager: Added Buff");
        newBuffs.Add(buff);
        buff.icon = buffImage;
        buff.duration = duration + 0.01f;
        buff.tickTime = tickTime;
        buff.tickValue = tickValue;
        buff.StartBuffUI();

        if (IsServer) { buff.StartBuffEffect(gameObject.GetComponent<PlayerStats>()); }
    }

    public void RemoveBuff(Buff buff)
    {
        expiredBuffs.Add(buff);
    }

    public void DispellBuffs()
    {
        foreach (Buff buff in buffs)
        {
            buff.Dispell();
        }
    }

    void HandleBuffs()
    {
        foreach (Buff buff in buffs)
        {
            buff.Update(gameObject.GetComponent<PlayerStats>());
            if (IsServer) { buff.UpdateEffect(gameObject.GetComponent<PlayerStats>()); }
        }

        if (newBuffs.Count > 0)
        {
            buffs.AddRange(newBuffs);
            newBuffs.Clear();
            buffDebuffUIWorldCanv.UpdateUIStart();
            if (onBuffsChangedCallback != null) { onBuffsChangedCallback.Invoke(); }
        }

        if (expiredBuffs.Count > 0)
        {
            foreach (Buff buff in expiredBuffs)
            {
                buffs.Remove(buff);
            }
            expiredBuffs.Clear();
            buffDebuffUIWorldCanv.UpdateUIStart();
            if (onBuffsChangedCallback != null) { onBuffsChangedCallback.Invoke(); }
        }
    }

    [ClientRpc]
    void DebugLogClientRpc()
    {
        Debug.Log("Habe Buff");
    }

    void Update()
    {
        HandleBuffs();
    }

    private void Start()
    {
        buffDebuffUIWorldCanv = transform.Find("Canvas World Space").Find("CanvasBuffsAndDebuffsPlayerWorldCanv").GetComponent<BuffDebuffUIWorldCanv>();
    }
}