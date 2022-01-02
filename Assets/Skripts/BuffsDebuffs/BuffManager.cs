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

    public void AddBuff(Buff buff, float duration)     // Funktion zum Hinzufügen eines Items
    {
        newBuffs.Add(buff);
        buff.effectDuration = duration;
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

















    //public bool AddBuff(Buff buff, float duration)     // Funktion zum Hinzufügen eines Items
    //{
    //    if (buffs.Count >= space)   // Falls Inventar voll ist wird Rückmeldung gegeben
    //    {
    //        Debug.Log("To many effects!");
    //        return false;
    //    }

    //    StartCoroutine(Wait(duration));
    //    IEnumerator Wait(float time)
    //    {
    //        Debug.Log("Starte Buff Timer");
    //        buffs.Add(buff);            // Ansonsten Item aufnehmen
    //        buff.effectDuration = duration;
    //        buff.BuffEffect(gameObject.GetComponent<PlayerStats>());

    //        yield return new WaitForSeconds(time);
    //        Debug.Log("Buff Timer Ende");
    //        buff.RemoveBuff(gameObject.GetComponent<PlayerStats>());
    //        Debug.Log("Buff weg");
    //    }

    //    if (onBuffsChangedCallback != null)
    //    {
    //        onBuffsChangedCallback.Invoke();  // Triggert immer wenn Item hinzugefügt oder entfernt wird. Nice für Update des UI.
    //    }
    //    return true;
    //}

    //public void Remove(Buff buff)  // Funktion zum Entfernen eines Items
    //{
    //    buffs.Remove(buff);
    //    if (onBuffsChangedCallback != null)
    //    {
    //        onBuffsChangedCallback.Invoke();     // Triggert immer wenn Item hinzugefügt oder entfernt wird. Nice für Update des UI.
    //    }
    //}
}