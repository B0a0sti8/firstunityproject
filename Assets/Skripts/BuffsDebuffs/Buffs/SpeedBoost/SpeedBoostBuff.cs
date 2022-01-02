using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostBuff : MasterSchmuff
{
    public Buff thisBuff;

    public override void BuffEffect(PlayerStats playerStats, float duration)
    {
        //base.BuffEffect(playerController);
        Debug.Log("Speed Up");
        playerStats.movementSpeed.AddModifierAdd(5); 
    }

    public override void RemoveBuff(PlayerStats playerStats)
    {
        // Entferne Buff Effekt
        Debug.Log("Speed Down");
        playerStats.movementSpeed.AddModifierAdd(-5);
        BuffManager.instance.Remove(thisBuff);
    }
}