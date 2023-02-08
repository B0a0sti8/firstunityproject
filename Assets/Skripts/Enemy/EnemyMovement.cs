using Pathfinding;
using System;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10000f;
    float nextWypointDistance = 1f;
    int currentWypoint = 0;

    bool isStoppingPF = false;
    Coroutine ceasePathfindingCoroutine;

    bool isInvokingPF = false;
    Coroutine updatePathCoroutine;

    Path path;
    Seeker seeker;
    Rigidbody2D rb2d;
    EnemyAI eAI;
    Transform target;

    private void Start()
    {
        eAI = GetComponent<EnemyAI>();
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();

        speed = 20000;
        target = eAI.target;
    }

    public void StopChasing(float linTime)
    {
        if (!isStoppingPF)
        {
            ceasePathfindingCoroutine = StartCoroutine(CeasePathfinding(linTime));
            isStoppingPF = true;
        }
    }

    public void ChaseTarget()
    {
        if (eAI.target == null) return;
        target = eAI.target;

        if (isStoppingPF)                                   // Falls 
        {
            StopCoroutine(ceasePathfindingCoroutine);
            isStoppingPF = false;
        }

        if (!isInvokingPF)
        {
            updatePathCoroutine = StartCoroutine(InvokePathfinding());
            isInvokingPF = true;
        }
        Move();         
    }

    IEnumerator CeasePathfinding(float aggroTime)
    {
        yield return new WaitForSeconds(aggroTime);
        if (isInvokingPF)
        {
            StopCoroutine(updatePathCoroutine);
            isInvokingPF = false;
        }
    }

    IEnumerator InvokePathfinding()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (seeker.IsDone())
            { seeker.StartPath(rb2d.position, target.position, OnPathComplete); }
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            this.path = p;
            currentWypoint = 0;
        }
    }

    void Move()
    {
        if (path == null || (currentWypoint >= path.vectorPath.Count))
        { return; }

        Vector2 direction = ((Vector2)path.vectorPath[currentWypoint] - rb2d.position).normalized;
        //transform.localRotation *= Quaternion.FromToRotation(-transform.up, direction);
        Vector2 force = direction * speed * GetComponent<EnemyStats>().movementSpeed.GetValue() * Time.deltaTime;
        rb2d.AddForce(force);

        float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWypoint]);
        if (distance < nextWypointDistance)
        { currentWypoint++; }
    }
}