using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Buff
{
    public BuffManager buffManager;
    public BuffManagerNPC buffManagerNPC;
    public Sprite icon = null;
    float elapsed;

    public GameObject buffSource=null;

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

    public virtual void StartBuffEffect(CharacterStats playerStats) 
    {
        durationTimeLeft = duration;
    }

    public virtual void EndBuffEffect(CharacterStats playerStats) 
    {
        if (playerStats.gameObject.GetComponent<BuffManager>() != null)
        {
            buffManager = playerStats.gameObject.GetComponent<BuffManager>();
            buffManager.RemoveBuff(this);
        }
        else
        {
            buffManagerNPC = playerStats.gameObject.GetComponent<BuffManagerNPC>();
            buffManagerNPC.RemoveBuff(this);
        }
    }

    public virtual void Dispell()
    {
        if (isRemovable)
        {
            elapsed = duration;
        }
    }

    public virtual void Update(CharacterStats playerStats)
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