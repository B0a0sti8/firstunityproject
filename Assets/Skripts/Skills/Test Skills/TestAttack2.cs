using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack2 : SkillPrefab
{
    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        Debug.Log("Attack2");
        GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += 120f;
    }
}