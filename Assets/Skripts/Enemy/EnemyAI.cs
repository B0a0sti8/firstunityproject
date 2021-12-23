using System.Collections;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyAI : MonoBehaviour
{
    public bool hasAttackSkript;

    public bool hasTarget = false;
    GameObject[] potentialTargets;
    GameObject[] viableTargets;
    float[] targetDistances;
    //bool setTargetToNull = false;

    public EnemyAttack ownEnemyAttack;

    //public Transform playerTransform;

    public Transform target = null;
    public Transform targetTemp = null;


    public float speed = 200f;
    public float nextWypointDistance;
    public float unitRange = 6f;
    public float agroRange = 12f;
    public float agroTime = 2.0f;

    public int currentWypoint = 0;
    public Path path;
    public Seeker seeker;
    public Rigidbody2D rb2d;

    public bool runningCeasePathfindingCoroutine = false;
    public Coroutine ceasePathfindingCoroutine;

    public bool runningUpdatePathCoroutine = false;
    public Coroutine updatePathCoroutine;


    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();

        targetTemp = target;
    }

    void Update()
    {
        SearchTargets(); // Sucht nach vernünftigem Target: 1. Sucht alle Spieler, 2. Prüft Entfernung und InSight, 3. Sucht nächstgelegenen Spieler

        ChaseTarget();

        if (targetTemp != target) // whenever target changes
        {
            targetTemp = target;

            if (target == null) // when loosing target
            {

            }
            else  // when gaining target
            {
                // share target with every group member
                GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject allE in allEnemies)
                {
                    if (gameObject.GetComponent<EnemyStats>().groupNumber == allE.GetComponent<EnemyStats>().groupNumber)
                    {
                        allE.GetComponent<EnemyAI>().target = target;
                    }
                }

                gameObject.transform.Find("Canvas World Space").transform.Find("AggroText").gameObject.SetActive(true);
                StartCoroutine(Wait(1f));
                IEnumerator Wait(float time)
                {
                    yield return new WaitForSeconds(time);
                    gameObject.transform.Find("Canvas World Space").transform.Find("AggroText").gameObject.SetActive(false);
                }
            }
        }
    }

    #region Verfolge Ziele
    private void ChaseTarget()
    {
        if (hasTarget)
        {
            float distaceToTarget = Vector2.Distance(transform.position, target.position); // enemy <-> enemy's target (player)
            //float distaceToTarget = Vector2.Distance(transform.position, playerTransform.position); // enemy <-> enemy's target (player)
            if (distaceToTarget < agroRange && TargetInSight(target) && target.GetComponent<PlayerStats>().isAlive == true)
            {
                //target = playerTransform;

                if (runningCeasePathfindingCoroutine)
                {
                    StopCoroutine(ceasePathfindingCoroutine);
                    runningCeasePathfindingCoroutine = false;
                    //Debug.Log("ERROR 1 lolsos");
                }

                if (!runningUpdatePathCoroutine)
                {
                    updatePathCoroutine = StartCoroutine(InvokePathfinding());
                    runningUpdatePathCoroutine = true;
                    //Debug.Log("ERROR 2 lol? sooos");
                }
            }
            else if (!runningCeasePathfindingCoroutine)
            {
                if (target.GetComponent<PlayerStats>().isAlive == false)
                {
                    ceasePathfindingCoroutine = StartCoroutine(CeasePathfinding(0f));
                }
                else
                {
                    ceasePathfindingCoroutine = StartCoroutine(CeasePathfinding(agroTime));
                }
                runningCeasePathfindingCoroutine = true;
            }

            if (target.GetComponent<PlayerStats>().isAlive == false) // target is dead
            {
                target = null;
            }
            else // target is alive
            {
                FollowPath();
            }
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            currentWypoint = 0;
        }
    }

    void FollowPath()
    {
        if (path == null)
        {
            return;
        }

        if ((unitRange >= Vector2.Distance(rb2d.position, target.position) && TargetInSight(target))
            || (currentWypoint >= path.vectorPath.Count && !TargetInSight(target)))
        {
            if (hasAttackSkript) // auto-attack target
            {
                if (target != null)
                {
                    ownEnemyAttack.StartEnemyAtk(target.gameObject);
                }
            }
            return;
        }

        try
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWypoint] - rb2d.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;
            rb2d.AddForce(force);

            float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWypoint]);
            if (distance < nextWypointDistance)
            {
                currentWypoint++;
            }
        }
        catch (Exception e) { }
    }

    bool TargetInSight(Transform target)
    {
        bool inSight = false;
        Vector2 direction = ((Vector2)target.position - rb2d.position).normalized;
        Vector2 endPosition = rb2d.position + direction * agroRange;

        RaycastHit2D hit = Physics2D.Linecast(rb2d.position, endPosition, (1 <<
            LayerMask.NameToLayer("Borders")) | (1 << LayerMask.NameToLayer("Action")));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                inSight = true;
            }
            else
            {
                inSight = false;
            }
        }

        return inSight;
    }

    IEnumerator CeasePathfinding(float aggroTime)
    {
        yield return new WaitForSeconds(aggroTime);
        if (runningUpdatePathCoroutine)
        {
            StopCoroutine(updatePathCoroutine);
            runningUpdatePathCoroutine = false;
            Debug.Log("UpdatePath stopped. target -> null");
            target = null;
            //setTargetToNull = false;
        }
    }

    IEnumerator InvokePathfinding()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            UpdatePath();
        }
    }
    #endregion

    #region Suche nach Zielen
    private void SearchTargets()
    {
        potentialTargets = GameObject.FindGameObjectsWithTag("Player");
        if (potentialTargets.Length == 0) { return; }
            
        viableTargets = new GameObject[potentialTargets.Length];

        targetDistances = new float[potentialTargets.Length];
        for (int i = 0; i < potentialTargets.Length; i++)
        {
            targetDistances[i] = Mathf.Infinity;
        }

        int n = 0;
        foreach (GameObject potTar in potentialTargets)
        {
            float targetDist = Vector2.Distance(gameObject.transform.position, potTar.transform.position);
            if (targetDist <= agroRange && TargetInSight(potTar.transform) && potTar.GetComponent<PlayerStats>().isAlive == true)
            {
                viableTargets[n] = potTar;
                targetDistances[n] = targetDist;
                n += 1;
            }
        }

        if (viableTargets[0] != null)
        {
            int minIndex = Array.IndexOf(targetDistances, Mathf.Min(targetDistances));
            target = viableTargets[minIndex].transform;
        }
        //else
        //{
        //    setTargetToNull = true;
        //    //target = null;
        //}

        if (target != null)
        {
            hasTarget = true;
        }
        else
        {
            hasTarget = false;
        }
    }
    #endregion

    #region Momentan nicht verwendet
    //bool PlayerInSight()
    //{
    //    bool inSight = false;
    //    Vector2 direction = ((Vector2)playerTransform.position - rb2d.position).normalized;
    //    Vector2 endPosition = rb2d.position + direction * agroRange;

    //    RaycastHit2D hit = Physics2D.Linecast(rb2d.position, endPosition, (1 <<
    //        LayerMask.NameToLayer("Borders")) | (1 << LayerMask.NameToLayer("Action")));
    //    if (hit.collider != null)
    //    {
    //        if (hit.collider.gameObject.CompareTag("Player"))
    //        {
    //            inSight = true;
    //        }
    //        else
    //        {
    //            inSight = false;
    //        }
    //    }

    //    return inSight;
    //}


    void OnDrawGizmosSelected()
    {
        /* Vector2 direction = ((Vector2)target.position - rb2d.position).normalized * agroRange;
         Gizmos.DrawRay(rb2d.position, direction);

         Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(rb2d.position, unitRange);

         Gizmos.color = Color.green;
         Gizmos.DrawWireSphere(rb2d.position, agroRange); */
    }
    #endregion
}