using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SummonImps : SkillPrefab
{
    [SerializeField] private GameObject imp;
    SummonerClass mySummonerClass;

    private float impBaseDamage;

    int impCount;
    float impLifeTime;
    float impLifeTimeBase;

    float elapsed;

    public override void Start()
    {
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
        animationTime = 5f;
        base.Start();
        myClass = "Summoner";

        impBaseDamage = 12f;

        hasGlobalCooldown = true;

        needsTargetEnemy = true;
        skillRange = 20;

        hasOwnCooldown = true;
        ownCooldownTimeBase = 3f;

        castTimeOriginal = 5f;
        isSkillChanneling = true;

        elapsed = 0;
        impCount = 6;
        impLifeTimeBase = 10;
    }

    public override void Update()
    {
        tooltipSkillDescription = "Summons a bunch of Imps around the target Enemy";

        base.Update();

        if (masterChecks.isSkillInterrupted)
        { isChannelingSkillEffectActive = false; }

        if (isChannelingSkillEffectActive)
        {
            if (elapsed >= castTimeModified / impCount)
            {
                elapsed = 0;
                SpawnImp();
            }
            elapsed += Time.deltaTime;
        }
    }

    public override void StartCasting()
    {
        PlaySkillAnimation("Summoner", "Summoner_summon2");
        base.StartCasting();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        elapsed = 0;
    }

    void SpawnImp()
    {
        impLifeTime = (impLifeTimeBase + mySummonerClass.increasedMinionDuration) * playerStats.skillDurInc.GetValue();
        float impDamage = impBaseDamage * (1 + mySummonerClass.increasedMinionDamage);

        mySummonerClass.SummonerClass_OnMinionSummoned();

        // Bittet den Server um Spawn des Imps, schickt Referenz des Spielers. Wenn kein Target verfügbar ist, wird Imp um den Spieler herum gespawnt.
        NetworkObjectReference playerReference = (NetworkObjectReference)PLAYER;
        if (interactionCharacter.focus != null)
        {
            NetworkObjectReference enemyReference = (NetworkObjectReference)interactionCharacter.focus.gameObject;
            SpawnImpServerRpc(enemyReference, playerReference, impDamage, impLifeTime);
            //Debug.Log("MyImpLifetime = " + impLifeTime);
            //Debug.Log("Skill duration charakterstats: " + playerStats.skillDurInc.GetValue());
            //Debug.Log("Additional damage Summoner class: " + mySummonerClass.increasedMinionDamage);
        }
        else
        {
            NetworkObjectReference enemyReference = (NetworkObjectReference)PLAYER;
            SpawnImpServerRpc(enemyReference, playerReference, impDamage, impLifeTime);
            //Debug.Log("MyImpLifetime = " + impLifeTime);
            //Debug.Log("Skill duration charakterstats: " + playerStats.skillDurInc.GetValue());
            //Debug.Log("Additional Duration Summoner class: " + mySummonerClass.increasedMinionDuration);
        }
    }

    [ServerRpc]
    private void SpawnImpServerRpc(NetworkObjectReference targetEnemy, NetworkObjectReference summoningPlayer, float impDamage, float impDuration)
    {
        // Holt sich den Summoning Player aus der Network-Referenz
        //Debug.Log("Summon Imp Server RPC!");
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        targetEnemy.TryGet(out NetworkObject targE);
        GameObject targEn = targE.gameObject;

        // Erzeugt zufällige Koordinaten
        float x = Random.Range(2, 5);
        float y = Random.Range(2, 5);
        float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
        float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1

        if (sumPla != null)
        {
            // Baut aus den zufälligen Koordinaten den Spawnpunkt des Imps. Setzt das Herrchen fest und setzt den Imp in den Kampf. Spawnt den Imp Serverseitig
            Vector2 posi = (Vector2)targEn.transform.position + new Vector2(x * signx, y * signy);
            GameObject impling = Instantiate(imp, posi, Quaternion.identity);
            impling.GetComponent<NetworkObject>().Spawn();
            impling.GetComponent<MinionPetAI>().isInFight = true;
            impling.GetComponent<HasLifetime>().maxLifetime = impDuration;
            //Debug.Log("MyImpLifetime = " + impDuration);
            impling.GetComponent<MeleeEnemyAttackTest>().baseAttackDamage = impDamage;

            impling.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
            sumPla.GetComponent<PlayerStats>().myMinions.Add(impling);
            //Debug.Log("Added Minion as Server" + sumPla.GetComponent<PlayerStats>().myMinions.Count);

            NetworkObjectReference implingRef = (NetworkObjectReference)impling;

            // Die ClientRpc sagt allen Clients, wer das Herrchen ist und dass das Herrchen ein Pet hat.
            SummonImpClientRpc(summoningPlayer, implingRef);
        }
    }

    [ClientRpc]
    private void SummonImpClientRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference impling, ClientRpcParams clientRpcParams = default)
    {
        // Die ClientRpc sagt allen Clients, wer das Herrchen ist und dass das Herrchen ein Pet hat.
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        impling.TryGet(out NetworkObject impl);
        GameObject impli = impl.gameObject;

        impli.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
        impli.GetComponent<MinionPetAI>().isInFight = true;
    }
}
