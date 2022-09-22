using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

            int critRandom = Random.Range(1, 100);
            float critChance = playerStats.critChance.GetValue();
            float critMultiplier = playerStats.critMultiplier.GetValue();
            playerStats.view.RPC("TakeDamage", RpcTarget.All, tickValue, 101, critRandom, critChance, critMultiplier);

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
