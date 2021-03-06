using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    #region Singleton

    public static BuffManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnBuffsChanged();
    public OnBuffsChanged onBuffsChangedCallback;

    public List<Buff> buffs = new List<Buff>();
    public List<Buff> newBuffs = new List<Buff>();
    public List<Buff> expiredBuffs = new List<Buff>();

    public void AddBuff(Buff buff, Sprite buffImage, float duration, float value)
    {
        newBuffs.Add(buff);
        buff.icon = buffImage;
        buff.duration = duration;
        buff.value = value;
        buff.StartBuffEffect(gameObject.GetComponent<PlayerStats>());
    }

    public void AddBuff(Buff buff, Sprite buffImage, float duration, float tickTime, float tickValue)
    {
        newBuffs.Add(buff);
        buff.icon = buffImage;
        buff.duration = duration + 0.01f;
        buff.tickTime = tickTime;
        buff.tickValue = tickValue;
        buff.StartBuffEffect(gameObject.GetComponent<PlayerStats>());
    }

    public void RemoveBuff(Buff buff)
    {
        expiredBuffs.Add(buff);
    }

    public void HandleBuffs()
    {
        foreach (Buff buff in buffs)
        {
            buff.Update(gameObject.GetComponent<PlayerStats>());
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