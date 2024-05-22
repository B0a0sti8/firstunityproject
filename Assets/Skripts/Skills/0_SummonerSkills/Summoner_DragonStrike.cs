using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Summoner_DragonStrike : SkillPrefab
{
    SummonerClass mySummonerClass;

    float impactDamageBase;
    public GameObject myFireImpactAnim;

    public override void Start()
    {
        mySummonerClass = PLAYER.transform.Find("SkillManager").Find("Summoner").GetComponent<SummonerClass>();
        animationTime = 5f;
        base.Start();
        myClass = "Summoner";
        tooltipSkillDescription = "Ultimate Spell: Summons a true Dragon, just for a moment, dealing massive amounts of damage around target enemy. ";
        isUltimateSpell = true;
        ultimateSpellName = "Dragon Strike";

        skillRadius = 4f;
        targetsEnemiesOnly = true;
        needsTargetEnemy = true;
        myAreaType = AreaType.CircleAroundTarget;
        skillRange = 20;
        canSelfCastIfNoTarget = false;
        impactDamageBase = 1000;
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        float impactDamage = impactDamageBase * myUltimateSpellHelpers.Count * playerStats.dmgInc.GetValue();

        DealDamage(impactDamage);
        Vector3 myPosition = targetSnapShot.transform.position;
        myPosition.z = 0.2f;
        FireImpactServerRpc(myPosition);

        StartCoroutine(AdditionalExplosion(myPosition + new Vector3(0, skillRange * 0.3f, 0), 0.15f, impactDamage));
        StartCoroutine(AdditionalExplosion(myPosition + new Vector3(0, -skillRange * 0.3f, 0), 0.3f, impactDamage));
        StartCoroutine(AdditionalExplosion(myPosition + new Vector3(skillRange * 0.3f, skillRange * 0.2f, 0), 0.45f, impactDamage));
        StartCoroutine(AdditionalExplosion(myPosition + new Vector3(-skillRange * -0.3f, skillRange * 0.2f, 0), 0.6f, impactDamage));
        StartCoroutine(AdditionalExplosion(myPosition + new Vector3(-skillRange * 0.3f, skillRange * 0.2f, 0), 0.75f, impactDamage));
        StartCoroutine(AdditionalExplosion(myPosition + new Vector3(skillRange * 0.3f, -skillRange * 0.2f, 0), 0.9f, impactDamage));

    }

    IEnumerator AdditionalExplosion(Vector3 centerPoint, float time, float myDamage)
    {
        yield return new WaitForSeconds(time);
        currentTargets.Clear();
        List<GameObject> myNewTargets = GetTargetsInCircleHelper(centerPoint, skillRange);
        foreach (GameObject preTa in myNewTargets)
        { if (preTa.layer == LayerMask.NameToLayer("Enemy")) currentTargets.Add(preTa); }
        DealDamage(myDamage);
        FireImpactServerRpc(centerPoint);
    }


    [ServerRpc]
    public void FireImpactServerRpc(Vector3 myPosition)
    {
        FireImpactClientRpc(myPosition);
    }

    [ClientRpc]
    public void FireImpactClientRpc(Vector3 myPosition)
    {
        GameObject myFireImpact = Instantiate(myFireImpactAnim, myPosition, Quaternion.identity);
        myFireImpact.transform.localScale *= 3;
        foreach (ParticleSystem ps in myFireImpact.transform.GetComponentsInChildren<ParticleSystem>())
        {
            var myMain = ps.main;
            myMain.simulationSpeed = 0.5f;
        }
        StartCoroutine(DestroyExplosionPrefab(myFireImpact));
    }

    IEnumerator DestroyExplosionPrefab(GameObject myExplosion)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(myExplosion);
    }
}

