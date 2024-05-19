using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BuffMainMinionDamageAndHealing : SkillPrefab
{
    public float buffBaseDuration;
    public float buffBaseValue;
    Color myNewColor;
    bool isConsumingLesserMinions;
    float maxConsumingTimer;
    float elapsedConsumingTimer;
    int ConsumingTimersTillBuffIsGiven;
    int ConsumingTimersTillBuffIsGivenMax;
    bool canIIncreaseBuffDuration;

    //public Sprite buffImage;
    MainMinionBuffDamageAndHealingBuff buff = new MainMinionBuffDamageAndHealingBuff();
    SummonerClass mySummonerClass;

    public override void Start()
    {
        base.Start();

        hasGlobalCooldown = true;
        isCastOnSelf = true;
        hasOwnCooldown = true;
        ownCooldownTimeBase = 120f;
        castTimeOriginal = 1.5f;
        buffBaseValue = 0.5f;
        buffBaseDuration = 15f;

        tooltipSkillDescription = "";
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        maxConsumingTimer = mySummonerClass.buffMainMinionDmgAndHealIsConsumingCooldownDec;
        elapsedConsumingTimer = 0;
        ConsumingTimersTillBuffIsGivenMax = playerStats.myMainMinions.Count;
        ConsumingTimersTillBuffIsGiven = 0;
        canIIncreaseBuffDuration = false;

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
                GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(mn.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "MainMinionBuffDamageAndHealingBuff", "MainMinionBuffDamageAndHealingBuff", true, buffDuration, 0, buffValue);
            }
        }
        isConsumingLesserMinions = true;
    }

    public override void Update()
    {
        base.Update();

        //Debug.Log("Update");
        if (mySummonerClass.buffMainMinionDmgAndHealIsConsuming & isConsumingLesserMinions) // Wenn der Skill aktiv ist und das "Friss kleinere Minions" Talent geskillt wurde.
        {
            for (int i = 0; i < ConsumingTimersTillBuffIsGivenMax; i++)     // Mehr Main Minions fressen schneller
            {
                elapsedConsumingTimer += Time.deltaTime;        // Timer wird für jeden Main Minion erhöht.
                //Debug.Log("Counting");
            }


            if (elapsedConsumingTimer >= maxConsumingTimer)     
            {
                elapsedConsumingTimer = 0;
                ConsumingTimersTillBuffIsGiven += 1;            // Damit man nicht schneller buffs kriegt wenn man mehr main minions hat, hier zusätzlicher timer
                                                                // Hier wird ein Minion gefressen. Wie gesagt, mehr Main Minions fressen schneller, kriegen aber gleich schnell buffs.

                Debug.Log("Counter full"); 
                canIIncreaseBuffDuration = false;
                isConsumingLesserMinions = false;
                for (int i = playerStats.myMinions.Count - 1; i >= 0 ; i--)
                {
                    Debug.Log("Going through all minions");
                    GameObject miniMinion = playerStats.myMinions[i];
                    if (miniMinion.name == "Imp(Clone)" || miniMinion.name == "SpiritWolf(Clone)")
                    {
                        RemoveMyMiniMinionServerRpc(miniMinion.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), i);
                        canIIncreaseBuffDuration = true;
                        isConsumingLesserMinions = true;
                        Debug.Log("Gettting Snack");
                        break;

                    }
                }
            }

            if (ConsumingTimersTillBuffIsGiven >= ConsumingTimersTillBuffIsGivenMax & canIIncreaseBuffDuration) // Counter ist voll, Minion gefressen, alle Main Minions erhalten einen Buff!
            {
                Debug.Log("Increasing Buff Duration");
                ConsumingTimersTillBuffIsGiven = 0;
                IncreaseBuffDuration();
            }
        }
    }

    private void IncreaseBuffDuration()
    {
        foreach (GameObject mainMinion in playerStats.myMainMinions)
        {
            float myNewBuffDuration = 0;
            float myNewBuffValue = 0;
            BuffManagerNPC bfMng;
            bfMng = mainMinion.GetComponent<BuffManagerNPC>();
            if (bfMng != null)
            {
                for (int i = 0; i < bfMng.buffs.Count; i++)
                {
                    Buff minionBuff = bfMng.buffs[i];
                    if (minionBuff.buffName == "MainMinionBuffDamageAndHealingBuff" & minionBuff.buffSource == PLAYER)
                    {
                        myNewBuffDuration = minionBuff.durationTimeLeft + mySummonerClass.buffMainMinionDmgAndHealIsConsumingDurationInc * playerStats.skillDurInc.GetValue();
                        myNewBuffValue = minionBuff.value;
                    }
                }

                if (myNewBuffDuration != 0)
                {
                    bfMng.RemoveBuffProcedure(PLAYER.GetComponent<NetworkObject>(), "MainMinionBuffDamageAndHealingBuff", false);
                    GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(mainMinion.GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "MainMinionBuffDamageAndHealingBuff", "MainMinionBuffDamageAndHealingBuff", false, myNewBuffDuration, 0, myNewBuffValue);
                }
            }
        }
    }

    //[ServerRpc]
    //private void IncreaseBuffDurationServerRpc(NetworkObjectReference minionRef, int buffNumber, float buffDurationInc)
    //{
    //    Debug.Log("Increasing Buff Duration Server");
    //    //minionRef.TryGet(out NetworkObject minion);
    //    //minion.GetComponent<BuffManagerNPC>().buffs[buffNumber].durationTimeLeft += buffDurationInc;
    //    //minion.GetComponent<BuffManagerNPC>().buffs[buffNumber].duration += buffDurationInc;
    //    IncreaseBuffDurationClientRpc(minionRef, buffNumber, buffDurationInc);
    //}

    //[ClientRpc]
    //private void IncreaseBuffDurationClientRpc(NetworkObjectReference minionRef, int buffNumber, float buffDurationInc)
    //{
    //    Debug.Log("Increasing Buff Duration Client"); 
    //    minionRef.TryGet(out NetworkObject minion);
    //    minion.GetComponent<BuffManagerNPC>().buffs[buffNumber].durationTimeLeft += buffDurationInc;
    //    minion.GetComponent<BuffManagerNPC>().buffs[buffNumber].duration += buffDurationInc;
    //}

    [ServerRpc]
    private void RemoveMyMiniMinionServerRpc(NetworkObjectReference minionRef, NetworkObjectReference playerRef, int minionIndex)
    {
        minionRef.TryGet(out NetworkObject minio);
        playerRef.TryGet(out NetworkObject myPlayer);

        if (minio != null && myPlayer != null)
        {
            myPlayer.GetComponent<PlayerStats>().myMinions.RemoveAt(minionIndex);
            Destroy(minio.gameObject);
        }
    }


    public void ShowMainMinionBuffEffect(GameObject minion)
    {
        ShowMainMinionBuffEffectServerRpc(minion.GetComponent<NetworkObject>());
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShowMainMinionBuffEffectServerRpc(NetworkObjectReference minionRef)
    {
        ShowMainMinionBuffEffectClientRpc(minionRef);
    }

    [ClientRpc]
    public void ShowMainMinionBuffEffectClientRpc(NetworkObjectReference minionRef)
    {
        minionRef.TryGet(out NetworkObject minion);
        minion.transform.Find("ImageComponent").Find("GameObject").GetComponent<SpriteRenderer>().color = new Color32(191, 76, 76, 255);
    }

    public void HideMainMinionBuffEffect(GameObject minion)
    {
        HideMainMinionBuffEffectServerRpc(minion.GetComponent<NetworkObject>());
    }

    [ServerRpc(RequireOwnership = false)]
    public void HideMainMinionBuffEffectServerRpc(NetworkObjectReference minionRef)
    {
        HideMainMinionBuffEffectClientRpc(minionRef);
    }

    [ClientRpc]
    public void HideMainMinionBuffEffectClientRpc(NetworkObjectReference minionRef)
    {
        minionRef.TryGet(out NetworkObject minion);
        minion.transform.Find("ImageComponent").Find("GameObject").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

}
