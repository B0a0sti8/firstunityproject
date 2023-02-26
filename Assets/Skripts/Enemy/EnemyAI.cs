using System.Collections;
using UnityEngine;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    public enum State
    {
        Idle,
        Chasing,
        DoAction,
        Attacking,
        WaitingForCast,
        DoNothing,
        Dying
    }

    private State state;

    public float aggroRange = 12f;
    public float targetRange = 2f;
    public float attackRange = 6f;
    public float aggroTime = 2.0f;

    EnemySkillPrefab[] mySkills;
    [SerializeField] float tBS = 5;              // Zeit zwischen zwei Skills, selbst wenn der CD ready ist.
    [SerializeField] float currentTBS = 0;       // Aktuelle Zeit zwischen zwei Skills
        
    bool isLC = false;
    Coroutine lc;

    public bool isCasting = false;
    bool noSkillReady = false;
    Coroutine isInCast;

    EnemyMovement eMove;
    public EnemyAttack ownEnemyAttack;
    public Transform target = null;
    Transform prevTarget = null;

    [SerializeField]
    public Dictionary<GameObject, int> aggroTable = new Dictionary<GameObject, int>();

    void HandleTargetAggro()
    {
        GameObject[] pT1 = GameObject.FindGameObjectsWithTag("Player");  // Sucht alle Spieler in der Scene
        GameObject[] pT2 = GameObject.FindGameObjectsWithTag("Ally");    // Sucht alle Pets/Helfer in der Scene
        List<GameObject> potentialTargets = pT1.Concat(pT2).ToList();                     // Schmeißt alle Ziele in ein Array
        Dictionary<GameObject, int> aggroTableNew = new Dictionary<GameObject, int>();

        foreach (GameObject pT in potentialTargets)     // Fügt alle aktuellen Ziele in ein Dictionary, mit Aggrowert 0
        {
            if (pT.GetComponent<CharacterStats>().isAlive.Value)
            { aggroTableNew.Add(pT, 0); }
        }        

        aggroTableNew.Keys.Except(aggroTable.Keys).ToList().ForEach(k => aggroTable.Add(k, 0));        // Vergleicht alle aktuellen Targets mit den bisherigen und fügt neue mit Aggro=0 hinzu
        aggroTable.Keys.Except(aggroTableNew.Keys).ToList().ForEach(k => aggroTable.Remove(k));        // Vergleicht alle aktuellen Targets mit den bisherigen und entfernt alle die nicht mehr da sind
        aggroTableNew.Clear();
    }

    public void IncreaseAggro(GameObject source, int aggro)
    {
        if (source != null && aggroTable.ContainsKey(source))
        { aggroTable[source] += aggro; }
    }

    void LoseAggro(GameObject source)
    {
        if (source != null && aggroTable.ContainsKey(source))
        { aggroTable[source] = 0; }
    }

    void SearchTargets()
    {
        foreach (GameObject tar in aggroTable.Keys.ToList())            // Wenn ein Spieler zu nah kommt, erhält Enemy aggro
        {
            float targetDist = Vector2.Distance(transform.position, tar.transform.position);
            if (targetDist <= aggroRange && TargetInSight(tar.transform) && tar.GetComponent<CharacterStats>().isAlive.Value == true)
            { IncreaseAggro(tar, 1); }
        }
        if (aggroTable.Count != 0)
        {
            target = aggroTable.Aggregate((x, y) => x.Value > y.Value ? x : y).Key.transform;       // Findet Eintrag im AggroTable mit der höchsten Aggro und prüft ob Insight
        }
        
        if (target != null && target.GetComponent<CharacterStats>().isAlive.Value && aggroTable[target.gameObject] > 0) 
        {
            if (state == State.Idle)
            { StartChase(); }

            state = State.Chasing; 
        }
    }

    bool CheckIfInRange(float distance)
    {
        if (target != null)
        {
            float targetDist = Vector2.Distance(transform.position, target.transform.position);
            if (targetDist <= distance && TargetInSight(target.transform) && target.GetComponent<CharacterStats>().isAlive.Value == true)
            { return true; }
        }
        return false;
    }

    public bool TargetInSight(Transform target)
    {
        bool inSight = false;
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        Vector2 endPosition = (Vector2)transform.position + direction * aggroRange;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, endPosition, (1 <<
            LayerMask.NameToLayer("Borders")) | (1 << LayerMask.NameToLayer("Action")));

        if (hit.collider != null)
        { inSight = hit.collider.gameObject.CompareTag("Player")? true : false; }

        return inSight;
    }

    void StartChase()               // Gegner fängt an Ziele zu jagen. Könnte andere Gegner in der Nähe 'warnen'
    {
        gameObject.transform.Find("Canvas World Space").transform.Find("AggroText").gameObject.SetActive(true);
        StartCoroutine(Wait(1f));
        IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.transform.Find("Canvas World Space").transform.Find("AggroText").gameObject.SetActive(false);
        }
    }

    void Start()
    {
        state = State.Idle;
        eMove = GetComponent<EnemyMovement>();
        mySkills = transform.Find("Skills").GetComponents<EnemySkillPrefab>();
    }

    void Idle() 
    {
        // Gegner könnte herumwandern o.ä. sucht währenddessen nach Zielen
        // Neues System: Beim Laden der Scene wird ein Dictionary erstellt, in dem alle Spieler einem Aggro-Wert zugeordnet werden.
        // Wenn Skills verwendet werden (Heilung in bestimmten Radius) Dmg Spells von irgendwo oder ein Spieler zu nah kommt -> Aggrowert erhöht
        // Sobald der Aggrowert > 0 ist, greift Gegner an.

        // Diese beiden Funktionen müssen nicht jedes Frame ausgeführt werden. Reicht ~ jede Sekunde
        HandleTargetAggro();
        SearchTargets();
    }

    private void Chasing()              // Gegner jagt aktuell Ziele
    {
        prevTarget = target;            // Speichert aktuelles Target
        
        HandleTargetAggro();
        SearchTargets();                // Soll weiterhin schauen, welches Ziel die höchste Aggro hat

        float distanceToTarget = Vector2.Distance(transform.position, target.position); // enemy <-> enemy's target (player)

        if (distanceToTarget < aggroRange && distanceToTarget > attackRange && TargetInSight(target) &&           // Wenn Ziel am Leben, in Aggro-Range,
            target.GetComponent<CharacterStats>().isAlive.Value == true && prevTarget == target)                        // kein anderes Ziel wichtiger und Ziel sichtbar
        {
            eMove.ChaseTarget();
            if (isLC == true) { isLC = false; StopCoroutine(lc); }
        }
        else if (target != prevTarget)                                           // Falls sich Target geändert hat
        {
            eMove.StopChasing(0f);
            eMove.ChaseTarget();
            if (isLC == true) { isLC = false; StopCoroutine(lc); }
        }
        else if (target.GetComponent<CharacterStats>().isAlive.Value == false)         // Falls Charakter tot ist
        {
            LoseAggro(target.gameObject);
            eMove.StopChasing(0f);
            state = State.Idle;
            if (isLC == true) { isLC = false; StopCoroutine(lc); }
        }
        else if (distanceToTarget < attackRange)                                 // Falls das Ziel in Reichweite ist
        {
            eMove.StopChasing(0.2f);
            state = State.DoAction;
            if (isLC == true) { isLC = false; StopCoroutine(lc); }
        }
        else                                                                     // Falls Ziel nicht sichtbar oder zu weit weg, lauf noch bisschen hinterher. Dann brich ab.
        {
            if (isLC == false)
            {
                lc = StartCoroutine(LingeringChase(aggroTime));
                isLC = true;
            }
            eMove.ChaseTarget();
        }
    }

    private void DoAction() 
    {
        if (mySkills.Count() == 0 || currentTBS != 0)                // Schaut ob er Skills hat und wieder casten darf. Wenn eines von beiden nicht erfüllt ist, soll er in Attack range laufen, oder angreifen
        {
            //Debug.Log("Ich habe keine Skills, chase oder greife an.");
            if (CheckIfInRange(attackRange) & TargetInSight(target) & target.GetComponent<CharacterStats>().isAlive.Value)            // Schaut ob Kriterien für Angriff erfüllt sind. Wenn nicht: Jagen
            {
                state = State.Attacking;
                Attacking();
            }
            else
            {
                state = State.Chasing;
                Chasing();
            }
            return;
        }

        foreach (EnemySkillPrefab mS in mySkills)               // Wenn er Skills hat und casten darf: Geht durch all seine Skills
        {
            if (mS.range != 0 & CheckIfInRange(mS.range) & mS.skillReady)       // Schaut für jeden Skill, ob er eine Range hat, wenn ja ob er in Range ist und ob Skill ready ist
            {
                //Debug.Log("Ein Skill ist ready, ich caste");
                mS.CastSkill();
                currentTBS = tBS;
                noSkillReady = false;
                if (mS.duration > 0)        // Skill wurde ausgeführt, warte ggf. Skilldauer ab.
                {
                    isInCast = StartCoroutine(WaitingForCast(mS.duration));
                    state = State.WaitingForCast;
                }
                break;
            }
            else if (mS.range == 0 & mS.skillReady)
            {
                //Debug.Log("Ein Skill ist ready, ich caste");
                mS.CastSkill();
                currentTBS = tBS;
                noSkillReady = false;
                if (mS.duration > 0)        // Skill wurde ausgeführt, warte ggf. Skilldauer ab.
                {
                    isInCast = StartCoroutine(WaitingForCast(mS.duration));
                    state = State.WaitingForCast;
                }
                break;
            }
            else    // Wenn er out of range ist oder kein Skill ready: In Attack range laufen, oder angreifen
            {
                noSkillReady = true;
                //Debug.Log("Kein Skill ready oder alle out of range, ich chase oder greife an.");
                
            }
        }

        if (noSkillReady)
        {
            if (CheckIfInRange(attackRange) & TargetInSight(target) & target.GetComponent<CharacterStats>().isAlive.Value)            // Schaut ob Kriterien für Angriff erfüllt sind. Wenn nicht: Jagen
            {
                state = State.Attacking;
                Attacking();
            }
            else
            {
                state = State.Chasing;
                Chasing();
            }
        }
    }

    private void Attacking() 
    {
        //Debug.Log("Ich mach dich fertig!");
        state = State.Chasing;
    }

    private void Dying() 
    { }



    void Update()
    {
        //Debug.Log(state);
        //if (GetComponent<CharacterStats>().isAlive == false)
        //{ state = State.Dying; }
        if (target != null)
        {
            Vector2 lookDir = (Vector2)(transform.position - target.position).normalized;
            transform.localRotation = Quaternion.LookRotation(lookDir, Vector3.back);
        }


        switch (state)
        {
            default:
            case State.Idle:
                Idle();
                break;

            case State.Chasing:
                Chasing();
                break;

            case State.DoAction:
                DoAction();
                break;

            case State.Attacking:
                Attacking();
                break;

            case State.WaitingForCast:
                //Debug.Log("Ich caste gerade");
                // Wenn der Gegner bei seinem Cast unterbrochen wird, kann man es hier rein tun
                break;

            case State.DoNothing:
                break;

            case State.Dying:
                Dying();
                break;
        }

        currentTBS = (currentTBS > 0) ? currentTBS - Time.deltaTime : 0;
    }

    IEnumerator LingeringChase(float delayTime)
    {
        Transform linTar = target;
        yield return new WaitForSeconds(delayTime);
        state = State.Idle;
        isLC = false;
        LoseAggro(linTar.gameObject);
    }

    IEnumerator WaitingForCast(float castTime)
    {
        isCasting = true;
        yield return new WaitForSeconds(castTime);
        isCasting = false;

        if (CheckIfInRange(attackRange) & TargetInSight(target) & target.GetComponent<CharacterStats>().isAlive.Value)            // Schaut ob Kriterien für Angriff erfüllt sind. Wenn nicht: Jagen
        {
            state = State.Attacking;
            Attacking();
        }
        else
        {
            state = State.Chasing;
            Chasing();
        }
    }

    public void SetState(State s)
    {
        state = s;
    }

    void OnDrawGizmosSelected()
    {    }

}