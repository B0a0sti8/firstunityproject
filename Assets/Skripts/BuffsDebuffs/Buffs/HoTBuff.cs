using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoTBuff : Buff
{
    new public bool isOverTime = true;

    public override void StartBuffEffect(PlayerStats playerStats)
    {
        base.StartBuffEffect(playerStats);
        tickTimeElapsed = 0;
        Debug.Log("Hallo");
    }

    public override void EndBuffEffect(PlayerStats playerStats)
    {
        base.EndBuffEffect(playerStats);
    }

    public override Buff Clone()
    {
        HoTBuff clone = (HoTBuff)this.MemberwiseClone();
        return clone;
    }

    public override void Update(PlayerStats playerStats)
    {
        base.Update(playerStats);
        tickTimeElapsed += Time.deltaTime;
        Debug.Log(tickTimeElapsed);
        if (tickTimeElapsed >= tickTime)
        {
            tickTimeElapsed = 0;
            playerStats.currentHealth += tickValue;
            Debug.Log(tickValue);
        }
    }
}
