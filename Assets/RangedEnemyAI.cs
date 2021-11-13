using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RangedEnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWypointDistance;
    public float unitRange = 6f;
    public float agroRange = 12f;
    public float agroTime = 3.0f;

    private int currentWypoint = 0;
    private bool targetReached = false;
    private Path path;
    private Seeker seeker;
    private Rigidbody2D rb2d;

    private bool runningCeasePathfindingCoroutine = false;
    private Coroutine ceasePathfindingCoroutine;

    private bool runningUpdatePathCoroutine = false;
    private Coroutine updatePathCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float distaceToTarget = Vector2.Distance(transform.position, target.position);
        if (distaceToTarget < agroRange && TargetInSight())
        {
            StopCoroutine(ceasePathfindingCoroutine);
            runningCeasePathfindingCoroutine = false;

            if (!runningUpdatePathCoroutine)
            {
                updatePathCoroutine = StartCoroutine(InvokePathfinding());
                runningUpdatePathCoroutine = true;
            }
        }
        else if (!runningCeasePathfindingCoroutine)
        {
            ceasePathfindingCoroutine = StartCoroutine(CeasePathfinding());
            runningCeasePathfindingCoroutine = true;
        }
        FollowPath();
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
        if ((currentWypoint + unitRange >= path.vectorPath.Count && TargetInSight())
            || (currentWypoint >= path.vectorPath.Count && !TargetInSight()))
        {
            targetReached = true;
            return;
        } else
        {
            targetReached = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWypoint] - rb2d.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb2d.AddForce(force);

        float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWypoint]);
        if (distance < nextWypointDistance)
        {
            currentWypoint++;
        }
    }

    bool TargetInSight()
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

    IEnumerator CeasePathfinding()
    {
        yield return new WaitForSeconds(agroTime);
        if (runningUpdatePathCoroutine)
        {
            StopCoroutine(updatePathCoroutine);
            runningUpdatePathCoroutine = false;
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
}
