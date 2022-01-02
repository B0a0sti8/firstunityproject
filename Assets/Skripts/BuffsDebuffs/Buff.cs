using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Buff
{
    public string name = "New Buff";          // Bisherige Definiton des Namens wird überschrieben
    public Sprite icon = null;                // Effekt Sprite 
    public float effectDuration;              // Dauer des Effekts
    private float elapsed;
    public BuffManager buffManager;
    public bool isRemovable = true;

    public virtual void StartBuffEffect(PlayerStats playerStats) 
    {

    }

    public virtual void EndBuffEffect(PlayerStats playerStats) 
    {
        buffManager = playerStats.gameObject.GetComponent<BuffManager>();
        buffManager.RemoveBuff(this);
    }

    public void Dispell()
    {
        if (isRemovable)
        {
            elapsed = effectDuration;
        }
    }

    public virtual void Update(PlayerStats playerStats)
    {
        elapsed += Time.deltaTime;
        if (elapsed >= effectDuration) {
            EndBuffEffect(playerStats); 
        }
    }

    public abstract Buff Clone();
}

//public string tooltipText;                // Für später Text der im Tooltipp angezeigt wird
//public bool isDebuff = false;             // Zusätzlicher möglicher Unterscheidungsparameter zwischen Buff und Debuff.
// public bool isInterrupted = false;