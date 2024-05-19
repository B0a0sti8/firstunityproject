using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class SummonFireDemon : SkillPrefab
{
    float impactDamage;
    public GameObject myFireImpactAnim;

    public override void Start()
    {
        impactDamage = 1200;
        myAreaType = AreaType.CirclePlacable;
        hasGlobalCooldown = true;
        skillRadius = 6f;
        targetsEnemiesOnly = true;
        castTimeOriginal = 1.5f;
        
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        foreach (GameObject target in currentTargets) Debug.Log("Target: " + target);
        DealDamage(impactDamage);
        Vector3 myPosition = circleAim;
        myPosition.z = 0.2f;
        FireImpactServerRpc(myPosition);
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
    }
}