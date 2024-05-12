using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BuffManagerNPC : NetworkBehaviour
{
    public delegate void OnBuffsChanged();
    public OnBuffsChanged onBuffsChangedCallback;

    [SerializeField] public List<Buff> buffs = new List<Buff>();
    [SerializeField] public List<Buff> newBuffs = new List<Buff>();
    [SerializeField] public List<Buff> expiredBuffs = new List<Buff>();

    public void AddBuffProcedure(NetworkObjectReference target, NetworkObjectReference source, string buffName, string buffImageName, bool hasTicks, float duration, float tickTime, float value, float additionalValue1 = 0, float additionalValue2 = 0, float additionalValue3 = 0)
    {
        //Debug.Log("Procedure");
        AddBuffServerRpc(target, source, buffName, buffImageName, hasTicks, duration, tickTime, value, additionalValue1, additionalValue2, additionalValue3);
    }

    [ServerRpc(RequireOwnership = false)]
    void AddBuffServerRpc(NetworkObjectReference target, NetworkObjectReference source, string buffName, string buffImageName, bool hasTicks, float duration, float tickTime, float value, float additionalValue1 = 0, float additionalValue2 = 0, float additionalValue3 = 0)
    {
        //Debug.Log("Server Rpc");
        AddBuffClientRpc(target, source, buffName, buffImageName, hasTicks, duration, tickTime, value, additionalValue1, additionalValue2, additionalValue3);
    }

    [ClientRpc]
    void AddBuffClientRpc(NetworkObjectReference target, NetworkObjectReference source, string buffName, string buffImageName, bool hasTicks, float duration, float tickTime, float value, float additionalValue1 = 0, float additionalValue2 = 0, float additionalValue3 = 0)
    {
        //Debug.Log("Client Rpc");
        target.TryGet(out NetworkObject tar);
        source.TryGet(out NetworkObject sor);

        var buff = BuffMasterManager.MyInstance.ListOfAllBuffs[buffName];
        Buff clone = buff.Clone();
        clone.buffSource = sor.gameObject;
        //Sprite buffIcon = Resources.Load<Sprite>("BuffDebuffSprites/" + buffImageName);
        Sprite buffIcon = BuffMasterManager.MyInstance.ListOfAllBuffSprites[buffImageName];
        buffIcon.name = buffImageName;

        if (hasTicks)
        { tar.gameObject.GetComponent<BuffManagerNPC>().AddBuff(clone, buffIcon, duration, tickTime, value, additionalValue1, additionalValue2, additionalValue3); }
        else
        { tar.gameObject.GetComponent<BuffManagerNPC>().AddBuff(clone, buffIcon, duration, value, additionalValue1, additionalValue2, additionalValue3); }
    }

    public void AddBuff(Buff buff, Sprite buffImage, float duration, float value, float additionalValue1 = 0, float additionalValue2 = 0, float additionalValue3 = 0)
    {
        //Debug.Log("Adding Buff 1");
        newBuffs.Add(buff);
        buff.icon = buffImage;
        buff.duration = duration;
        buff.value = value;
        buff.additionalValue1 = additionalValue1;
        buff.additionalValue2 = additionalValue2;
        buff.additionalValue3 = additionalValue3;
        buff.StartBuffUI();

        if (IsServer) { buff.StartBuffEffect(gameObject.GetComponent<CharacterStats>()); }
    }

    public void AddBuff(Buff buff, Sprite buffImage, float duration, float tickTime, float tickValue, float additionalValue1 = 0, float additionalValue2 = 0, float additionalValue3 = 0)
    {
        //Debug.Log("Adding Buff 2");
        newBuffs.Add(buff);
        buff.icon = buffImage;
        buff.duration = duration + 0.01f;
        buff.tickTime = tickTime;
        buff.tickValue = tickValue;
        buff.additionalValue1 = additionalValue1;
        buff.additionalValue2 = additionalValue2;
        buff.additionalValue3 = additionalValue3;

        buff.StartBuffUI();

        if (IsServer) { buff.StartBuffEffect(gameObject.GetComponent<CharacterStats>()); }
    }


    public void RemoveBuff(Buff buff)
    {
        expiredBuffs.Add(buff);
    }

    public void HandleBuffs()
    {
        //Debug.Log(buffs.Count);
        //Debug.Log(newBuffs.Count);
        foreach (Buff buff in buffs)
        {
            buff.Update(gameObject.GetComponent<CharacterStats>());
            //if (IsServer) { buff.Update(gameObject.GetComponent<CharacterStats>()); }
        }

        if (newBuffs.Count > 0)
        {
            buffs.AddRange(newBuffs);
            newBuffs.Clear();
            if (onBuffsChangedCallback != null) { onBuffsChangedCallback.Invoke(); }
        }

        if (expiredBuffs.Count > 0)
        {
            foreach (Buff buff in expiredBuffs)
            {
                buffs.Remove(buff);
            }
            expiredBuffs.Clear();
            if (onBuffsChangedCallback != null) { onBuffsChangedCallback.Invoke(); }
        }
    }

    public void DispellBuffs()
    {
        foreach (Buff buff in buffs)
        {
            buff.Dispell();
        }
    }

    void Update()
    {
        HandleBuffs();
    }
}
