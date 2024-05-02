using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BuffMinionAttackSpeed : SkillPrefab
{
    public float duration = 10f;
    public float value;

    public Sprite buffImage;
    AttackSpeedBoostBuff buff = new AttackSpeedBoostBuff();

    public override void Start()
    {
        base.Start();

        hasGlobalCooldown = true;
        isSelfCast = true;
        ownCooldownTimeBase = 10f;
        castTimeOriginal = 1.5f;
        value = 2f;

        value *= playerStats.buffInc.GetValue();

    }

    public override void Update()
    {
        tooltipSkillDescription = "Buffs all your Minions and your Action speed";

        base.Update();
    }

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
                mn.GetComponent<BuffManagerNPC>().AddBuff(clone, buffImage, duration, value);
            }
            
        }

        foreach (var minio in playerStats.myMainMinions)
        {
            minio.TryGet(out NetworkObject mn);
            Buff clone = buff.Clone();
            clone.buffSource = PLAYER;
            if (mn  != null)
            {
                mn.GetComponent<BuffManagerNPC>().AddBuff(clone, buffImage, duration, value);
            }
           
        }

    }
}
