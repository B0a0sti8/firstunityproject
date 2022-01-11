using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Buff
{
    public BuffManager buffManager;
    public Sprite icon = null;
    float elapsed;

    public string buffName;
    public string buffDescription;
    public float duration;
    public float durationTimeLeft;
    public bool isRemovable = true;
    //public bool isOverTime = false;

    public float value;

    public float tickTime;
    public float tickTimeElapsed;
    public float tickValue;

    public virtual void StartBuffEffect(PlayerStats playerStats) 
    {
        durationTimeLeft = duration;
    }

    public virtual void EndBuffEffect(PlayerStats playerStats) 
    {
        buffManager = playerStats.gameObject.GetComponent<BuffManager>();
        buffManager.RemoveBuff(this);
    }

    public virtual void Dispell()
    {
        if (isRemovable)
        {
            elapsed = duration;
        }
    }

    public virtual void Update(PlayerStats playerStats)
    {
        elapsed += Time.deltaTime;
        if (elapsed >= duration) {
            EndBuffEffect(playerStats); 
        }

        if (durationTimeLeft > 0)
        {
            durationTimeLeft -= Time.deltaTime;
        }
    }

    public abstract Buff Clone();
}