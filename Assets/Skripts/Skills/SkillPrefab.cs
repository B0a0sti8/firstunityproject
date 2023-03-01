using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class SkillPrefab : MonoBehaviour//, IUseable
{
    private Camera mainCam;

    [HideInInspector]
    public MasterChecks masterChecks;
    [HideInInspector]
    public GameObject PLAYER;
    [HideInInspector]
    public InteractionCharacter interactionCharacter; // focus skript
    [HideInInspector]
    public PlayerStats playerStats;

    [Header("Target")]
    public bool needsTargetEnemy;
    public bool needsTargetAlly;
    public bool canSelfCastIfNoTarget;
    public bool targetsEnemiesOnly;
    public bool targetsAlliesOnly;
    public List<GameObject> currentTargets = new List<GameObject>();

    [Header("Mana")]
    public bool needsMana; // optional
    public int manaCost;

    [Header("Range/Radius")]
    public float skillRange;
    bool targetInSight;

    public float skillRadius; // unbenutzt bisher (außer für tooltip)

    [Header("Own Cooldown")]
    public bool hasOwnCooldown;
    public float ownCooldownTimeBase; // 0 if hasOwnCooldown = false
    [HideInInspector]
    public float ownCooldownTimeModified;
    [HideInInspector]
    public float ownCooldownTimeLeft;
    [HideInInspector]
    public bool ownCooldownActive = false;

    [Header("Skilltype")]
    public bool hasGlobalCooldown;

    public bool isSuperInstant; // can not be true if hasGlobalCooldown is true
    bool isSkillInOwnSuperInstantQueue = false;

    private Interactable zwischenSpeicher;

    [Header("Tooltip")]
    [HideInInspector]
    public MasterEventTrigger masterET;
    [HideInInspector]
    public string tooltipSkillName;
    [HideInInspector]
    public string tooltipSkillDescription;
    [HideInInspector]
    public Sprite tooltipSkillSprite;
    [HideInInspector]
    public string tooltipSkillType;
    [HideInInspector]
    public string tooltipSkillCooldown;
    [HideInInspector]
    public string tooltipSkillCosts;
    [HideInInspector]
    public string tooltipSkillRange;
    [HideInInspector]
    public string tooltipSkillRadius;

    [Header("Casting")]
    public float castTimeOriginal = 0f;
    public float castTimeModified;
    public bool castStarted = false;
    public bool isSkillChanneling;

    [Header("Area of Effect")]
    public bool isAOECircle = false;
    public bool isAOEFrontCone = false;
    public bool isAOELine = false;
    public bool isSelfCast = false;
    public Vector3 coneAOEDirection;
    public float coneAOEAngle = 50;
    private Interactable circleAim;
    public bool isPlacableAoE;
    public GameObject unusedSpell;
    public GameObject PlacableAOEIndicator;
    bool hasUnusedSpell = false;
    public float skillDuration;



    public void StartSkillChecks() // snjens beginnt sein abenteuer
    {
        if (PLAYER.GetComponent<PlayerStats>().isAlive.Value)
        {
            ConditionCheck();
        }
        else
        {
            Debug.Log("ERROR: You are dead!");
        }
    }

    public virtual void ConditionCheck() // checks for conditions (Mana, Aufladungen, ...)
    {
        if (needsMana)
        {
            if (playerStats.currentMana.Value >= manaCost)
            {
                TargetCheck();
            }
            else
            {
                Debug.Log("ERROR: not enough mana (Current: " + playerStats.currentMana.Value + ", Needed: " + manaCost + ")");
            }
        }
        else
        {
            TargetCheck();
        }
    }

    public void TargetCheck() // checks for fitting target
    {
        if (needsTargetEnemy) // for skills that need a enemy target
        {
            if (interactionCharacter.focus != null)
            {
                if (LayerMask.NameToLayer("Enemy") == interactionCharacter.focus.gameObject.layer) // if enemy in focus
                {
                    AOECheck();
                }
                else
                {
                    Debug.Log("No fitting target");
                    FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
                }
            }
            else
            {
                Debug.Log("Target needed");
                FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
            }
        }
        else if (needsTargetAlly) // for skill that need a friendly target
        {
            if (interactionCharacter.focus != null)
            {
                if (LayerMask.NameToLayer("Action") == interactionCharacter.focus.gameObject.layer || 
                    LayerMask.NameToLayer("Ally") == interactionCharacter.focus.gameObject.layer) // if ally in focus
                {
                    Debug.Log("Ally target");
                    AOECheck();
                }
                else
                {
                    if (canSelfCastIfNoTarget) // Hier weitermachen! Freund gebraucht, Gegner drinnen, selfcastable, heilt gegner.
                    {
                        zwischenSpeicher = interactionCharacter.focus;
                        interactionCharacter.focus = null;
                        AOECheck();
                        interactionCharacter.focus = zwischenSpeicher;
                    }
                    else
                    {
                        Debug.Log("No fitting target");
                        FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
                    }
                }
            }
            else
            {
                if (canSelfCastIfNoTarget)
                {
                    AOECheck();
                }
                else
                {
                    Debug.Log("Target needed");
                    FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
                }
            }
        }
        else // for skills that don't need a target
        {
            AOECheck();
        }
    }

    public void AOECheck()
    {
        currentTargets.Clear();
        if (!isAOECircle && !isAOEFrontCone && !isAOELine) // Skill ist singletarget
        {
            if (interactionCharacter.focus != null)
            { currentTargets.Add(interactionCharacter.focus.gameObject); }

            if (canSelfCastIfNoTarget && interactionCharacter.focus == null)
            { currentTargets.Add(PLAYER); }

            if (isSelfCast)
            { currentTargets.Add(PLAYER); }
        }

        if (isAOECircle) // Kreisförmiger Flächenzauber. Um Gegner, um freundliches Ziel oder um den Spieler selbst. Noch nicht implementiert: Bei Mausklick an entsprechende Stelle
        {
            isAOEFrontCone = false; isAOELine = false; // Nur sicherheitshalber, falls jmd mehrere Sachen angekreuzt hat.

            if (canSelfCastIfNoTarget && interactionCharacter.focus == null)
            { circleAim = PLAYER.GetComponent<Interactable>(); }
            else
            { circleAim = interactionCharacter.focus; }

            if (needsTargetEnemy) 
            {
                Collider2D[] hit = Physics2D.OverlapCircleAll(circleAim.gameObject.transform.position, skillRadius, (1 << LayerMask.NameToLayer("Enemy")));
                foreach (Collider2D coll in hit)
                {
                    currentTargets.Add(coll.gameObject);
                }
            }
            else if (needsTargetAlly)
            {
                Collider2D[] hit = Physics2D.OverlapCircleAll(circleAim.gameObject.transform.position, skillRadius, (1 << LayerMask.NameToLayer("Ally")) | (1 << LayerMask.NameToLayer("Action")));
                foreach (Collider2D coll in hit)
                {
                    currentTargets.Add(coll.gameObject);
                }
            }
            else if (isSelfCast)
            {
                if (targetsAlliesOnly)
                {
                    Collider2D[] hit = Physics2D.OverlapCircleAll(circleAim.gameObject.transform.position, skillRadius, (1 << LayerMask.NameToLayer("Ally")) | (1 << LayerMask.NameToLayer("Action")));
                    foreach (Collider2D coll in hit)
                    {
                        currentTargets.Add(coll.gameObject);
                    }
                }
                else if (targetsEnemiesOnly)
                {
                    Collider2D[] hit = Physics2D.OverlapCircleAll(circleAim.gameObject.transform.position, skillRadius, (1 << LayerMask.NameToLayer("Enemy")));
                    foreach (Collider2D coll in hit)
                    {
                        currentTargets.Add(coll.gameObject);
                    }
                }
                else
                {
                    Collider2D[] hit = Physics2D.OverlapCircleAll(circleAim.gameObject.transform.position, skillRadius, (1 << LayerMask.NameToLayer("Ally")) | (1 << LayerMask.NameToLayer("Action")));
                    foreach (Collider2D coll in hit)
                    {
                        currentTargets.Add(coll.gameObject);
                    }

                    Collider2D[] hit2 = Physics2D.OverlapCircleAll(circleAim.gameObject.transform.position, skillRadius, (1 << LayerMask.NameToLayer("Enemy")));
                    foreach (Collider2D coll in hit2)
                    {
                        currentTargets.Add(coll.gameObject);
                    }
                }
            }
            else if(isPlacableAoE) // Platzierbarer Flächenzauber. Kommt später
            {
                QueueCheck();
            }
        }

        else if (isAOEFrontCone)
        {
            isAOELine = false; // Nur sicherheitshalber, falls jmd mehrere Sachen angekreuzt hat.
            circleAim = PLAYER.GetComponent<Interactable>();

            if (targetsAlliesOnly)
            {
                Collider2D[] hit = Physics2D.OverlapCircleAll(circleAim.gameObject.transform.position, skillRadius, (1 << LayerMask.NameToLayer("Ally")) | (1 << LayerMask.NameToLayer("Action")));
                foreach (Collider2D coll in hit)
                {
                    Vector2 newVector = (coll.transform.position - PLAYER.transform.position);
                    if (Vector2.SignedAngle(PLAYER.GetComponent<PlayerController>().currentDirectionTrue, newVector) <= coneAOEAngle)
                    {
                        currentTargets.Add(coll.gameObject);
                    }
                }
            }
            else if (targetsEnemiesOnly)
            {
                Collider2D[] hit = Physics2D.OverlapCircleAll(circleAim.gameObject.transform.position, skillRadius, (1 << LayerMask.NameToLayer("Enemy")));
                foreach (Collider2D coll in hit)
                {
                    Vector2 newVector = (coll.transform.position - PLAYER.transform.position);
                    if (Vector2.SignedAngle(PLAYER.GetComponent<PlayerController>().currentDirectionTrue, newVector) <= coneAOEAngle)
                    {
                        currentTargets.Add(coll.gameObject);
                    }
                }
            }
            else
            {
                Collider2D[] hit = Physics2D.OverlapCircleAll(circleAim.gameObject.transform.position, skillRadius, (1 << LayerMask.NameToLayer("Ally")) | (1 << LayerMask.NameToLayer("Action")));
                foreach (Collider2D coll in hit)
                {
                    Vector2 newVector = (coll.transform.position - PLAYER.transform.position);
                    if (Vector2.SignedAngle(PLAYER.GetComponent<PlayerController>().currentDirectionTrue, newVector) <= coneAOEAngle)
                    {
                        currentTargets.Add(coll.gameObject);
                    }
                }

                Collider2D[] hit2 = Physics2D.OverlapCircleAll(circleAim.gameObject.transform.position, skillRadius, (1 << LayerMask.NameToLayer("Enemy")));
                foreach (Collider2D coll in hit2)
                {
                    Vector2 newVector = (coll.transform.position - PLAYER.transform.position);
                    if (Vector2.SignedAngle(PLAYER.GetComponent<PlayerController>().currentDirectionTrue, newVector) <= coneAOEAngle)
                    {
                        currentTargets.Add(coll.gameObject);
                    }
                }
            }
        }

        else if (isAOELine)
        {
            
        }


        if(isPlacableAoE != true)
        {
            RangeCheck();
        }
    }

    public void RangeCheck() // check for range and line of sight
    {
        if (needsTargetEnemy || needsTargetAlly) // // for skills that need a target
        {
            if (interactionCharacter.focus == null && canSelfCastIfNoTarget)        // Falls kein Target aber selbst castable
            {
                QueueCheck();
            }
            else
            {
                float distance = Vector2.Distance(PLAYER.transform.position,
                interactionCharacter.focus.gameObject.transform.position);
                if (distance <= skillRange) // target in range
                {
                    RaycastHit2D[] hit = Physics2D.LinecastAll(PLAYER.transform.position,
                        interactionCharacter.focus.gameObject.transform.position, (1 << LayerMask.NameToLayer("Borders")) |
                        (1 << LayerMask.NameToLayer("Action")) | (1 << LayerMask.NameToLayer("Ally")) | (1 << LayerMask.NameToLayer("Enemy")));
                    targetInSight = true;
                    for (int i = 0; i < hit.Length; i++)
                    {
                        if (hit[i].collider.gameObject.layer == LayerMask.NameToLayer("Borders"))
                        {
                            targetInSight = false;
                        }
                    }
                    if (targetInSight) // target in sight
                    {
                        QueueCheck();
                    }
                    else // target not in sight
                    {
                        Debug.Log("Target in range but NOT in sight");
                        FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
                    }
                }
                else // target not in range
                {
                    if (LayerMask.NameToLayer("Enemy") == interactionCharacter.focus.gameObject.layer && needsTargetAlly && canSelfCastIfNoTarget) // Spezialfall: Wenn eigentlich freundliches Target gebraucht wird, aber ein Gegner im Target ist, das jedoch nicht gemerkt wird, weil der Skill selfcastable ist, muss die Range nicht gecheckt werden.
                    {
                        currentTargets.Clear();
                        currentTargets.Add(PLAYER);
                        QueueCheck();
                    }
                    else
                    {
                        Debug.Log("Target not in range: Distance " + distance + " > " + skillRange);
                        FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
                    }
                }
            }
        }
        else // for skills that don't need a target
        {
            QueueCheck();
        }
    }

    public void QueueCheck() // checks if queue is empty
    {
        if ((!isSuperInstant && !masterChecks.masterIsSkillInQueue) || (isSuperInstant && !isSkillInOwnSuperInstantQueue))
        {
            OwnCooldownCheck();
        }
        else
        {
            Debug.Log("ERROR: queue full");
            FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
        }
    }

    public void OwnCooldownCheck() // checks for own cooldown
    {
        if (hasOwnCooldown) // has own cooldown
        {
            if (!ownCooldownActive) // own cooldown not active
            {
                SuperInstantCheck(); // UseSkill1
            }
            else if (ownCooldownTimeLeft <= masterChecks.masterOwnCooldownEarlyTime) // time left <= 0.5
            {
                SuperInstantCheck(); // UseSkill2
            }
            else // own cooldown active
            {
                Debug.Log("ERROR: Own cooldown active " + ownCooldownTimeLeft);
                FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
            }
        }
        else // has no own cooldown
        {
            SuperInstantCheck(); // UseSkill1
        }
    }

    public void SuperInstantCheck() // checks if skill is a SuperInstant
    {
        if (isSuperInstant)
        {
            StartCoroutine(Wait(ownCooldownTimeLeft));
            IEnumerator Wait(float time)
            {
                isSkillInOwnSuperInstantQueue = true;
                if (time > 0) { FindObjectOfType<AudioManager>().Play("HoverClickUpPitch"); }
                else { FindObjectOfType<AudioManager>().Play("HoverClick"); }
                yield return new WaitForSeconds(time);
                isSkillInOwnSuperInstantQueue = false;
                ownCooldownActive = true;
                ownCooldownTimeLeft = ownCooldownTimeModified;
                if (needsMana) { playerStats.currentMana.Value -= manaCost; }
                StartCasting();
            }
        }
        else
        {
            UseSkill3();
        }
    }

    public void UseSkill3() // checks for GlobalCooldown and skill casting times and waits for skill
    {
        if ((!hasGlobalCooldown || (hasGlobalCooldown && !masterChecks.masterGCActive)) && !playerStats.isCurrentlyCasting) // no GC and casting trouble
        {
            //if (isPlacableAoE)
            //{ PlacableAoESkillProcedure(); }
            //else
            StartCoroutine(WaitForSkill(Mathf.Max(ownCooldownTimeLeft, masterChecks.masterAnimTimeLeft)));
        }
        else if ((masterChecks.masterGCTimeLeft <= masterChecks.masterGCEarlyTime) && (masterChecks.castTimeCurrent <= masterChecks.masterGCEarlyTime)) // GC early cast
        {
            //if (isPlacableAoE)
            //{ PlacableAoESkillProcedure(); }
            //else
            StartCoroutine(WaitForSkill(Mathf.Max(ownCooldownTimeLeft, masterChecks.masterGCTimeLeft, masterChecks.masterAnimTimeLeft, masterChecks.castTimeCurrent)));
        }
        else // hasGlobalCooldown && globalCooldownActive // GC active (too early)
        {
            if (masterChecks.castTimeCurrent > masterChecks.masterGCEarlyTime)
            { Debug.Log("ERROR: Casting"); }
            else
            { Debug.Log("ERROR A: GC active (too early) " + masterChecks.masterGCTimeLeft + " > " + masterChecks.masterGCEarlyTime); }
            
            FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
        }
    }

    private IEnumerator WaitForSkill(float time)
    {
        masterChecks.masterIsSkillInQueue = true;
        if (time > 0) { FindObjectOfType<AudioManager>().Play("HoverClickUpPitch"); }
        else { FindObjectOfType<AudioManager>().Play("HoverClick"); }
        yield return new WaitForSeconds(time);
        masterChecks.masterIsSkillInQueue = false;
        //Debug.Log("... Use Skill");
        if (isPlacableAoE)
        {
            PlacableAoESkillProcedure();
        }
        else
        {
            TriggerSkill();
        }
    }

    public void TriggerSkill()
    {
        // TriggerGlobalCooldown
        if (hasGlobalCooldown)
        {
            masterChecks.masterGCActive = true;
            masterChecks.masterGCTimeLeft = masterChecks.masterGCTimeModified;
        }

        // TriggerSkillAnimation
        masterChecks.masterAnimationActive = true;
        masterChecks.masterAnimTimeLeft = masterChecks.masterAnimTime;

        // TriggerOwnCooldown
        if (hasOwnCooldown)
        {
            ownCooldownActive = true;
            ownCooldownTimeLeft = ownCooldownTimeModified;
        }

        if (needsMana)
        {
            playerStats.ManageMana(-manaCost);
        }

        if (isAOEFrontCone)
        {
            StartCoroutine(FrontAOEIndicator(0.2f));
        }

        StartCasting();
    }

    public void StartCasting()
    {
        if (castTimeOriginal <= 0)
        {
            SkillEffect();
            if (isPlacableAoE)
            {
                unusedSpell.GetComponent<AoESpellIndicator>().duration = skillDuration;
                unusedSpell.GetComponent<AoESpellIndicator>().isIndicatorActive = true;
                unusedSpell = null;
                hasUnusedSpell = false;
                Debug.Log("Zahle Mana");
            }
        }
        else
        {
            if (isSkillChanneling)
            { playerStats.castingBarChanneling = true; }
            else
            { playerStats.castingBarChanneling = false; }
            playerStats.castingBarImage = tooltipSkillSprite;
            playerStats.castingBarText = tooltipSkillName;
            
            PLAYER.transform.Find("PlayerParticleSystems").Find("CastingParticles").gameObject.GetComponent<ParticleSystem>().Play();
            masterChecks.isSkillInterrupted = false;
            castTimeModified = castTimeOriginal / playerStats.actionSpeed.GetValue();
            masterChecks.castTimeCurrent = castTimeModified;
            masterChecks.castTimeMax = castTimeModified;
            castStarted = true;
            Debug.Log("Skloss");
            playerStats.isCurrentlyCasting = true;

            if (isSkillChanneling)
            {
                SkillEffect();
            }
        }
    }

    public virtual void SkillEffect() // overridden by each skill seperately
    {
        
    }

    public virtual void Update()
    {
        if (masterChecks.masterIsCastFinished && castStarted)
        {
            if (isPlacableAoE)
            {
                unusedSpell.GetComponent<AoESpellIndicator>().duration = skillDuration;
                unusedSpell.GetComponent<AoESpellIndicator>().isIndicatorActive = true;
                unusedSpell = null;
                hasUnusedSpell = false;
                Debug.Log("Zahle Mana");
            }
            SkillEffect();
            castStarted = false;
            if (!isSkillChanneling)
            {
                masterChecks.masterIsCastFinished = false;
            }
        }
        

        if (ownCooldownTimeLeft > 0)
        {
            ownCooldownTimeLeft -= Time.deltaTime;
        }
        else
        {
            if (ownCooldownActive)
            {
                ownCooldownActive = false;
                ownCooldownTimeLeft = 0;
            }
        }

        float attackSpeedModifier = 1 - (playerStats.actionSpeed.GetValue() / 100);
        ownCooldownTimeModified = ownCooldownTimeBase * attackSpeedModifier;

        if (unusedSpell != null || hasUnusedSpell == true)
        {
            Vector3 mouseScreenposition = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            unusedSpell.transform.position = new Vector3(mouseScreenposition.x, mouseScreenposition.y, 0);

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Destroy(unusedSpell);
                unusedSpell = null;
                hasUnusedSpell = false;
            }
            else if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                TriggerSkill();
            }
        }
    }

    void Awake()
    {
        PLAYER = transform.parent.transform.parent.gameObject;

        masterChecks = PLAYER.transform.Find("Own Canvases").transform.Find("Canvas Action Skills").GetComponent<MasterChecks>();

        interactionCharacter = PLAYER.GetComponent<InteractionCharacter>();

        playerStats = PLAYER.GetComponent<PlayerStats>();
        
        mainCam = GameObject.Find("CameraMama").transform.Find("Main Camera").GetComponent<Camera>();
    }

    public virtual void Start()
    {
    }

    public void DealDamage(float damage)
    {
        for (int i = 0; i < currentTargets.Count; i++)
        {
            DamageOrHealing.DealDamage(PLAYER.GetComponent<NetworkBehaviour>(), currentTargets[i].GetComponent<NetworkBehaviour>(), damage, false, false);
        }
    }

    public void DoHealing(float healing)
    {
        
        for (int i = 0; i < currentTargets.Count; i++)
        {
            DamageOrHealing.DoHealing(PLAYER.GetComponent<NetworkBehaviour>(), currentTargets[i].GetComponent<NetworkBehaviour>(), healing);
            //currentTargets[i].GetComponent<CharacterStats>().view.RPC("GetHealing", RpcTarget.All, healing, critRandom, critChance, critMultiplier);
        }
    }

    public IEnumerator FrontAOEIndicator(float time)
    {
        Vector2 normalSize = PLAYER.transform.Find("RotationMeasurement").GetComponent<SpriteRenderer>().size;
        PLAYER.transform.Find("RotationMeasurement").GetComponent<SpriteRenderer>().color = Color.red;
        var frontSpriteColor1 = PLAYER.transform.Find("RotationMeasurement").GetComponent<SpriteRenderer>().color;
        frontSpriteColor1.a = 0.1f;
        PLAYER.transform.Find("RotationMeasurement").GetComponent<SpriteRenderer>().color = frontSpriteColor1;
        PLAYER.transform.Find("RotationMeasurement").GetComponent<SpriteRenderer>().size = new Vector2(skillRadius / (normalSize.x * 1.3f), skillRadius / (normalSize.y * 1.3f));

        yield return new WaitForSeconds(time);

        PLAYER.transform.Find("RotationMeasurement").GetComponent<SpriteRenderer>().color = Color.white;
        var frontSpriteColor2 = PLAYER.transform.Find("RotationMeasurement").GetComponent<SpriteRenderer>().color;
        frontSpriteColor2.a = 0.1f;
        PLAYER.transform.Find("RotationMeasurement").GetComponent<SpriteRenderer>().color = frontSpriteColor2;
        PLAYER.transform.Find("RotationMeasurement").GetComponent<SpriteRenderer>().size = normalSize;
    }

    public IEnumerator CircleAOEIndicator(float time)
    {
        Vector2 normalSize = PLAYER.transform.Find("RotationMeasurement").Find("CircleAOEIndicator").GetComponent<SpriteRenderer>().size;

        PLAYER.transform.Find("RotationMeasurement").Find("CircleAOEIndicator").GetComponent<SpriteRenderer>().enabled = true;
        PLAYER.transform.Find("RotationMeasurement").Find("CircleAOEIndicator").GetComponent<SpriteRenderer>().color = Color.red;
        var frontSpriteColor1 = PLAYER.transform.Find("RotationMeasurement").Find("CircleAOEIndicator").GetComponent<SpriteRenderer>().color;
        frontSpriteColor1.a = 0.1f;
        PLAYER.transform.Find("RotationMeasurement").Find("CircleAOEIndicator").GetComponent<SpriteRenderer>().color = frontSpriteColor1;
        PLAYER.transform.Find("RotationMeasurement").Find("CircleAOEIndicator").GetComponent<SpriteRenderer>().size = new Vector2(skillRadius / (normalSize.x * 1.3f), skillRadius / (normalSize.y * 1.3f));

        yield return new WaitForSeconds(time);

        PLAYER.transform.Find("RotationMeasurement").Find("CircleAOEIndicator").GetComponent<SpriteRenderer>().color = Color.white;
        var frontSpriteColor2 = PLAYER.transform.Find("RotationMeasurement").Find("CircleAOEIndicator").GetComponent<SpriteRenderer>().color;
        frontSpriteColor2.a = 0.1f;
        PLAYER.transform.Find("RotationMeasurement").Find("CircleAOEIndicator").GetComponent<SpriteRenderer>().color = frontSpriteColor2;
        PLAYER.transform.Find("RotationMeasurement").Find("CircleAOEIndicator").GetComponent<SpriteRenderer>().size = normalSize;
        PLAYER.transform.Find("RotationMeasurement").Find("CircleAOEIndicator").GetComponent<SpriteRenderer>().enabled = false;
    }

    public void PlacableAoESkillProcedure()
    {
        if (!hasUnusedSpell)
        {
            unusedSpell = Instantiate(PlacableAOEIndicator, mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Quaternion.identity);
            hasUnusedSpell = true;
            Debug.Log("Labl Labl");
        }
        else // Wenn Unused Spell
        {
            Debug.Log("Gubl Gubl");
        }
    }
}


//public void Use()
//{
//    throw new System.NotImplementedException();
//}

#region UseSkill1()
//public void UseSkill() // checks for time between skill (e.g. Animation, GlobalCooldown) (+ stuff)
//{
//    if (!masterChecks.masterAnimationActive)
//    {
//        if (hasGlobalCooldown)
//        {
//            if (!masterChecks.masterGCActive)
//            {
//                Debug.Log("Use GCSkill normal");
//                FindObjectOfType<AudioManager>().Play("HoverClick");
//                TriggerSkill();
//            }
//            else // GlobalCooldown Activ
//            {
//                if (masterChecks.masterGCTimeLeft <= masterChecks.masterGCEarlyTime) //&& !masterChecks.masterIsSkillInQueue)
//                {
//                    Debug.Log("Global Cooldown. Waiting for GCSkill ...  " + masterChecks.masterGCTimeLeft + "  " + masterChecks.masterGCTime);
//                    masterChecks.masterIsSkillInQueue = true;
//                    FindObjectOfType<AudioManager>().Play("HoverClickUpPitch");
//                    StartCoroutine(Wait(masterChecks.masterGCTimeLeft));
//                    IEnumerator Wait(float time)
//                    {
//                        yield return new WaitForSeconds(time);
//                        masterChecks.masterIsSkillInQueue = false;
//                        Debug.Log("... Use GCSkill");
//                        TriggerSkill();
//                    }
//                }
//                else if (masterChecks.masterIsSkillInQueue)
//                {
//                    Debug.Log("ERROR GC: queue full ????????????????????");
//                    FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
//                }
//                else
//                {
//                    Debug.Log("ERROR GC: too early   " + masterChecks.masterGCTimeLeft);
//                    FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
//                }
//            }
//        }
//        else // kein GlobalCooldown Skill
//        {
//            Debug.Log("Use InstantSkill normal");
//            FindObjectOfType<AudioManager>().Play("HoverClick");
//            TriggerSkill();
//        }
//    }
//    else // Animation wait active
//    {
//        if (!hasGlobalCooldown) //&& !masterChecks.masterIsSkillInQueue)
//        {
//            masterChecks.masterIsSkillInQueue = true;
//            FindObjectOfType<AudioManager>().Play("HoverClickUpPitch");
//            Debug.Log("Animation wait active ... " + masterChecks.masterAnimTimeLeft);
//            StartCoroutine(Wait(masterChecks.masterAnimTimeLeft)); // wait until animation is over
//            IEnumerator Wait(float time)
//            {
//                yield return new WaitForSeconds(time);
//                masterChecks.masterIsSkillInQueue = false;
//                Debug.Log("... Use InstantSkill");
//                TriggerSkill();
//            }
//        }
//        else if (!masterChecks.masterGCActive) //&& hasGlobalCooldown) //&& !masterChecks.masterIsSkillInQueue) //???
//        {
//            masterChecks.masterIsSkillInQueue = true;
//            FindObjectOfType<AudioManager>().Play("HoverClickUpPitch");
//            Debug.Log("Animation wait active ... " + masterChecks.masterAnimTimeLeft);
//            StartCoroutine(Wait(masterChecks.masterAnimTimeLeft)); // wait until animation is over
//            IEnumerator Wait(float time)
//            {
//                yield return new WaitForSeconds(time);
//                masterChecks.masterIsSkillInQueue = false;
//                Debug.Log("... Use GCSkill");
//                TriggerSkill();
//            }
//        }
//        else if (masterChecks.masterGCTimeLeft <= masterChecks.masterGCEarlyTime)
//        {
//            masterChecks.masterIsSkillInQueue = true;
//            FindObjectOfType<AudioManager>().Play("HoverClickUpPitch");
//            Debug.Log("CDTimeLeft: " + masterChecks.masterGCTimeLeft + "    AnimTimeLeft: " + masterChecks.masterAnimTimeLeft);
//            if (masterChecks.masterGCTimeLeft >= masterChecks.masterAnimTimeLeft)
//            {
//                Debug.Log("Global Cooldown. Waiting for GCSkill ..." + masterChecks.masterGCTimeLeft);
//                StartCoroutine(Wait(masterChecks.masterGCTimeLeft)); // wait until global cooldown is over
//            }
//            else // AnimTimeLeft > GCTimeLeft
//            {
//                Debug.Log("Animation wait active ... " + masterChecks.masterAnimTimeLeft);
//                StartCoroutine(Wait(masterChecks.masterAnimTimeLeft)); // wait until animation is over
//            }
//            IEnumerator Wait(float time)
//            {
//                yield return new WaitForSeconds(time);
//                masterChecks.masterIsSkillInQueue = false;
//                Debug.Log("... Use GCSkill");
//                TriggerSkill();
//            }
//        }
//        else if (masterChecks.masterIsSkillInQueue)
//        {
//            Debug.Log("ERROR A: queue full ?????????????");
//            FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
//        }
//        else // !skillInQueue && hasGlobalCooldown && globalCooldownActive
//        {
//            Debug.Log("ERROR A: GC active   " + masterChecks.masterGCTimeLeft);
//            FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
//        }
//    }
//}
#endregion

// different Metod of getting a distance (might be better or not)
// float distance2 = (playerSkillSkript.transform.position - interactionCharacter.focus.gameObject.transform.position).sqrMagnitude;
// if (distance2 <= skillRange * skillRange)