using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Unity.Netcode;
//using UnityEditor.Animations;

public class SkillPrefab : NetworkBehaviour//, IUseable
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

    public string myClass;
    public float animationTime = 1.5f;

    [Header("Target")]
    public bool isSelfCast = false;
    public bool needsTargetEnemy;
    public bool needsTargetAlly;
    public bool canSelfCastIfNoTarget;
    public bool targetsEnemiesOnly;
    public bool targetsAlliesOnly;
    public List<GameObject> currentTargets = new List<GameObject>();
    protected GameObject mainTargetForCircleAoE;
    bool forceTargetPlayer;

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


    public enum AreaType
    {
        SingleTarget,
        SingleTargetSelf,
        CircleAroundTarget,
        Front,
        Line,
        CirclePlacable
    }

    public AreaType myAreaType;

    [Header("Area of Effect")]
    public bool isAOECircle = false;
    public bool isAOEFrontCone = false;
    public bool isAOELine = false;
    public Vector3 coneAOEDirection;
    public float coneAOEAngle = 50;
    private Interactable circleAim;
    public bool isPlacableAoE;
    public GameObject PlacableAOEIndicator;

    public GameObject unusedSpell;
    bool hasUnusedSpell = false;

    public float skillDuration;

    Animator classAnimator;

    private Interactable zwischenSpeicher;

    public void StartSkillChecks() // Checks if Player is alive.
    {
        if (PLAYER.GetComponent<PlayerStats>().isAlive.Value) ConditionCheck();
        else Debug.Log("ERROR: You are dead!");
        forceTargetPlayer = false;
    }

    public virtual void ConditionCheck() // checks for conditions (Mana, Aufladungen, ...)
    {
        // In den spezifischen Skills kann diese Methode überschrieben werden um andere Voraussetzungen hinzuzufügen.

        if (needsMana && playerStats.currentMana.Value < manaCost) return; // Schaut ob der Spieler Mana braucht und genug hat.
        TargetCheck();
    }

    public void TargetCheck() // checks for fitting target
    {
        // Wenn er ein gegnerisches Target braucht, und entweder keines oder keinen Gegner hat, und nicht auf sich selbst casten kann, wird abgebrochen.
        if (needsTargetEnemy) 
        {
            if ((interactionCharacter.focus == null || LayerMask.NameToLayer("Enemy") != interactionCharacter.focus.gameObject.layer) & !canSelfCastIfNoTarget) return;
        }

        // Wenn er ein freundliches Target braucht, und entweder keines oder keinen freundliches hat, und nicht auf sich selbst casten kann, wird abgebrochen.
        if (needsTargetAlly) 
        {
            if (interactionCharacter == null) return;
            else if (LayerMask.NameToLayer("Action") != interactionCharacter.focus.gameObject.layer 
                & LayerMask.NameToLayer("Ally") != interactionCharacter.focus.gameObject.layer 
                & !canSelfCastIfNoTarget) return;
        }

        // Wenn er bis hier her kommt, braucht er kein Target, kann den Skill auf sich selbst casten, falls er keines hat oder er hat ein passendes Target.
        RangeCheck();
    }

    // Eine Helferfunktion, die alle Ziele innerhalb eines Kreises findet.
    public List<GameObject> GetTargetsInCircleHelper(Vector3 circleCenter, float circleRadius)
    {
        List <GameObject> listOfMatches = new List<GameObject>();

        Collider2D[] hit = Physics2D.OverlapCircleAll(circleCenter, circleRadius, (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ally")) | (1 << LayerMask.NameToLayer("Action")));
        foreach (Collider2D coll in hit) listOfMatches.Add(coll.gameObject);
        
        return listOfMatches;
    }

    // Holt sich alle Ziele, die vom Skill betroffen werden. Basierend auf dem AreaType (z.B. singletarget, Front AoE, Cirle AoE) und den parametern targetsAlliesOnly und targetsEnemiesOnly
    // Anmerkung SONDERFALL!!: Es gibt den eigenartigen Fall, dass der Spieler ein freundliches Target braucht, aber ein gegnerisches hat. Durch canSelfCastIfNoTarget aber eigentlich auf sich selbst casten könnte.
    // Dieser Sonderfall wird durch die Variable forceTargetPlayer abgedeckt und die entsprechenden Zeilen sind mit "SONDERFALL!!" markiert.
    public void AOECheck()
    {
        currentTargets.Clear(); // Leert die Liste an Targets.

        // Hier werden alle Fälle durchgegangen, die an Zielen Vorhanden sind: Single Target / Skills auf einen Selbst / FrontAoE / Einen Kreis um das Ziel / Eine Linie / Einen setzbaren Kreis.
        switch (myAreaType)
        {
            // Skills die nur auf den Spieler gehen
            case AreaType.SingleTargetSelf:
                currentTargets.Add(PLAYER);
                break;

            // Skills die nur auf ein Ziel gehen
            case AreaType.SingleTarget:
                if (forceTargetPlayer) currentTargets.Add(interactionCharacter.focus.gameObject); // SONDERFALL!! Siehe Funktionsbeschreibung.
                else if (interactionCharacter.focus != null) currentTargets.Add(interactionCharacter.focus.gameObject);
                else if (canSelfCastIfNoTarget) currentTargets.Add(PLAYER);
                break;

            // Skills die einen Kreis um ein Ziel herum betreffen
            case AreaType.CircleAroundTarget:
                // Holt sich alle Targets um das Ziel (Oder um den Spieler falls der Skill ohne passendes Ziel gecastet werden kann).
                List<GameObject> prelimTargetsCírcle = new List<GameObject>(); prelimTargetsCírcle.Clear();

                if (isSelfCast) prelimTargetsCírcle.AddRange(GetTargetsInCircleHelper(PLAYER.transform.position, skillRadius)); // Ein Selbst-Skill um den Spieler
                if (forceTargetPlayer) prelimTargetsCírcle.AddRange(GetTargetsInCircleHelper(PLAYER.transform.position, skillRadius)); // SONDERFALL!! Siehe Funktionsbeschreibung.
                else if (interactionCharacter.focus != null) prelimTargetsCírcle.AddRange(GetTargetsInCircleHelper(interactionCharacter.focus.transform.position, skillRadius)); // Skills die ein Ziel brauchen
                else if (canSelfCastIfNoTarget) prelimTargetsCírcle.AddRange(GetTargetsInCircleHelper(PLAYER.transform.position, skillRadius)); // Skills die ein Ziel bräuchten, aber notfalls den SPieler nehmen können.

                // Sammelt nur Freunde aus den Zielen
                if (targetsAlliesOnly)
                {
                    foreach (GameObject preTa in prelimTargetsCírcle)
                    { if (preTa.layer == LayerMask.NameToLayer("Action") || preTa.layer == LayerMask.NameToLayer("Ally")) currentTargets.Add(preTa); }
                }

                // Sammelt alle Feinde aus den Zielen
                else if (targetsEnemiesOnly)
                {
                    foreach (GameObject preTa in prelimTargetsCírcle)
                    { if (preTa.layer == LayerMask.NameToLayer("Enemy")) currentTargets.Add(preTa); }
                }

                // Wenn der Skill sowohl Freunde als auch Partner betrifft, nimmt er alle.
                else currentTargets.AddRange(prelimTargetsCírcle);

                break;

            // Skills die alles in einem Winkel vor dem Spieler betreffen
            case AreaType.Front:
                // Vorgehen bisschen komisch: Schnappt sich erstmal alle Ziele in einem Kreis um den Spieler rum. Schaut dann, ob der Winkel zwischen der Spieler-Blickrichtung und der Richtung zum Gegner kleiner ist, als der ConeAOEAngle des Skills.
                List<GameObject> prelimTargetsFront = new List<GameObject>(); prelimTargetsFront.Clear();
                List<GameObject> prelimTargetsFront2 = new List<GameObject>(); prelimTargetsFront2.Clear();

                prelimTargetsFront.AddRange(GetTargetsInCircleHelper(PLAYER.transform.position, skillRadius));
                foreach (GameObject preTa in prelimTargetsFront)
                {
                    Vector2 newVector = (preTa.transform.position - PLAYER.transform.position);
                    if (Vector2.SignedAngle(PLAYER.GetComponent<PlayerController>().currentDirectionTrue, newVector) <= coneAOEAngle / 2) prelimTargetsFront2.Add(preTa); // Vergleicht ob der Winkel passt und schreibt alle Ziele in eine Liste
                }

                // Sammelt nur Freunde aus den Zielen
                if (targetsAlliesOnly)
                {
                    foreach (GameObject preTa in prelimTargetsFront2)
                    { if (preTa.layer == LayerMask.NameToLayer("Action") || preTa.layer == LayerMask.NameToLayer("Ally")) currentTargets.Add(preTa); }
                }

                // Sammelt alle Feinde aus den Zielen
                else if (targetsEnemiesOnly)
                {
                    foreach (GameObject preTa in prelimTargetsFront2)
                    { if (preTa.layer == LayerMask.NameToLayer("Enemy")) currentTargets.Add(preTa); }
                }

                // Wenn der Skill sowohl Freunde als auch Partner betrifft, nimmt er alle.
                else currentTargets.AddRange(prelimTargetsFront2);

                break;

            case AreaType.CirclePlacable:
                // Schnappt sich alle Ziele in einem Kreis um die derzeitige Position des Mauszeigers.
                List<GameObject> prelimTargetsCircPlacable = new List<GameObject>(); prelimTargetsCircPlacable.Clear();
                prelimTargetsCircPlacable.AddRange(GetTargetsInCircleHelper(mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue()), skillRadius));

                // Sammelt nur Freunde aus den Zielen
                if (targetsAlliesOnly)
                {
                    foreach (GameObject preTa in prelimTargetsCircPlacable)
                    { if (preTa.layer == LayerMask.NameToLayer("Action") || preTa.layer == LayerMask.NameToLayer("Ally")) currentTargets.Add(preTa); }
                }

                // Sammelt alle Feinde aus den Zielen
                else if (targetsEnemiesOnly)
                {
                    foreach (GameObject preTa in prelimTargetsCircPlacable)
                    { if (preTa.layer == LayerMask.NameToLayer("Enemy")) currentTargets.Add(preTa); }
                }

                // Wenn der Skill sowohl Freunde als auch Partner betrifft, nimmt er alle.
                else currentTargets.AddRange(prelimTargetsCircPlacable);

                break;

            case AreaType.Line:
                // Wird später implementiert.
                break;

            default:
                Debug.Log("Kein AreaType angegeben. Skill wird nichts. ");
                break;
        }
    }

    public void RangeCheck() // check for range and line of sight
    {
        // Für Skills die kein Ziel brauchen
        if (!needsTargetAlly && !needsTargetEnemy) QueueCheck();


        // Falls kein Target aber selbst castable
        if (interactionCharacter.focus == null && canSelfCastIfNoTarget) QueueCheck();

        // Spezialfall: Wenn eigentlich freundliches Target gebraucht wird, aber ein Gegner im Target ist, das jedoch nicht gemerkt wird, weil der Skill selfcastable ist, muss die Range nicht gecheckt werden.
        if (LayerMask.NameToLayer("Enemy") == interactionCharacter.focus.gameObject.layer && needsTargetAlly && canSelfCastIfNoTarget)
        {
            forceTargetPlayer = true;
            QueueCheck();
        }

        // Bestimmt den Abstand zwischen Ziel und Spieler
        float distance = Vector2.Distance(PLAYER.transform.position, interactionCharacter.focus.transform.position);
        if (distance <= skillRange) // target in range
        {
            // Versucht einen Raycast, ob Blickfeld durch Mauern o.ä. blockiert ist.
            RaycastHit2D[] hit = Physics2D.LinecastAll(PLAYER.transform.position,
                interactionCharacter.focus.gameObject.transform.position, (1 << LayerMask.NameToLayer("Borders")) |
                (1 << LayerMask.NameToLayer("Action")) | (1 << LayerMask.NameToLayer("Ally")) | (1 << LayerMask.NameToLayer("Enemy")));

            targetInSight = true;
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.gameObject.layer == LayerMask.NameToLayer("Borders")) targetInSight = false;
            }

            if (targetInSight) QueueCheck(); // target in sight
            else Debug.Log("Target in range but NOT in sight"); // target not in sight

        }
        else Debug.Log("Target not in range: Distance " + distance + " > " + skillRange); // target not in range
    }

    public void QueueCheck() // checks if queue is empty
    {
        if ((!isSuperInstant && !masterChecks.masterIsSkillInQueue) || (isSuperInstant && !isSkillInOwnSuperInstantQueue)) OwnCooldownCheck();
        else Debug.Log("ERROR: queue full");
    }

    public void OwnCooldownCheck() // checks for own cooldown
    {
        if (hasOwnCooldown) // has own cooldown
        {
            if (!ownCooldownActive) SuperInstantCheck(); // own cooldown not active
            else if (ownCooldownTimeLeft <= masterChecks.masterOwnCooldownEarlyTime) SuperInstantCheck(); // time left <= 0.5
            else Debug.Log("ERROR: Own cooldown active " + ownCooldownTimeLeft); // own cooldown active
        }
        else SuperInstantCheck(); // has no own cooldown
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
                if (needsMana) { playerStats.ManageMana(-manaCost); }
                StartCasting();
            }
        }
        else
        {
            GlobalCooldownCheck();
        }
    }

    public void GlobalCooldownCheck() // checks for GlobalCooldown and skill casting times and waits for skill
    {
        if ((!hasGlobalCooldown || (hasGlobalCooldown && !masterChecks.masterGCActive)) && !playerStats.isCurrentlyCasting) // no GC and casting trouble
        {
            StartCoroutine(WaitForSkill(Mathf.Max(ownCooldownTimeLeft, masterChecks.masterAnimTimeLeft)));
        }
        else if ((masterChecks.masterGCTimeLeft <= masterChecks.masterGCEarlyTime) && (masterChecks.castTimeCurrent <= masterChecks.masterGCEarlyTime)) // GC early cast
        {
            StartCoroutine(WaitForSkill(Mathf.Max(ownCooldownTimeLeft, masterChecks.masterGCTimeLeft, masterChecks.masterAnimTimeLeft, masterChecks.castTimeCurrent)));
        }
        else // hasGlobalCooldown && globalCooldownActive // GC active (too early)
        {
            if (masterChecks.castTimeCurrent > masterChecks.masterGCEarlyTime) Debug.Log("ERROR: Casting");
            else Debug.Log("ERROR A: GC active (too early) " + masterChecks.masterGCTimeLeft + " > " + masterChecks.masterGCEarlyTime);            
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

    public virtual void StartCasting()
    {
        if (castTimeOriginal <= 0)
        {
            AOECheck();
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
            //Debug.Log("Skloss");
            playerStats.isCurrentlyCasting = true;

            if (isSkillChanneling)
            {
                AOECheck();
                SkillEffect();
            }
        }
    }

    public void PlaySkillAnimation(string className, string animationName)
    {
        float animTime;
        List<RuntimeAnimatorController> allClassAnims = classAnimator.gameObject.GetComponent<MultiplayerAnimationControl>().allClassAnimationControllers;

        foreach (RuntimeAnimatorController mAC in allClassAnims)
        {
            if (mAC.name.Contains(className))
            {
                classAnimator.runtimeAnimatorController = mAC;
                classAnimator.Play(animationName);

                AnimationClip[] clips = classAnimator.runtimeAnimatorController.animationClips;
                foreach (AnimationClip cli in clips)
                {
                    Debug.Log("Check2");
                    if (cli.name == animationName)
                    {
                        Debug.Log("Check3");
                        animTime = cli.length;
                        classAnimator.speed = 1 / ((animationTime / animTime) * 1 / (1 + playerStats.actionSpeed.GetValue()));
                        Debug.Log("Eigentlicher Anim Speed: " + animTime / classAnimator.speed);

                        Debug.Log("Animation Clip length = " + classAnimator.GetCurrentAnimatorStateInfo(0).length);
                        StartCoroutine(StopAnimation(animationTime, classAnimator, className));
                    }
                }
            }
        }
    }

    public IEnumerator StopAnimation(float time, Animator lastAnim, string className)
    {
        yield return new WaitForSeconds(time);
        lastAnim.speed = 1;
        classAnimator.Play(className + "_idle");

    }

    public virtual void SkillEffect() // overridden by each skill seperately
    {
        playerStats.OnCastedSkillCallback();
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
            AOECheck();
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
        mainCam = GameObject.Find("CameraMama").transform.Find("MainKamera").GetComponent<Camera>();

        PLAYER = transform.parent.parent.gameObject;
        masterChecks = PLAYER.transform.Find("Own Canvases").Find("Canvas Action Skills").GetComponent<MasterChecks>();
        interactionCharacter = PLAYER.GetComponent<InteractionCharacter>();
        playerStats = PLAYER.GetComponent<PlayerStats>();
        classAnimator = PLAYER.transform.Find("PlayerAnimation").GetComponent<Animator>();
    }

    public virtual void Start()
    {
        
    }

    public void DealDamage(float damage)
    {
        for (int i = 0; i < currentTargets.Count; i++)
        { DamageOrHealing.DealDamage(PLAYER.GetComponent<NetworkBehaviour>(), currentTargets[i].GetComponent<NetworkBehaviour>(), damage, false, false); }
    }

    public void DealDamage(GameObject extraTarget, float damage)
    {
        if (extraTarget != null) DamageOrHealing.DealDamage(PLAYER.GetComponent<NetworkBehaviour>(), extraTarget.GetComponent<NetworkBehaviour>(), damage, false, false);
    }

    public void DoHealing(float healing)
    {
        for (int i = 0; i < currentTargets.Count; i++)
        { DamageOrHealing.DoHealing(PLAYER.GetComponent<NetworkBehaviour>(), currentTargets[i].GetComponent<NetworkBehaviour>(), healing); }
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



    public virtual void SetMyCooldown(float coolDown)
    {
        ownCooldownTimeBase = coolDown;
    }
}