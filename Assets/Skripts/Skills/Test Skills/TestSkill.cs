using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : SkillPrefab
{
    public override void MasterETStuff()
    {
        skillDescription = "Remove most <color=lightblue>Buffs/Debuffs</color> on yourself.";
        base.MasterETStuff();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        PLAYER.GetComponent<BuffManager>().DispellBuffs();

        // Play Animation
        // Play Soundeffect
        // Skilleffect
    }
}

// create skill skript
// create input
// add to Input_Skills
// add skill skript to button + adjust settings
// drag button in Input_Skills skript
// add On Click () event to button (drag in button, choose StartSkillChecks)