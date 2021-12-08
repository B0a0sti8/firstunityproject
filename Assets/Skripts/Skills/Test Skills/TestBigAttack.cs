using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBigAttack : SkillPrefab
{
    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        Debug.Log("Activate BigAttack: 400 Damage");
        GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += 400f;
    }
}