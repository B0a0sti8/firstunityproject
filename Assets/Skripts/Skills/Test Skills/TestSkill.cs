using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : SkillPrefab
{
    public float duration;
    public float value;

    public Sprite buffImage;
    ArmorBuff buff = new ArmorBuff();

    public override void Start()
    {
        ownCooldownTimeBase = 5f;
        duration = 10f;
        value = 50f;

        base.Start();
    }

    public override void Update()
    {
        tooltipSkillDescription = "Remove most <color=lightblue>Buffs/Debuffs</color> on yourself.\n\n" +
            "You gain " + value + " <color=grey>Armor</color> for " + duration + "s.";

        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        
        PLAYER.GetComponent<BuffManager>().DispellBuffs();
        
        Buff clone = buff.Clone();
        PLAYER.GetComponent<BuffManager>().AddBuff(clone, buffImage, duration, value);
        
        // Play Animation
        // Play Soundeffect
        // Skilleffect
    }
}

//GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += 100f; // DPS-Meter

// create skill skript
// create input
// add to Input_Skills
// add skill skript to button + adjust settings
// drag button in Input_Skills skript
// add On Click () event to button (drag in button, choose StartSkillChecks)