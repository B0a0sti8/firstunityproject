using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasterEventTriggerStats : EventTrigger
{
    public string statDescription;
    CharacterPanelScript cps;

    private void Awake()
    {
        cps = transform.parent.parent.gameObject.GetComponent<CharacterPanelScript>();
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        WhatStatIsIt();
        TooltipScreenSpaceUIStats.ShowTooltip_Static(statDescription);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        TooltipScreenSpaceUIStats.HideTooltip_Static();
    }

    void WhatStatIsIt()
    {
        if (gameObject.name == "Armor") { statDescription = cps.ttArmor; }

        else if (gameObject.name == "Mastery") { statDescription = cps.ttMastery; }
        else if (gameObject.name == "DmgInc") { statDescription = cps.ttDmgInc; }
        else if (gameObject.name == "HealInc") { statDescription = cps.ttHealInc; }

        else if (gameObject.name == "Toughness") { statDescription = cps.ttToughness; }
        else if (gameObject.name == "PhysRed") { statDescription = cps.ttPhysRed; }
        else if (gameObject.name == "MagRed") { statDescription = cps.ttMagRed; }
        else if (gameObject.name == "IncHealInc") { statDescription = cps.ttIncHealInc; }
        else if (gameObject.name == "BlockChance") { statDescription = cps.ttBlockChance; }

        else if (gameObject.name == "Intellect") { statDescription = cps.ttIntellect; }
        else if (gameObject.name == "SkillRadInc") { statDescription = cps.ttSkillRadInc; }
        else if (gameObject.name == "SkillDurInc") { statDescription = cps.ttSkillDurInc; }
        else if (gameObject.name == "CritChance") { statDescription = cps.ttCritChance; }
        else if (gameObject.name == "CritMult") { statDescription = cps.ttCritMult; }

        else if (gameObject.name == "Charisma") { statDescription = cps.ttCharisma; }
        else if (gameObject.name == "BuffInc") { statDescription = cps.ttBuffInc; }
        else if (gameObject.name == "DebuffInc") { statDescription = cps.ttDebuffInc; }

        else if (gameObject.name == "Tempo") { statDescription = cps.ttTempo; }
        else if (gameObject.name == "TickRateMod") { statDescription = cps.ttTickRateMod; }
        else if (gameObject.name == "ActionSpeed") { statDescription = cps.ttActionSpeed; }
        else if (gameObject.name == "Evade") { statDescription = cps.ttEvade; }

        else if (gameObject.name == "LifeSteal") { statDescription = cps.ttLifeSteal; }
        else if (gameObject.name == "MoveSpeed") { statDescription = cps.ttMoveSpeed; }
    }
}
