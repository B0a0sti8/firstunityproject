using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BuffMainMinionDamageAndHealing : SkillPrefab
{
    public float buffBaseDuration;
    public float buffBaseValue;

    //public Sprite buffImage;
    MainMinionBuffDamageAndHealingBuff buff = new MainMinionBuffDamageAndHealingBuff();

    public override void Start()
    {
        base.Start();

        hasGlobalCooldown = true;
        isSelfCast = true;
        hasOwnCooldown = true;
        ownCooldownTimeBase = 120f;
        castTimeOriginal = 1.5f;
        buffBaseValue = 0.5f;
        buffBaseDuration = 10f;


        tooltipSkillDescription = "Buffs all your Minions and your Action speed";
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        float buffValue = buffBaseValue * playerStats.buffInc.GetValue();
        float buffDuration = buffBaseDuration * playerStats.skillDurInc.GetValue();

        Debug.Log("Buffing all my Main Minions!");

        foreach (var minio in playerStats.myMainMinions)
        {
            minio.TryGet(out NetworkObject mn);
            Buff clone = buff.Clone();
            clone.buffSource = PLAYER;
            if (mn != null)
            {
                GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(mn.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "MainMinionBuffDamageAndHealingBuff", "MainMinionBuffDamageAndHealingBuff", false, buffDuration, 0, buffValue);
            }
        }

        foreach (var minio in playerStats.myMainMinions)
        {
            minio.TryGet(out NetworkObject mn);
            Buff clone = buff.Clone();
            clone.buffSource = PLAYER;
            if (mn != null)
            {
                GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(mn.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "MainMinionBuffDamageAndHealingBuff", "MainMinionBuffDamageAndHealingBuff", false, buffDuration, 0, buffValue);
            }
        }
    }
}
