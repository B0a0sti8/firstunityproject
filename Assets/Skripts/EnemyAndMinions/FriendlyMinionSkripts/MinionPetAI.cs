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

    public GameObject myMaster;
    public bool isAggroForced;
    public Transform forcedTarget;

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

    void Start()
    {
        state = State.Idle;
        eMove = GetComponent<MinionPetMovement>();
        mySkills = transform.Find("Skills").GetComponents<EnemySkillPrefab>();
    }

    void GetCurrentTarget()
    {
        if (isAggroForced)
        {
            target = forcedTarget;
        }
        else if (myMaster.GetComponent<InteractionCharacter>().focus.gameObject.CompareTag("Enemy"))
        {
            target = myMaster.GetComponent<InteractionCharacter>().focus.transform;
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
            LayerMask.NameToLayer("Borders")) | (1 << LayerMask.NameToLayer("Action")));

        if (hit.collider != null)
        { inSight = hit.collider.gameObject.CompareTag("Player") ? true : false; }

        return inSight;
    }

    void Idle()
    {
        // Gegner könnte herumwandern o.ä. sucht währenddessen nach Zielen
        // Neues System: Beim Laden der Scene wird ein Dictionary erstellt, in dem alle Spieler einem Aggro-Wert zugeordnet werden.
        // Wenn Skills verwendet werden (Heilung in bestimmten Radius) Dmg Spells von irgendwo oder ein Spieler zu nah kommt -> Aggrowert erhöht
        // Sobald der Aggrowert > 0 ist, greift Gegner an.

        // Diese beiden Funktionen müssen nicht jedes Frame ausgeführt werden. Reicht ~ jede Sekunde
        GetCurrentTarget();
        //SearchTargets();
    }

    private void Chasing()              // Gegner jagt aktuell Ziele
    {

        GetCurrentTarget();
        //SearchTargets();              // Soll weiterhin schauen, welches Ziel die höchste Aggro hat

        float distanceToTarget = Vector2.Distance(transform.position, target.position); // enemy <-> enemy's target (player)

        if (distanceToTarget > attackRange && TargetInSight(target) &&                 // Wenn Ziel am Leben, in Aggro-Range,        distanceToTarget < aggroRange wird für minions nicht gebraucht
            target.GetComponent<CharacterStats>().isAlive.Value == true)                        // kein anderes Ziel wichtiger und Ziel sichtbar
        {
            eMove.ChaseTarget();
            if (isLC == true) { isLC = false; StopCoroutine(lc); }
        }
        else if (target.GetComponent<CharacterStats>().isAlive.Value == false)         // Falls Charakter tot ist
        {
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
}
