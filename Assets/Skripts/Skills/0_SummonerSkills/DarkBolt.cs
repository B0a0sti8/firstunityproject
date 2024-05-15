using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DarkBolt : SkillPrefab
{
    public float damage = 400f;
    [SerializeField] GameObject myProjectile;
    SummonerClass mySummonerClass;

    public override void Start()
    { 
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
        castTimeOriginal = 1.5f;
        ownCooldownTimeBase = 3f;
        needsTargetEnemy = true;
        skillRange = 20;
        hasGlobalCooldown = true;

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

        float tOA = 0.2f;

        GameObject targetRefe = currentTargets[0];
        SpawnDarkBoltServerRpc(PLAYER.GetComponent<NetworkObject>(), tOA, targetRefe.GetComponent<NetworkObject>());

        if (mySummonerClass.doesDarkboltIncreaseMinionLifetime)
        {
            mySummonerClass.SummonerClass_IncreaseLivingMinionDuration(mySummonerClass.darkBoltLifeTimeIncrease);
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
        DealDamage(damage * mySummonerClass.darkBoltDamageModifier);
    }
}
