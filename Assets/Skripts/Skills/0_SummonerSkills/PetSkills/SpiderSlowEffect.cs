using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpiderSlowEffect : EnemySkillPrefab
{
    List<GameObject> myTargets = new List<GameObject>();
    public GameObject myFireImpactAnim;
    public float slowValue;

    private void Awake()
    {
        cooldown = 1f;
        range = 0f;
        radius = 10f;
        skillReady = false;
        remainingCD = 1f;
        slowValue = 0;
    }

    public override void SkillEffect()
    {
        //Debug.Log("Ich caste brav" + transform.parent);
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.parent.position, radius, (1 << LayerMask.NameToLayer("Enemy")));

        foreach (Collider2D coll in hit)
        { myTargets.Add(coll.gameObject); }

        foreach (GameObject tar in myTargets)
        {
            tar.GetComponent<BuffManagerNPC>().RemoveBuffProcedure(transform.parent.GetComponent<NetworkObject>(), "SummonerSpiderSlowEffectDebuff", false);
            GiveBuffOrDebuffToTarget.GiveBuffOrDebuff(tar.GetComponent<NetworkObject>(), transform.parent.GetComponent<NetworkObject>(), "SummonerSpiderSlowEffectDebuff", "SummonerSpiderSlowEffectDebuff", false, 5f, 0, slowValue);
        }
        myTargets.Clear();

        //SpiderWebServerRpc(transform.parent.position);

        base.SkillEffect();
    }

    [ServerRpc]
    public void SpiderWebServerRpc(Vector3 myPosition)
    {
        //SpiderWebClientRpc(myPosition);
    }

    [ClientRpc]
    public void SpiderWebClientRpc(Vector3 myPosition)
    {
        //GameObject myFireImpact = Instantiate(myFireImpactAnim, myPosition, Quaternion.identity);
        //myFireImpact.transform.localScale *= 3;
        //foreach (ParticleSystem ps in myFireImpact.transform.GetComponentsInChildren<ParticleSystem>())
        //{
        //    var myMain = ps.main;
        //    myMain.simulationSpeed = 0.5f;
        //}
    }
}
