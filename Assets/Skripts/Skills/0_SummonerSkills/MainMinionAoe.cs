using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMinionAoe : SkillPrefab
{
    [SerializeField] GameObject dragonAOEAnimation;
    float dragonDamagePerTick;

    public override void Start()
    {
        base.Start();
        myClass = "Summoner";

        isSelfCast = true;

        skillRadius = 5f;

        hasOwnCooldown = true;
        hasGlobalCooldown = false;
        ownCooldownTimeBase = 10f;
        dragonDamagePerTick = 10f;
    }

    public override void Update()
    {
        tooltipSkillDescription = "Each main deals Area Damage";

        base.Update();
    }

    public override void ConditionCheck()
    {
        //if (transform.GetComponent<SummonerClass>().myMainSummonerMinions.Count >= transform.GetComponent<SummonerClass>().maxNrOfMainSummonerMinions)
        //{ Debug.Log("Zu viele Summoner Begleiter!"); return; }

        if (playerStats.myMainMinions.Count == 0)
        { Debug.Log("Kein Begleiter!"); return; }
        base.ConditionCheck();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();

        foreach (var mn in PLAYER.GetComponent<PlayerStats>().myMainMinions)
        {
            MinionPetAI minion = mn.GetComponent<MinionPetAI>();

            switch (minion.name)
            {
                case "Dragonling(Clone)":
                    // Drache soll Flächenschaden machen
                    Debug.Log("Dragonling");
                    GameObject effect = Instantiate(dragonAOEAnimation, minion.transform);
                    effect.transform.localEulerAngles = new Vector3(90, 0, 0);
                    effect.transform.localPosition = new Vector3(0, 0, -2);
                    StartCoroutine(DragonDamage(minion.transform, effect));
                    break;
                case "Stonegolem(Clone)":
                    // Golem soll Flächenschaden machen
                    Debug.Log("Stonegolem");
                    break;
                case "Waterspirit(Clone)":
                    // Wassergeist soll heilen
                    Debug.Log("Waterspirit");
                    break;
                default:
                    Debug.Log("Irgendwas anderes z.B. Jaegerpet. Macht normalen Flächenschaden machen");
                    break;
            }


            //minion.ForceAggroToTarget(PLAYER.GetComponent<InteractionCharacter>().focus.transform);
        }
    }

    IEnumerator DragonDamage(Transform dragon, GameObject effect)
    {
        for (int i = 0; i < 10; i++)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(dragon.position, skillRadius, (1 << LayerMask.NameToLayer("Enemy")));
            foreach (Collider2D coll in hit)
            {
                if(coll.GetComponent<EnemyStats>() != null) 
                { 
                    DamageOrHealing.DealDamage(dragon.GetComponent<NetworkBehaviour>(), coll.gameObject.GetComponent<NetworkBehaviour>(), dragonDamagePerTick);
                }
            }
            Debug.Log("DragonFIA!");
            yield return new WaitForSeconds(0.5f);
        }
        GameObject.Destroy(effect);
        effect.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    void SpawnSkillEffectServerRpc() 
    {

    }

    [ServerRpc]
    void DespawnSkillEffectServerRpc()
    {

    }
}
