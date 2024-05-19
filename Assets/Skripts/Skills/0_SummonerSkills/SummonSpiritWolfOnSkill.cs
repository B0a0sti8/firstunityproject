using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonSpiritWolfOnSkill : SkillPrefab
{
    public float buffDurationBase;
    public float buffValueBase;

    float wolfDamage;
    float wolfLifetime;

    [SerializeField] private GameObject myWolfPrefab;

    //public Sprite buffImage;
    SummonSpiritWolfOnSkillBuff buff = new SummonSpiritWolfOnSkillBuff();

    SummonerClass mySummonerClass;

    public override void Start()
    {
        base.Start();
        buffDurationBase = 15f;
        hasGlobalCooldown = true;
        hasOwnCooldown = true;
        ownCooldownTimeBase = 60f;

        canSelfCastIfNoTarget = true;
        isCastOnSelf = false;
        needsTargetAlly = true;

        castTimeOriginal = 0f;
        skillRange = 10f;

        wolfLifetime = 10f;
        wolfDamage = 10f;

        buffValueBase = 0.52f;


        tooltipSkillDescription = "Buffs one Player. Target has a chance to summon a spirit wolf on skill";

        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
    }

    public override void ConditionCheck()
    {
        //ownCooldownTimeBase = mySummonerClass.summonerSummonSpiritWolfOnSkillCooldown;
        base.ConditionCheck();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        float buffDuration = (buffDurationBase + mySummonerClass.summonerSummonSpiritWolfOnSkillDurationInc) * playerStats.skillDurInc.GetValue();

        float buffValue = buffValueBase * playerStats.buffInc.GetValue();
        if (currentTargets[0].tag == "Player")
        {
            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(currentTargets[0], PLAYER, "SummonSpiritWolfOnSkillBuff", "SummonSpiritWolfOnSkillBuff", false, buffDuration, 0f, buffValue);
        }
    }

    public void SubscribeToOnCastedSkill(NetworkObjectReference mySubscribedPlayerRef)
    {
        // Dieser Code resultiert aus StartBuffEffekt und sollte daher nur auf dem Server laufen... Nach meinem Verständnis ist damit eine Server RPc nicht nötig.
        //if (IsServer) Debug.Log("Hallo ichj bin der Server, Sub");
        //else Debug.Log("Ich bin kein Server, habe hier eigentlich nichts zu suchen...");

        SubscribeToOnCastedSkillClientRpc(mySubscribedPlayerRef);
    }

    public void UnsubscribeFromOnCastedSkill(NetworkObjectReference mySubscribedPlayerRef)
    {
        // Dieser Code resultiert aus StartBuffEffekt und sollte daher nur auf dem Server laufen... Nach meinem Verständnis ist damit eine Server RPc nicht nötig.
        //if (IsServer) Debug.Log("Hallo ich bin der Server, Unsub");
        //else Debug.Log("Ich bin kein Server, habe hier eigentlich nichts zu suchen...");

        UnsubscribeFromOnCastedSkillClientRpc(mySubscribedPlayerRef);
    }

    [ClientRpc]
    public void SubscribeToOnCastedSkillClientRpc(NetworkObjectReference mySubscribedPlayerRef)
    {
        mySubscribedPlayerRef.TryGet(out NetworkObject mySubscribedPlayer);
        //Debug.Log("ClientRpc, sollten alle sehen, sub");
        if (PLAYER.GetComponent<NetworkObject>().IsOwner) //PLAYER.GetComponent<NetworkObject>().IsOwner
        {
            //Debug.Log("MyPlayer! Sollte nur einer sehen. Sub");
            //Debug.Log("In ref enthalten: " + mySubscribedPlayer);
            mySubscribedPlayer.GetComponent<PlayerStats>().onCastedSkill += ChanceToSummonSpiritWolf;
        } // Debug.Log("Not my Player... Returning"); 
    }

    [ClientRpc]
    public void UnsubscribeFromOnCastedSkillClientRpc(NetworkObjectReference mySubscribedPlayerRef)
    {
        mySubscribedPlayerRef.TryGet(out NetworkObject mySubscribedPlayer);
        //Debug.Log("ClientRpc, sollten alle sehen, unsub");
        if (PLAYER.GetComponent<NetworkObject>().IsOwner) //PLAYER.GetComponent<NetworkObject>().IsOwner
        {
            //Debug.Log("MyPlayer! Sollte nur einer sehen. Unsub");
            //Debug.Log("In ref enthalten: " + mySubscribedPlayer);
            mySubscribedPlayer.GetComponent<PlayerStats>().onCastedSkill -= ChanceToSummonSpiritWolf;
        } // Debug.Log("Not my Player... Returning");
    }

    void ChanceToSummonSpiritWolf(PlayerStats myCarrierPlayerStats)
    {
        if (!PLAYER.GetComponent<NetworkObject>().IsOwner)
        {
            //Debug.Log("Spieler gehört mir nicht. ich sollte hier nie sein.");
            return;
        }
        else
        {
            //Debug.Log("Das ist mein Spieler, ich sollte hier sein.");
            //Debug.Log("Mein Buffträger. Bin ich der Owner? " + myCarrierPlayerStats.gameObject.GetComponent<NetworkObject>().IsOwner);
        }
        myCarrierPlayerStats.gameObject.GetComponent<InteractionCharacter>().GetCurrentTargetForMultiplayer();
        //Debug.Log("Should also be seen on all clients. Test.");
        float buffValue = buffValueBase * playerStats.buffInc.GetValue();
        //Debug.Log("Chance to Summon Spirit Wolf.");

        int randomNr = Random.Range(0, 1000);
        if (buffValue * 1000 > randomNr)
        {
            //Debug.Log("Übergebe: " + myCarrierPlayerStats.gameObject);
            //Debug.Log("Das hier sollte nur einer sehen. Summone Wolf.");
            SummonSpiritWolf(PLAYER.GetComponent<NetworkObject>(), myCarrierPlayerStats.gameObject.GetComponent<NetworkObject>());
        }
    }

    public void SummonSpiritWolf(NetworkObjectReference summoningPlayer, NetworkObjectReference targetRef)
    {
        //Debug.Log("Das hier sollte nur einer sehen. Summone Wolf 2.");
        SummonSpiritWolfServerRpc(summoningPlayer, targetRef, wolfDamage * (1 + mySummonerClass.summonerSummonSpiritWolfOnSkillWolfDamageInc) * playerStats.dmgInc.GetValue(), (wolfLifetime + mySummonerClass.summonerSummonSpiritWolfOnSkillWolfDurationInc) * playerStats.skillDurInc.GetValue());
        targetRef.TryGet(out NetworkObject test);
        //Debug.Log("Erhalte und gebe weiter: " + test.gameObject);
    }


    [ServerRpc]
    public void SummonSpiritWolfServerRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference targetRef, float wDamage, float wDuration)
    {
        //Debug.Log("Das hier sollte nur der Server sehen. Summone Wolf ServerRpc.");
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        targetRef.TryGet(out NetworkObject target);
        GameObject mTarget = target.gameObject;

        //mTarget.GetComponent<InteractionCharacter>().GetCurrentTargetForMultiplayer();

        Debug.Log("ServerRpc empfange" + mTarget);
        Debug.Log(sumPla); Debug.Log(mTarget.GetComponent<InteractionCharacter>());
        Debug.Log(mTarget.GetComponent<InteractionCharacter>().focus);
        Debug.Log(mTarget.GetComponent<InteractionCharacter>().focus.transform);

        float x = Random.Range(1f, 2f);
        float y = Random.Range(1f, 2f);
        float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
        float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1

        if (sumPla != null)
        {
            //Debug.Log("Creating Position 2");
            Vector2 posi = (Vector2)mTarget.transform.position + new Vector2(x * signx, y * signy);
            //Debug.Log(posi);
            GameObject summonerWolf = sumPla.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonSpiritWolfOnSkill>().myWolfPrefab;
            GameObject sumWo = GameObject.Instantiate(summonerWolf, posi, Quaternion.identity);
            sumWo.GetComponent<NetworkObject>().Spawn();
            sumWo.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
            sumWo.GetComponent<MinionPetAI>().isInFight = true;

            //Debug.Log(mTarget); Debug.Log(mTarget.GetComponent<InteractionCharacter>().focus);
            sumWo.GetComponent<MinionPetAI>().ForceAggroToTarget(mTarget.GetComponent<InteractionCharacter>().focus.transform);
            sumWo.GetComponent<HasLifetime>().maxLifetime = wDuration;
            sumWo.GetComponent<MeleeEnemyAttackTest>().baseAttackDamage = wDamage;

            sumPla.GetComponent<PlayerStats>().myMinions.Add(sumWo);

            NetworkObjectReference wolfRef = (NetworkObjectReference)sumWo;
            SummonSpiritWolfClientRpc(summoningPlayer, wolfRef);
        }
    }

    [ClientRpc]
    public void SummonSpiritWolfClientRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference wolfRef)
    {
        summoningPlayer.TryGet(out NetworkObject sour);
        wolfRef.TryGet(out NetworkObject summWo);

        GameObject sumPla = sour.gameObject;
        summWo.gameObject.GetComponent<MinionPetAI>().myMaster = sumPla.transform;

        Debug.Log(summWo.gameObject);
        Debug.Log(sumPla);

        if (sour.GetComponent<NetworkObject>().IsOwner)
        {
            if (mySummonerClass.hasCastSpeedOnMinionSummonedTalent)
            {
                mySummonerClass.SummonerClass_OnMinionSummoned();
                Debug.Log("I'm the owner, I get cast speed.");
            }
        }
    }
}
