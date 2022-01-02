using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Buff", menuName = "BuffsAndDebuffs/Buff")]
public class Buff : ScriptableObject
{
    new public string name = "New Buff";      // Bisherige Definiton des Namens wird überschrieben
    public Sprite icon = null;                // Effekt Sprite 
    public bool isDebuff = false;             // Zusätzlicher möglicher Unterscheidungsparameter zwischen Buff und Debuff.
    //public float effectDuration;            // Dauer des Effekts
    public string tooltipText;                // Für später Text der im Tooltipp angezeigt wird
    public GameObject Hallo2;
    //[HideInInspector]
    public GameObject Hallo;
    public bool isInterrupted = false;

    private void Awake()
    {
        Hallo = Instantiate(Hallo2);
    }
}




// ______________________________________________ Looooool ______________________________________________________



public class MasterSchmuff : MonoBehaviour
{
    //public virtual void BuffEffect(PlayerController playerController) { }
    public virtual void BuffEffect(PlayerStats playerStats, float duration) { }

    //public virtual void RemoveBuff(PlayerController playerController) { }
    public virtual void RemoveBuff(PlayerStats playerStats) { }
}