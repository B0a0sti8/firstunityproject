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

    public void AddBuffProcedure(NetworkObjectReference target, NetworkObjectReference source, string buffName, string buffImageName, bool hasTicks, float duration, float tickTime, float value, float additionalValue1 = 0, float additionalValue2 = 0, float additionalValue3 = 0)
    {
         AddBuffServerRpc(target, source, buffName, buffImageName, hasTicks, duration, tickTime, value, additionalValue1, additionalValue2, additionalValue3);
    }

    [ServerRpc(RequireOwnership = false)]
    void AddBuffServerRpc(NetworkObjectReference target, NetworkObjectReference source, string buffName, string buffImageName, bool hasTicks, float duration, float tickTime, float value, float additionalValue1 = 0, float additionalValue2 = 0, float additionalValue3 = 0)
    {
        AddBuffClientRpc(target, source, buffName, buffImageName, hasTicks, duration, tickTime, value, additionalValue1, additionalValue2, additionalValue3);     
    }

    [ClientRpc]
    void AddBuffClientRpc(NetworkObjectReference target, NetworkObjectReference source, string buffName, string buffImageName, bool hasTicks, float duration, float tickTime, float value, float additionalValue1 = 0, float additionalValue2 = 0, float additionalValue3 = 0)
    {
        target.TryGet(out NetworkObject tar);
        source.TryGet(out NetworkObject sor);

        var buff = BuffMasterManager.MyInstance.ListOfAllBuffs[buffName];
        Buff clone = buff.Clone();
        clone.buffSource = sor.gameObject;
        //Sprite buffIcon = Resources.Load<Sprite>("BuffDebuffSprites/" + buffImageName);
        Sprite buffIcon = BuffMasterManager.MyInstance.ListOfAllBuffSprites[buffImageName];
        buffIcon.name = buffImageName;

        if (hasTicks)
        { tar.gameObject.GetComponent<BuffManager>().AddBuff(clone, buffIcon, duration, tickTime, value, additionalValue1, additionalValue2, additionalValue3); }
        else
        { tar.gameObject.GetComponent<BuffManager>().AddBuff(clone, buffIcon, duration, value, additionalValue1, additionalValue2, additionalValue3); }
    }

    // Gedacht für z.B. Stärkungen und Schwächungen.
    public void AddBuff(Buff buff, Sprite buffImage, float duration, float value, float additionalValue1 = 0, float additionalValue2 = 0, float additionalValue3 = 0)
    {
        newBuffs.Add(buff);
        buff.icon = buffImage;
        buff.duration = duration;
        buff.value = value;

        buff.additionalValue1 = additionalValue1;
        buff.additionalValue2 = additionalValue2;
        buff.additionalValue3 = additionalValue3;

        buff.StartBuffUI();

        if (IsServer) { buff.StartBuffEffect(gameObject.GetComponent<PlayerStats>()); }
    }

    // Gedacht für z.B. Schaden und Heilung über Zeit.
    public void AddBuff(Buff buff, Sprite buffImage, float duration, float tickTime, float tickValue, float additionalValue1 = 0, float additionalValue2 = 0, float additionalValue3 = 0)
    {
        Debug.Log("BuffManager: Added Buff");
        newBuffs.Add(buff);
        buff.icon = buffImage;
        buff.duration = duration + 0.01f;
        buff.tickTime = tickTime;
        buff.tickValue = tickValue;

        buff.additionalValue1 = additionalValue1;
        buff.additionalValue2 = additionalValue2;
        buff.additionalValue3 = additionalValue3;

        buff.StartBuffUI();

        if (IsServer) { buff.StartBuffEffect(gameObject.GetComponent<PlayerStats>()); }
    }

    public void RemoveBuffProcedure(NetworkObjectReference sourceRef, string refBuffName, bool singlebuff = false)
    {
        RemoveBuffServerRpc(sourceRef, refBuffName, singlebuff);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveBuffServerRpc(NetworkObjectReference sourceRef, string refBuffName, bool singlebuff = false)
    {
        RemoveBuffClientRpc(sourceRef, refBuffName, singlebuff);
    }

    [ClientRpc]
    public void RemoveBuffClientRpc(NetworkObjectReference sourceRef, string refBuffName, bool singlebuff = false)
    {
        sourceRef.TryGet(out NetworkObject source);
        Debug.Log("Removing Buff 1");

        foreach (Buff myBu in buffs)
        {
            Debug.Log(myBu.buffSource + "   " + source);
            Debug.Log(myBu.buffName + "   " + refBuffName);
            if (myBu.buffSource == source.gameObject && myBu.buffName == refBuffName)
            {
                RemoveBuff(myBu);
                if (singlebuff) break;
            }
        }
    }

    public void RemoveBuff(Buff buff)
    {
        expiredBuffs.Add(buff);
        if(IsServer) buff.EndBuffEffect(gameObject.GetComponent<CharacterStats>());
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

    void Update()
    {
        HandleBuffs();
    }

    private void Start()
    {
        buffDebuffUIWorldCanv = transform.Find("Canvas World Space").Find("CanvasBuffsAndDebuffsPlayerWorldCanv").GetComponent<BuffDebuffUIWorldCanv>();
    }
}