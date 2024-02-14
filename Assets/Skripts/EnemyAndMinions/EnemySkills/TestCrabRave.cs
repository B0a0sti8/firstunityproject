using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TestCrabRave : EnemySkillPrefab
{
    [SerializeField] GameObject crabbling;
    Transform skillTarget;
    List<GameObject> myTargetsEnemies = new List<GameObject>();
    List<GameObject> myTargetsFriends = new List<GameObject>();
    List<GameObject> myCrabs = new List<GameObject>();

    Coroutine raveParty;

    [SerializeField] Animator crabRaveAnimator;
    // Start is called before the first frame update
    void Start()
    {
        cooldown = 10f;
        duration = 5f;
        animationDuration = 4f;
        crabRaveAnimator = transform.parent.Find("CrabBoss").GetComponent<Animator>();
        radius = 10;
        baseHealing = 10;
    }

    public override void Update()
    {
        if (startedAnimation)
        {
            crabRaveAnimator.SetFloat("RaveSpeed", 1f);
            crabRaveAnimator.SetBool("IsRaving", true);
            startedAnimation = false;
        }

        if (endedAnimation)
        {
            crabRaveAnimator.SetBool("IsRaving", false);
            endedAnimation = false;
        }
        base.Update();
    }

    public override void AtSkillStart()
    {
        Collider2D[] hit1 = Physics2D.OverlapCircleAll(transform.parent.position, radius, (1 << LayerMask.NameToLayer("Action")));

        foreach (Collider2D coll in hit1)
        { myTargetsEnemies.Add(coll.gameObject); }

        Collider2D[] hit2 = Physics2D.OverlapCircleAll(transform.parent.position, radius, (1 << LayerMask.NameToLayer("Enemy")));

        foreach (Collider2D coll in hit1)
        { myTargetsFriends.Add(coll.gameObject); }

        raveParty = StartCoroutine(TheRavePartyGoesOn(duration));

        base.AtSkillStart();
    }

    IEnumerator TheRavePartyGoesOn(float duration)
    {
        for (int i = 0; i < 50; i++)
        {
            foreach (GameObject tar in myTargetsEnemies)
            {
                DamageOrHealing.DoHealing(transform.parent.gameObject.GetComponent<NetworkBehaviour>(), tar.GetComponent<NetworkBehaviour>(), baseHealing);
                if (i == 0 || i == 5 || i == 10 || i == 12 || i == 17 || i == 19 || i == 23 || i == 28 || i == 25 || i == 50)
                {
                    float x = Random.Range(2, 5);
                    float y = Random.Range(2, 5);
                    float signx = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1
                    float signy = Random.Range(0, 2) * 2 - 1;       // Entweder -1 oder 1

                    Vector2 posi = (Vector2)tar.transform.position + new Vector2(x * signx, y * signy);
                    GameObject crab = Instantiate(crabbling, posi, Quaternion.identity);

                    crab.transform.Find("CrabBoss").GetComponent<Animator>().SetBool("IsRaving", true);
                    crab.GetComponent<EnemyAI>().SetState(EnemyAI.State.DoNothing);
                    myCrabs.Add(crab);
                }
            }

            foreach (GameObject tar in myTargetsFriends)
            {
                DamageOrHealing.DoHealing(transform.parent.gameObject.GetComponent<NetworkBehaviour>(), tar.GetComponent<NetworkBehaviour>(), baseHealing);
            }

            yield return new WaitForSeconds(duration / 50);
        }

        FadeInStopRaving(1.5f, myCrabs);
        myCrabs.Clear();
        myTargetsEnemies.Clear();
        myTargetsFriends.Clear();
    }

    IEnumerator FadeInStopRaving(float duration, List<GameObject> crabs)
    {
        yield return new WaitForSeconds(duration);
        foreach (var crab in crabs)
        {
            crab.transform.Find("CrabBoss").GetComponent<Animator>().SetBool("IsRaving", false);
            crab.GetComponent<EnemyAI>().SetState(EnemyAI.State.Idle);
        }
    }
}