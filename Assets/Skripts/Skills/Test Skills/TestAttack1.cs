using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack1 : SkillPrefab
{
    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        Debug.Log("Attack1");
        GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += 100f;
        //player.currentMana -= 100;
    }
}