using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TestEnergyDrain : SkillPrefab
{
    public float damage = 200f;

    public override void Start()
    {
        ownCooldownTimeBase = 12f;

        base.Start();
    }

    public override void Update()
    {
        tooltipSkillDescription = "Deal <color=orange>" + damage + " damage</color> to any target.\n" +
            "<color=green>Gain " + (damage / 2) + "Life</color> (half the damage).";

        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        
        DealDamage(damage);

        DamageOrHealing.DoHealing(PLAYER.GetComponent<NetworkBehaviour>(), PLAYER.GetComponent<NetworkBehaviour>(), damage / 2);
    }
}