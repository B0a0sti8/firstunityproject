using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonSpiritWolfOnSkillBuff : Buff
{
    CharacterStats myCurrentPlayerStats;
    float wolfDamage;
    float wolfDuration;

    public override void StartBuffUI()
    {
        base.StartBuffUI();
        buffName = "SummonSpiritWolfOnSkillBuff";
        buffDescription = "Target has a chance to summon a spirit wolf for each skill";
    }

    public override void StartBuffEffect(CharacterStats characterstats)
    {
        base.StartBuffEffect(characterstats);

        PlayerStats playerStats = (PlayerStats)characterstats;
        isRemovable = false;
        myCurrentPlayerStats = playerStats;

        wolfDamage = additionalValue1;
        wolfDuration = additionalValue2;

        SubscribeToOnCastedSkill(playerStats);
    }

    public void SubscribeToOnCastedSkill(PlayerStats playerStats)
    {
        // Nachdem das in StartBuffEffekt gestartet wird, sollte hier nur der Server laufen.
        // Debug.Log("Still on buff: Subscribing to event");
        buffSource.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonSpiritWolfOnSkill>().SubscribeToOnCastedSkill(playerStats.gameObject);
    }

    public void UnsubscribeFromOnCastedSkill(PlayerStats playerStats)
    {
        // Debug.Log("Still on buff: Unsubbing");
        buffSource.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonSpiritWolfOnSkill>().UnsubscribeFromOnCastedSkill(playerStats.gameObject);
    }

    public override void EndBuffEffect(CharacterStats characterstats)
    {
        base.EndBuffEffect(characterstats);
        PlayerStats playerStats = (PlayerStats)characterstats;
        UnsubscribeFromOnCastedSkill(playerStats);
    }

    public override Buff Clone()
    {
        SummonSpiritWolfOnSkillBuff clone = (SummonSpiritWolfOnSkillBuff)this.MemberwiseClone();
        return clone;
    }
}
