using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TheWayOfTheChickenDamageDebuff : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "Chicken";
        buffDescription = "Chicken";
        base.StartBuffEffect(playerStats);
        isRemovable = false;
        tickTimeElapsed = 0;
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
    }

    public override Buff Clone()
    {
        TheWayOfTheChickenDamageDebuff clone = (TheWayOfTheChickenDamageDebuff)this.MemberwiseClone();
        return clone;
    }

    public override void Update(CharacterStats playerStats)
    {
        base.Update(playerStats);
        tickTimeElapsed += Time.deltaTime;
        if (tickTimeElapsed >= tickTime)
        {
            tickTimeElapsed = 0;

            DamageOrHealing.DealDamage(buffSource.GetComponent<NetworkBehaviour>(), playerStats.gameObject.GetComponent<NetworkBehaviour>(), tickValue, false, false);
            //playerStats.view.RPC("TakeDamage", RpcTarget.All, tickValue, 101, critRandom, critChance, critMultiplier);
        }
    }
}
