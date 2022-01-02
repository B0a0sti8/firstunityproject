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

    public List<Buff> buffs = new List<Buff>();     // Liste in die Items eingetragen werden
    public List<Buff> newBuffs = new List<Buff>();
    public List<Buff> expiredBuffs = new List<Buff>();

    public void AddBuff(Buff buff, float duration, Sprite buffImage)     // Funktion zum Hinzufügen eines Items
    {
        newBuffs.Add(buff);
        buff.effectDuration = duration;
        buff.icon = buffImage;
        buff.StartBuffEffect(gameObject.GetComponent<PlayerStats>());
    }

    public void AddBuff(Buff buff, float duration, Sprite buffImage, float tickTime, float tickValue)
    {
        newBuffs.Add(buff);
        buff.effectDuration = duration + 0.01f;
        buff.icon = buffImage;
        buff.tickTime = tickTime;
        buff.tickValue = tickValue;
        buff.StartBuffEffect(gameObject.GetComponent<PlayerStats>());
    }

    public void RemoveBuff(Buff buff)     // Funktion zum Hinzufügen eines Items
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