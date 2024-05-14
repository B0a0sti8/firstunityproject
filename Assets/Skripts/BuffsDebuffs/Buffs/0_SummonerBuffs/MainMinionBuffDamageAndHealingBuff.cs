using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMinionBuffDamageAndHealingBuff : Buff
{
    public override void StartBuffEffect(CharacterStats playerStats)
    {
        buffName = "MainMinionBuffDamageAndHealingBuff";
        buffDescription = "";
        base.StartBuffEffect(playerStats);
        ((EnemyStats)playerStats).dmgModifier.AddModifierAdd(value);
        isRemovable = false;

        buffSource.transform.Find("SkillManager").Find("Summoner").GetComponent<BuffMainMinionDamageAndHealing>().ShowMainMinionBuffEffect(playerStats.gameObject);
    }

    public override void EndBuffEffect(CharacterStats playerStats)
    {
        base.EndBuffEffect(playerStats);
        ((EnemyStats)playerStats).dmgModifier.RemoveModifierAdd(value);
        buffSource.transform.Find("SkillManager").Find("Summoner").GetComponent<BuffMainMinionDamageAndHealing>().HideMainMinionBuffEffect(playerStats.gameObject);
    }

    public override Buff Clone()
    {
        MainMinionBuffDamageAndHealingBuff clone = (MainMinionBuffDamageAndHealingBuff)this.MemberwiseClone();
        return clone;
    }

}