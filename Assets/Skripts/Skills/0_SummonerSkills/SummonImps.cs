using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonImps : SkillPrefab
{
    [SerializeField] private GameObject imp;

    private bool skillEffektActive = false;

    int impCount;
    float impLifeTime;

    float elapsed;

    public override void Start()
    {
        base.Start();
        myClass = "Summoner";

        hasGlobalCooldown = true;

        needsTargetEnemy = true;
        skillRange = 20;

        hasOwnCooldown = true;
        ownCooldownTimeBase = 3f;

        castTimeOriginal = 5f;
        isSkillChanneling = true;

        elapsed = 0;
        impCount = 6;
        impLifeTime = 10;
    }

    public override void Update()
    {
        tooltipSkillDescription = "Summons a bunch of Imps around target Enemy";

        base.Update();

        if (masterChecks.isSkillInterrupted)
        { skillEffektActive = false; }

        if (masterChecks.masterIsCastFinished && skillEffektActive)
        { skillEffektActive = false; masterChecks.masterIsCastFinished = false; return; }

        if (skillEffektActive)
        {
            if (elapsed >= castTimeModified / impCount)
            {
                elapsed = 0;
                SpawnImp();
            }
            elapsed += Time.deltaTime;
        }
    }

    void SpawnImp()
    {
        Debug.Log("Summoning Dragonling");
        float x = Random.Range(2, 3);
        float y = Random.Range(2, 3);
        float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
        float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1

        Vector2 posi = (Vector2)transform.position + new Vector2(x * signx, y * signy);
        GameObject impling = Instantiate(imp, posi, Quaternion.identity);
        impling.GetComponent<NetworkObject>().Spawn();
        impling.GetComponent<MinionPetAI>().myMaster = PLAYER.transform;
        impling.GetComponent<MinionPetAI>().isInFight = true;
        impling.GetComponent<HasLifetime>().maxLifetime = impLifeTime;


        PLAYER.GetComponent<PlayerStats>().myMinions.Add(impling);
        //transform.GetComponent<SummonerClass>().mySummonerMinions.Add(impling);
    }

    public override void ConditionCheck()
    {
        //if (transform.GetComponent<SummonerClass>().myMainSummonerMinions.Count >= transform.GetComponent<SummonerClass>().maxNrOfMainSummonerMinions)
        //{ Debug.Log("Zu viele Summoner Begleiter!"); return; }

        //if (playerStats.myMainMinions.Count >= playerStats.maxNrOfMainMinions)
        //{ Debug.Log("Zu viele Begleiter!"); return; }
        base.ConditionCheck();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        elapsed = 0;
        skillEffektActive = true;
    }
}
