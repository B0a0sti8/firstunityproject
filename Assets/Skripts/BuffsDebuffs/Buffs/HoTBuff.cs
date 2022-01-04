using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HoTBuff : Buff
{
    //new public bool isOverTime = true;

    public override void StartBuffEffect(PlayerStats playerStats)
    {
        base.StartBuffEffect(playerStats);
        tickTimeElapsed = 0;
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
        if (tickTimeElapsed >= tickTime)
        {
            tickTimeElapsed = 0;

            int critRandom = Random.Range(1, 100);
            float critChance = playerStats.critChance.GetValue();
            float critMultiplier = playerStats.critMultiplier.GetValue();
            playerStats.view.RPC("GetHealing", RpcTarget.All, tickValue, critRandom, critChance, critMultiplier);

            //playerStats.currentHealth += tickValue;
            //bool isCrit = false;
            //if (Random.Range(1, 5) <= 2)
            //{
            //    isCrit = true;
            //}
            //DamagePopup.Create(playerStats.gameObject.transform.position, (int)tickValue, true, isCrit);
        }
    }
}
