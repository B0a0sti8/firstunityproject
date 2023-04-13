using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DarkBolt : SkillPrefab
{
    public float damage = 400f;
    [SerializeField] GameObject myProjectile;

    public override void Start()
    {
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

        Debug.Log("Firing DarkBolt");
        float tOA = 0.2f;

        GameObject project = Instantiate(myProjectile, PLAYER.transform.position, Quaternion.identity);
        project.GetComponent<NetworkObject>().Spawn();
        project.GetComponent<ProjectileFlyToTarget>().target = PLAYER.GetComponent<InteractionCharacter>().focus.transform;
        project.GetComponent<ProjectileFlyToTarget>().timeToArrive = tOA;
        StartCoroutine(DelayedDamage(tOA));
    }

    IEnumerator DelayedDamage(float tOA)
    {
        yield return new WaitForSeconds(tOA);
        DealDamage(damage);
    }
}
