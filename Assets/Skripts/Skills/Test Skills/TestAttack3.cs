using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack3 : SkillPrefab
{
    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        Debug.Log("Attack3");
        GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += 150f;
    }
}