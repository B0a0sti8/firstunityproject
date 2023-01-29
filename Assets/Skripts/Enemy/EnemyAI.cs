using System.Collections;
using UnityEngine;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Idle,
        Chasing,
        Casting,
        Attacking,
        Dying
    }

    private State state;

    public bool hasAttackSkript;

    //public bool hasTarget = false;

    public GameObject[] potentialTargets;
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

    [SerializeField]
    public Dictionary<GameObject, int> aggroTable = new Dictionary<GameObject, int>();
    public Dictionary<GameObject, int> aggroTableNew = new Dictionary<GameObject, int>();

    void HandleTargets()
    {
        GameObject[] pT1 = GameObject.FindGameObjectsWithTag("Player");  // Sucht alle Spieler in der Scene
        GameObject[] pT2 = GameObject.FindGameObjectsWithTag("Ally");    // Sucht alle Pets/Helfer in der Scene
        potentialTargets = pT1.Concat(pT2).ToArray();                    // Schmeißt alle Ziele in ein Array
        foreach (GameObject potTar in potentialTargets)
        { aggroTableNew.Add(potTar, 0); }                                // Fügt alle aktuellen Ziele in ein Dictionary, mit Aggrowert 0

        aggroTableNew.Keys.Except(aggroTable.Keys).ToList().ForEach(k => aggroTable.Add(k, 0));        // Vergleicht alle aktuellen Targets mit den bisherigen und fügt neue mit Aggro=0 hinzu
        aggroTable.Keys.Except(aggroTableNew.Keys).ToList().ForEach(k => aggroTable.Remove(k));        // Vergleicht alle aktuellen Targets mit den bisherigen und entfernt alle die nicht mehr da sind
    }


    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();

        state = State.Idle;
        potentialTargets = GameObject.FindGameObjectsWithTag("Player"); // Sucht alle Spieler in der Scene
        foreach (GameObject potTar in potentialTargets)
        {
            aggroTable.Add(potTar, 0);
        }

        targetTemp = target;
    }

    private void Idle() 
    {
        // Gegner könnte herumwandern o.ä. sucht währenddessen nach Zielen
        // Neues System: Beim Laden der Scene wird ein Dictionary erstellt, in dem alle Spieler einem Aggro-Wert zugeordnet werden.
        // Wenn Skills verwendet werden (Heilung in bestimmten Radius) Dmg Spells von irgendwo oder ein Spieler zu nah kommt -> Aggrowert erhöht
        // Sobald der Aggrowert > 0 ist, greift Gegner an.
        SearchTargets();
    }

    private void Chasing() 
    {
        // Gegner fängt an Ziele zu jagen. Könnte andere Gegner in der Nähe 'warnen'
        ChaseTarget();
    }

    private void Casting() 
    { }

    private void Attacking() 
    { }

    private void Dying() 
    { }



    void Update()
    {
        switch (state)
        {
            default:
            case State.Idle:
                Idle();
                break;

            case State.Chasing:
                Chasing();
                break;

            case State.Casting:
                Casting();
                break;

            case State.Attacking:
                Attacking();
                break;

            case State.Dying:
                Dying();
                break;
        }






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
                //GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                //foreach (GameObject allE in allEnemies)
                //{
                //    if (gameObject.GetComponent<EnemyStats>().groupNumber == allE.GetComponent<EnemyStats>().groupNumber)
                //    {
                //        allE.GetComponent<EnemyAI>().target = target;
                //    }
                //}

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
        if (target != null)
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
        { return; }

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

        Move();        
    }

    void Move()
    {
        try
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWypoint] - rb2d.position).normalized;
            Vector2 force = direction * speed * GetComponent<EnemyStats>().movementSpeed.GetValue() * Time.deltaTime;
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
        if (potentialTargets.Length == 0) { return; }
            
        viableTargets = new GameObject[potentialTargets.Length];       
        targetDistances = new float[potentialTargets.Length];

        for (int i = 0; i < potentialTargets.Length; i++)
        { targetDistances[i] = Mathf.Infinity; }

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
    }
    #endregion

    #region Momentan nicht verwendet

    void OnDrawGizmosSelected()
    {    }
    #endregion
}