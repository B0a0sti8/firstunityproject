using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnergyDrain : SkillPrefab
{
    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        Debug.Log("Activate EnergyDrain: 200 Damage");
        playerStats.currentHealth += 30;
        GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += 200f;
    }
}