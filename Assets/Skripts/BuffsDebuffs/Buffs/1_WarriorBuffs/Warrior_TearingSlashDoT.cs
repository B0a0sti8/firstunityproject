using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Warrior_TearingSlashDoT : Buff
{
    public override void StartBuffUI()
    {
        base.StartBuffUI();
        buffName = "Warrior_TearingSlashDoT";
        buffDescription = "Deals " + tickValue.ToString() + " damage every " + tickTime.ToString() + " seconds.";
    }

    public override void StartBuffEffect(CharacterStats enemyStats)
    {
        base.StartBuffEffect(enemyStats);
        isRemovable = true;
    }

    public override void EndBuffEffect(CharacterStats enemyStats)
    {
        base.EndBuffEffect(enemyStats);
    }

    public override void UpdateEffect(CharacterStats enemyStats)
    {
        base.UpdateEffect(enemyStats);

        tickTimeElapsed += Time.deltaTime;
        if (tickTimeElapsed >= tickTime)
        {
            tickTimeElapsed = 0;
            DamageOrHealing.DealDamage(buffSource.GetComponent<NetworkBehaviour>(), enemyStats.gameObject.GetComponent<NetworkBehaviour>(), tickValue, false, false);
        }
    }

    public override Buff Clone()
    {
        Warrior_TearingSlashDoT clone = (Warrior_TearingSlashDoT)this.MemberwiseClone();
        return clone;
    }
}
