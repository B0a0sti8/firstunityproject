using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class MinionPetAI : MonoBehaviour
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

    public Transform myMaster;
    public bool isAggroForced;
    public Transform forcedTarget;
    public bool isInFight;

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

    MinionPetMovement eMove;
    public EnemyAttack ownEnemyAttack;
    public Transform target = null;
    Transform prevTarget = null;

    [SerializeField]
    public Dictionary<GameObject, int> aggroTable = new Dictionary<GameObject, int>();

    private void Awake()
    {
        mySkills = transform.Find("Skills").GetComponents<EnemySkillPrefab>();
        eMove = GetComponent<MinionPetMovement>();
    }

    void Start()
    {
        state = State.Idle;
    }

    public void ForceAggroToTarget(Transform forcTar)
    {
        forcedTarget = forcTar;
        target = forcTar;
        isAggroForced = true;
    }

    public void DisableForcedAggro()
    {
        forcedTarget = null;
        isAggroForced = false;
    }

    void GetCurrentTarget()
    {
        prevTarget = target;

        if (isAggroForced)
        {
            target = forcedTarget;
        }

        else if (myMaster.GetComponent<InteractionCharacter>().focus != null)
        {
            if (myMaster.GetComponent<InteractionCharacter>().focus.gameObject.CompareTag("Enemy"))
            {
                target = myMaster.GetComponent<InteractionCharacter>().focus.transform;
            }
        }

        if (state == State.Idle && prevTarget != target)
        {
            eMove.StopChasing(0f);
            state = State.Chasing;
        }
    }

    public void GetRandomTargetNearby(int randomSeed)
    {
        GameObject[] pT1 = GameObject.FindGameObjectsWithTag("Enemy");  // Sucht alle Spieler in der Scene
        List<GameObject> potentialTargets = pT1.ToList();                     // Schmeißt alle Ziele in ein Array

        for (int i = potentialTargets.Count - 1; i > 0 ; i--)
        {
            Random.InitState(randomSeed);
            int j = Random.Range(0, i + 1);
            GameObject temp = potentialTargets[i];
            potentialTargets[i] = potentialTargets[j];
            potentialTargets[j] = temp;
        }

        foreach (GameObject pT in potentialTargets)     // Fügt alle aktuellen Ziele in ein Dictionary, mit Aggrowert 0
        {
            if (pT.GetComponent<CharacterStats>().isAlive.Value && (pT.transform.position - transform.position).magnitude < 6)
            {
                target = pT.transform;
                forcedTarget = pT.transform;
                Chasing();
                isAggroForced = true;
                break;
            }
            Idle();
        }
    }

    void Idle()
    {
        target = myMaster;
        eMove.FollowMaster();

        if (isInFight)
        {
            GetCurrentTarget();
        }
    }

    private void Chasing()              // Gegner jagt aktuell Ziele
    {
        if (!isInFight)
        {
            eMove.StopChasing(0f);
            target = myMaster;
            state = State.Idle;
            return;
        }

        GetCurrentTarget();

        float distanceToTarget = Vector2.Distance(transform.position, target.position); // enemy <-> enemy's target (player)

        if (distanceToTarget > attackRange && TargetInSight(target) &&                 // Wenn Ziel am Leben, in Aggro-Range,        distanceToTarget < aggroRange wird für minions nicht gebraucht
            target.GetComponent<CharacterStats>().isAlive.Value == true)                        // kein anderes Ziel wichtiger und Ziel sichtbar
        {
            eMove.ChaseTarget();
            if (isLC == true) { isLC = false; StopCoroutine(lc); }
        }
        else if (target.GetComponent<CharacterStats>().isAlive.Value == false)         // Falls Charakter tot ist
        {
            isAggroForced = false;
            eMove.StopChasing(0f);
            state = State.Idle;
            if (isLC == true) { isLC = false; StopCoroutine(lc); }
        }
        else if (distanceToTarget < attackRange)                                 // Falls das Ziel in Angriffs-Reichweite ist
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
            if (CheckIfInRange(attackRange) && TargetInSight(target) && target.GetComponent<CharacterStats>().isAlive.Value)            // Schaut ob Kriterien für Angriff erfüllt sind. Wenn nicht: Jagen
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
        ownEnemyAttack.StartEnemyAtk(target.gameObject);
        state = State.Chasing;
    }

    public void Dying()
    {
        state = State.Dying;
        myMaster.GetComponent<PlayerStats>().MinionHasDied(gameObject);
    }

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
        //LoseAggro(linTar.gameObject);
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

    bool TargetInSight(Transform target)
    {
        bool inSight = false;
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        Vector2 endPosition = (Vector2)transform.position + direction * aggroRange;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, endPosition, (1 <<
            LayerMask.NameToLayer("Borders")) | (1 << LayerMask.NameToLayer("Enemy")));

        if (hit.collider != null)
        { inSight = hit.collider.gameObject.CompareTag("Enemy") ? true : false; }

        return inSight;
    }
}
