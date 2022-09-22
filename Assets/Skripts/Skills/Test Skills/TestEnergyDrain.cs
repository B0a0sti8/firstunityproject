using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

        int critRandom = Random.Range(1, 100);
        float critChance = playerStats.critChance.GetValue();
        float critMultiplier = playerStats.critMultiplier.GetValue();

        PLAYER.GetComponent<CharacterStats>().view.RPC("GetHealing", RpcTarget.All, damage / 2, critRandom, critChance, critMultiplier);
    }
}