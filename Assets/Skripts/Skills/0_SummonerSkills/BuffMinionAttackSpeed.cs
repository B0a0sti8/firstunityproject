using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BuffMinionAttackSpeed : SkillPrefab
{
    public float buffDuration = 10f;
    public float buffValue;

    public Sprite buffImage;
    AttackSpeedBoostBuff buff = new AttackSpeedBoostBuff();

    public override void Start()
    {
        base.Start();

        hasGlobalCooldown = true;
        isCastOnSelf = true;
        ownCooldownTimeBase = 10f;
        castTimeOriginal = 1.5f;
        buffValue = 2f;

        buffValue *= playerStats.buffInc.GetValue();
        tooltipSkillDescription = "Buffs all your Minions and your Action speed";
    }

    //public override void Update()
    //{
    //    base.Update();
    //}

    public override void SkillEffect()
    {
        base.SkillEffect();

        Debug.Log("Buffing all my Minions!");

        foreach (var minio in playerStats.myMinions)
        {
            minio.TryGet(out NetworkObject mn);
            Buff clone = buff.Clone();
            clone.buffSource = PLAYER;
            if (mn != null)
            {
                //mn.GetComponent<BuffManagerNPC>().AddBuff(clone, buffImage, duration, value);
                //mn.GetComponent<BuffManagerNPC>().AddBuffProcedure(mn.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "AttackSpeedBoostBuff", "AttackSpeedBoostBuff", false, 0, 0, buffValue);
                GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(mn.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "AttackSpeedBoostBuff", "AttackSpeedBoostBuff", false, buffDuration, 0, buffValue);
            }
        }

        foreach (var minio in playerStats.myMainMinions)
        {
            minio.TryGet(out NetworkObject mn);
            Buff clone = buff.Clone();
            clone.buffSource = PLAYER;
            if (mn  != null)
            {
                //mn.GetComponent<BuffManagerNPC>().AddBuff(clone, buffImage, duration, value);
                //mn.GetComponent<BuffManagerNPC>().AddBuffProcedure(mn.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "BuffMinionAttackSpeed", "BuffMinionAttackSpeed", false, 0, 0, buffValue);
                GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(mn.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "AttackSpeedBoostBuff", "AttackSpeedBoostBuff", false, buffDuration, 0, buffValue);
            }
        }
    }
}
