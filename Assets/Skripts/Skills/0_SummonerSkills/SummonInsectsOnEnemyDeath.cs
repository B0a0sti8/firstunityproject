using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonInsectsOnEnemyDeath : SkillPrefab
{
    public float buffDuration = 15f;
    public float buffTickValue;
    public float buffTickTime;
    public float buffValue;

    private float insectDamage;
    private float insectCount;
    private float insectLifetime;


    SummonerClass mySummonerClass;

    public GameObject myInsectPrefab;

    public Sprite buffImage;
    SummonInsectsOnEnemyDeathBuff buff = new SummonInsectsOnEnemyDeathBuff();

    public override void Start()
    {
        base.Start();

        buffValue = 0;

        needsTargetEnemy = true;
        hasGlobalCooldown = true;
        isCastOnSelf = false;
        ownCooldownTimeBase = 10f;
        castTimeOriginal = 0f;
        buffTickValue = 2f;
        buffTickTime = 3f;
        skillRange = 10;

        buffTickValue *= playerStats.buffInc.GetValue();
        tooltipSkillDescription = "Summons Insects on Enemy Death";

        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();

        insectDamage = 10f;
        insectCount = 3f;
        insectLifetime = 5f;
    }

    public override void SkillEffect()
    {

        base.SkillEffect();

        float insectCountModified = insectCount + mySummonerClass.increasedInsectSummon;
        float insectLifeTimeModified = (insectLifetime + mySummonerClass.increasedMinionDuration) * playerStats.skillDurInc.GetValue();
        float insectDamageModified = insectDamage * (1+ mySummonerClass.increasedMinionDamage);

        Buff clone = buff.Clone();
        clone.buffSource = PLAYER;

        // Zwei zusätzliche Parameter für den Schaden und die Anzahl der minions
        GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(currentTargets[0].GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), "SummonInsectsOnEnemyDeathBuff", "SummonInsectsOnEnemyDeathBuff", false, buffDuration, 0, buffValue, insectDamageModified, insectCountModified, insectLifeTimeModified);
        //currentTargets[0].GetComponent<BuffManagerNPC>().AddBuff(clone, buffImage, duration, buffTickTime, buffTickValue);
    }


    // Dieser Code wird hier laufen gelassen, weil Buffs nicht wirklich Rpcs senden können...

    public void SpawnInsects(NetworkObjectReference summoningPlayer, NetworkObjectReference targetRef, int insectCount, float insectDamage, float insectLifetime)
    {
        SpawnInsectsServerRpc(summoningPlayer, targetRef, insectCount, insectDamage, insectLifetime);
    }


    [ServerRpc(RequireOwnership = false)]
    public void SpawnInsectsServerRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference targetRef, int myInsectCount, float myInsectDamage, float myInsectLifetime)
    {
        //Debug.Log("Summon Stone Golem Server RPC!");
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        targetRef.TryGet(out NetworkObject target);
        GameObject mTarget = target.gameObject;

        //Debug.Log("Server Rpc:  Trying to summon insects: " + insectCount);

        for (int i = 0; i < myInsectCount; i++)
        {
            Debug.Log("Creating Postiion");

            Random.InitState(i);
            Debug.Log(i);
            float x = Random.Range(1f, 2f);
            float y = Random.Range(1f, 2f);
            float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
            float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1

            if (sumPla != null)
            {
                Debug.Log("Creating Position 2");
                Vector2 posi = (Vector2)mTarget.transform.position + new Vector2(x * signx, y * signy);
                Debug.Log(posi);
                GameObject summonerInsect = sumPla.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonInsectsOnEnemyDeath>().myInsectPrefab;
                GameObject summonerInsec = GameObject.Instantiate(summonerInsect, posi, Quaternion.identity);
                summonerInsec.GetComponent<NetworkObject>().Spawn();
                summonerInsec.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
                summonerInsec.GetComponent<MinionPetAI>().isInFight = true;
                summonerInsec.GetComponent<HasLifetime>().maxLifetime = myInsectLifetime;
                summonerInsec.GetComponent<MeleeEnemyAttackTest>().baseAttackDamage = myInsectDamage;

                sumPla.GetComponent<PlayerStats>().myMinions.Add(summonerInsec);

                NetworkObjectReference insectRef = (NetworkObjectReference)summonerInsec;
                SpawnInsectsClientRpc(summoningPlayer, insectRef);
            }
        }
    }

    [ClientRpc]
    public void SpawnInsectsClientRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference insectRef)
    {
        Debug.Log("Setting InsectMaster!");
        summoningPlayer.TryGet(out NetworkObject sour);
        insectRef.TryGet(out NetworkObject summIns);

        GameObject sumPla = sour.gameObject;
        summIns.gameObject.GetComponent<MinionPetAI>().myMaster = sumPla.transform;

        Debug.Log(summIns.gameObject);
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
