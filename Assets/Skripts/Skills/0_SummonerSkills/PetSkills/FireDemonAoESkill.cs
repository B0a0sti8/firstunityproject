using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FireDemonAoESkill : EnemySkillPrefab
{
    Vector3 tarPos;
    List<GameObject> myTargets = new List<GameObject>();
    public GameObject myFireImpactAnim;

    private void Start()
    {
        cooldown = 7f;
        range = 0f;
        radius = 6f;
        baseDamage = 200;
        skillReady = false;
        remainingCD = 5f;
    }

    public override void SkillEffect()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.parent.position, radius, (1 << LayerMask.NameToLayer("Enemy")));

        foreach (Collider2D coll in hit)
        { myTargets.Add(coll.gameObject); }

        foreach (GameObject tar in myTargets)
        {
            DamageOrHealing.DealDamage(transform.parent.gameObject.GetComponent<NetworkBehaviour>(), tar.GetComponent<NetworkBehaviour>(), baseDamage);
        }
        myTargets.Clear();

        FireImpactServerRpc(transform.parent.position);

        base.SkillEffect();
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
