using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DarkBolt : SkillPrefab
{
    public float damage = 400f;
    [SerializeField] GameObject myProjectile;
    SummonerClass mySummonerClass;
    [SerializeField] GameObject myImp;
    float impBaseDamage;
    float impLifeTimeBase;

    public override void Start()
    {
        myClass = "Summoner";
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
        castTimeOriginal = 1.5f;
        ownCooldownTimeBase = 3f;
        needsTargetEnemy = true;
        skillRange = 20;
        hasGlobalCooldown = true;

        impBaseDamage = 12f;
        impLifeTimeBase = 10;

        base.Start();
    }

    public override void Update()
    {
        tooltipSkillDescription = "Fire a Bolt against target Enemy for some Big Ass Damage.";
        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        float impDamage = impBaseDamage * (1 + mySummonerClass.increasedMinionDamage);
        float impLifeTime = (impLifeTimeBase + mySummonerClass.increasedMinionDuration) * playerStats.skillDurInc.GetValue();

        float tOA = 0.2f;

        GameObject targetRefe = currentTargets[0];
        SpawnDarkBoltServerRpc(PLAYER.GetComponent<NetworkObject>(), tOA, targetRefe.GetComponent<NetworkObject>());

        if (mySummonerClass.doesDarkboltIncreaseMinionLifetime) mySummonerClass.SummonerClass_IncreaseLivingMinionDuration(mySummonerClass.darkBoltLifeTimeIncrease);

        int myChance = Random.Range(0, 100);
        if (mySummonerClass.darkBoltChanceToSpawnImp*100 > myChance)
        {
            SpawnImpServerRpc(currentTargets[0].GetComponent<NetworkObject>(), PLAYER.GetComponent<NetworkObject>(), impDamage, impLifeTime);
        }


        StartCoroutine(DelayedDamage(tOA));
    }

    [ServerRpc]
    private void SpawnDarkBoltServerRpc(NetworkObjectReference myPlayerRef, float timeOfArrival, NetworkObjectReference  targetRef)
    {
        myPlayerRef.TryGet(out NetworkObject myPlayer);
        targetRef.TryGet(out NetworkObject myTarget);

        GameObject project = Instantiate(myProjectile, myPlayer.transform.position, Quaternion.identity);
        //project.GetComponent<NetworkObject>().Spawn();
        project.GetComponent<ProjectileFlyToTarget>().target = myTarget.transform;
        project.GetComponent<ProjectileFlyToTarget>().timeToArrive = timeOfArrival;

        SpawnDarkBoltClientRpc(myPlayerRef, timeOfArrival, targetRef);
    }

    [ClientRpc]
    private void SpawnDarkBoltClientRpc(NetworkObjectReference myPlayerRef, float timeOfArrival, NetworkObjectReference targetRef)
    {
        myPlayerRef.TryGet(out NetworkObject myPlayer);
        targetRef.TryGet(out NetworkObject myTarget);


        GameObject project = Instantiate(myProjectile, myPlayer.transform.position, Quaternion.identity);
        project.GetComponent<ProjectileFlyToTarget>().target = myTarget.transform;
        project.GetComponent<ProjectileFlyToTarget>().timeToArrive = timeOfArrival;
    }

    IEnumerator DelayedDamage(float tOA)
    {
        yield return new WaitForSeconds(tOA);
        DealDamage(damage * mySummonerClass.darkBoltDamageModifier * playerStats.dmgInc.GetValue());
    }

    [ServerRpc]
    private void SpawnImpServerRpc(NetworkObjectReference targetEnemy, NetworkObjectReference summoningPlayer, float impDamage, float impDuration)
    {
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        targetEnemy.TryGet(out NetworkObject targE);
        GameObject targEn = targE.gameObject;

        float x = Random.Range(2, 5);
        float y = Random.Range(2, 5);
        float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
        float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1

        if (sumPla != null)
        {
            Vector2 posi = (Vector2)targEn.transform.position + new Vector2(x * signx, y * signy);
            GameObject impling = Instantiate(myImp, posi, Quaternion.identity);

            impling.GetComponent<NetworkObject>().Spawn();
            impling.GetComponent<MinionPetAI>().isInFight = true;
            impling.GetComponent<HasLifetime>().maxLifetime = impDuration;
            impling.GetComponent<MeleeEnemyAttackTest>().baseAttackDamage = impDamage;
            impling.GetComponent<MinionPetAI>().myMaster = sumPla.transform;

            sumPla.GetComponent<PlayerStats>().myMinions.Add(impling);

            SummonImpClientRpc(summoningPlayer, impling.GetComponent<NetworkObject>());
        }
    }

    [ClientRpc]
    private void SummonImpClientRpc(NetworkObjectReference summoningPlayer, NetworkObjectReference impling, ClientRpcParams clientRpcParams = default)
    {
        summoningPlayer.TryGet(out NetworkObject sour);
        GameObject sumPla = sour.gameObject;

        impling.TryGet(out NetworkObject impl);
        GameObject impli = impl.gameObject;

        impli.GetComponent<MinionPetAI>().myMaster = sumPla.transform;
        impli.GetComponent<MinionPetAI>().isInFight = true;
    }
}
