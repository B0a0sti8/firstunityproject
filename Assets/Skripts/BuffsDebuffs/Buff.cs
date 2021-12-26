using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Buff", menuName = "BuffsAndDebuffs/Buff")]
public class Buff : ScriptableObject
{
    new public string name = "New Buff";      // Bisherige Definiton des Namens wird ?berschrieben
    public Sprite icon = null;                // Effekt Sprite 
    public bool isDebuff = false;             // Zus?tzlicher m?glicher Unterscheidungsparameter zwischen Buff und Debuff.
    public float effectDuration;              // Dauer des Effekts
    public string tooltipText;                // Für später Text der im Tooltipp angezeigt wird

    public virtual void BuffEffect(PlayerController playerController)               // Wird ?berschrieben.
    {
        // Beeinflusst Spieler oder Gegner.
        Debug.Log("Erhalte Buff/Debuff: " + name);
        playerController._Speed += 5f;
    }

    public void RemoveBuff(PlayerController playerController)
    {
        // Inventory.instance.Remove(this);  Buff muss aus Kollektion entfernt werden können.
        // Entferne Buff Effekt
        Debug.Log("Verliere Buff/Debuff: " + name);
        playerController._Speed -= 5f;
        BuffManager.instance.Remove(this);
    }
}