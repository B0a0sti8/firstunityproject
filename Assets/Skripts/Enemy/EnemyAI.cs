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

    public EnemyAttack ownEnemyAttack;

    //public Transform playerTransform;

    public Transform target;
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
    }

    void Update()
    {

        SearchTargets(); // Sucht nach vernünftigem Target: 1. Sucht alle Spieler, 2. Prüft Entfernung und Insight, 3. Sucht nächstgelegenen Spieler

        if (hasTarget)
        {
            float distaceToTarget = Vector2.Distance(transform.position, target.position); // enemy <-> enemy's target (player)
                                                                                           //float distaceToTarget = Vector2.Distance(transform.position, playerTransform.position); // enemy <-> enemy's target (player)
            if (distaceToTarget < agroRange && TargetInSight(target))
            {
                //target = playerTransform;

                if (runningCeasePathfindingCoroutine) // ???
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
                ceasePathfindingCoroutine = StartCoroutine(CeasePathfinding());
                runningCeasePathfindingCoroutine = true;
            }
            FollowPath();
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);

            //if (target != null)
            //{
            //    seeker.StartPath(rb2d.position, target.position, OnPathComplete);
            //}
            //else
            //{
            //    Debug.Log("UpdatePath() but target == null");
            //}
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

        //if (target != null)
        //{
        //    if ((unitRange >= Vector2.Distance(rb2d.position, target.position) && TargetInSight())
        //    || (currentWypoint >= path.vectorPath.Count && !TargetInSight()))
        //    {
        //        if (hasAttackSkript) // auto-attack target
        //        {
        //            if (target != null)
        //            {
        //                ownEnemyAttack.StartEnemyAtk(target.gameObject);
        //            }
        //        }
        //        return;
        //    }
        //}

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

        //Vector2 direction = ((Vector2)path.vectorPath[currentWypoint] - rb2d.position).normalized;
        //Vector2 force = direction * speed * Time.deltaTime;
        //rb2d.AddForce(force);

        //float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWypoint]);
        //if (distance < nextWypointDistance)
        //{
        //    currentWypoint++;
        //}
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

    IEnumerator CeasePathfinding()
    {
        yield return new WaitForSeconds(agroTime);
        if (runningUpdatePathCoroutine)
        {
            StopCoroutine(updatePathCoroutine);
            runningUpdatePathCoroutine = false;

            //target = null;
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

    private void SearchTargets()
    {
        potentialTargets = GameObject.FindGameObjectsWithTag("Player");

        float targetDist;

        int n = 0;
        foreach (GameObject gObj in potentialTargets)
        {
            targetDist = Vector2.Distance(transform.position, gObj.transform.position);
            if (targetDist <= agroRange && TargetInSight(gObj.transform))
            {
                viableTargets[n] = gObj;
                n += 1;
            }
        }

        int m = 0;
        foreach (GameObject gObj in viableTargets)
        {
            targetDistances[m] = Vector2.Distance(transform.position, gObj.transform.position);
            m += 1;
        }

        int minIndex = Array.IndexOf(targetDistances, Mathf.Min(targetDistances));
        target = viableTargets[minIndex].transform;
        if(target != null)
        {
            hasTarget = true;
        }
        else
        {
            hasTarget = false;
        }
    }
}