using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MasterSchmuff
{
    public Buff thisBuff;

    public override void BuffEffect(PlayerStats playerStats, float duration)
    {
        //base.BuffEffect(playerController);
        Debug.Log("Speed Up");
        playerStats.gameObject.GetComponent<PlayerController>()._Speed += 5;
        //playerController._Speed += 5f;
    }

    public override void RemoveBuff(PlayerStats playerStats)
    {
        // Entferne Buff Effekt
        Debug.Log("Speed Down");
        playerStats.gameObject.GetComponent<PlayerController>()._Speed -= 5;
        //playerController._Speed -= 5f;
        BuffManager.instance.Remove(thisBuff);
    }
}