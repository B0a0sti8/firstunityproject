using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRangeEnemyAoEDebuff : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "TestEnemyAoEDot";
        buffDescription = "TestEnemyAoEDot";
        base.StartBuffEffect(playerStats);
        isRemovable = false;
        tickTimeElapsed = 0;
    }

    public override Buff Clone()
    {
        TestRangeEnemyAoEDebuff clone = (TestRangeEnemyAoEDebuff)this.MemberwiseClone();
        return clone;
    }

    public override void Update(CharacterStats playerStats)
    {
        base.Update(playerStats);
        tickTimeElapsed += Time.deltaTime;
        if (tickTimeElapsed >= tickTime)
        {
            tickTimeElapsed = 0;

            DamageOrHealing.DealDamage(buffSource, playerStats.gameObject, tickValue, false, false);
        }
    }
}
