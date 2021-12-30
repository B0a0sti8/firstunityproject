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

    public int space = 24;      // Platz im Inventar (Listengröße?)

    public List<Buff> buffs = new List<Buff>();     // Liste in die Items eingetragen werden

    public bool AddBuff(Buff buff)     // Funktion zum Hinzufügen eines Items
    {
        if (buffs.Count >= space)   // Falls Inventar voll ist wird Rückmeldung gegeben
        {
            Debug.Log("To many effects!");
            return false;
        }

        buffs.Add(buff);            // Ansonsten Item aufnehmen
        buff.BuffEffect(gameObject.GetComponent<PlayerController>());

        StartCoroutine(Wait(buff.effectDuration));
        IEnumerator Wait(float time)
        {
            Debug.Log("Starte Buff Timer");
            yield return new WaitForSeconds(time);
            Debug.Log("Buff Timer Ende");
            buff.RemoveBuff(gameObject.GetComponent<PlayerController>());
            Debug.Log("Buff weg");
        }

        if (onBuffsChangedCallback != null)
        {
            onBuffsChangedCallback.Invoke();  // Triggert immer wenn Item hinzugefügt oder entfernt wird. Nice für Update des UI.
        }
        return true;
    }

    public void Remove(Buff buff)  // Funktion zum Entfernen eines Items
    {
        buffs.Remove(buff);
        if (onBuffsChangedCallback != null)
        {
            onBuffsChangedCallback.Invoke();     // Triggert immer wenn Item hinzugefügt oder entfernt wird. Nice für Update des UI.
        }
    }
}