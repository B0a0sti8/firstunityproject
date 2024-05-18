using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TestGainLifeHot : SkillPrefabOld
{
    [Header("Hot-Stats")]
    public float instantHealing = 50f;
    public float tickTime = 2f;
    public int tickHealing = 50;
    public float duration = 10f;
    
   

    public Sprite buffImage;
    HoTBuff buff = new HoTBuff();

    public override void Start()
    {

        ownCooldownTimeBase = 2f;
        

        base.Start();

        needsTargetAlly = true;
        canSelfCastIfNoTarget = true;
        skillRange = 10;
    }

    public override void Update()
    {
        tooltipSkillDescription = "Gain <color=green>" + instantHealing + " Life</color>.\n" +
            "HOT:\n" +
            "<color=green>" + tickHealing + " Health</color> every <color=yellow>" + tickTime + "s</color>\n" +
            "Duration: <color=yellow>" + duration + "s</color>";

        base.Update();
    }

    public override void SkillEffect()
    {
       
        base.SkillEffect();

        DoHealing(instantHealing);

        Buff clone = buff.Clone();
        clone.InitializeBuff(PLAYER);
        if (interactionCharacter.focus == null)
        {
            //PLAYER.GetComponent<BuffManager>().AddBuff(clone, buffImage, duration, tickTime, tickHealing);
            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(PLAYER.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "HoTBuff", "HoTBuff", true, duration, tickTime, tickHealing);
        }
        else
        {
            Debug.Log("Anderer Charakter");
            //interactionCharacter.focus.GetComponent<BuffManager>().AddBuff(clone, buffImage, duration, tickTime, tickHealing);
            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(interactionCharacter.focus.gameObject.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "HoTBuff", "HoTBuff", true, duration, tickTime, tickHealing);
        }

        
    }
}