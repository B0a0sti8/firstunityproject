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
    public Camera mainCam;

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
    public bool isCastOnSelf = false;
    public bool needsTargetEnemy;
    public bool needsTargetAlly;
    public bool canSelfCastIfNoTarget;
    public bool targetsEnemiesOnly;
    public bool targetsAlliesOnly;
    public List<GameObject> currentTargets = new List<GameObject>();
    protected GameObject mainTargetForCircleAoE;
    bool forceTargetPlayer;
    GameObject targetSnapShot;

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
    public bool isSkillChanneling = false;
    public bool isChannelingSkillEffectActive;

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
    public Vector3 coneAOEDirection;
    public float coneAOEAngle = 50;
    public Vector3 circleAim;
    //public bool isPlaceableAoE;
    public GameObject PlacableAOEIndicator;
    float indicatorScaleFactor = 1;

    //public GameObject unusedSpell;
    //bool hasUnusedSpell = false;

    public float skillDuration;

    Animator classAnimator;

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
        if (masterChecks.hasUnusedSpell) return; // Schaut ob der Spieler einen ungenutzten Skill in der Schwebe hat (v.a. Flächeneffekte die gezielt werden. Das sieht man mit der entsprechenden Fläche an der Maus. Wer macht überhaupt sowas behindertes?!)

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
        if (interactionCharacter.focus != null) targetSnapShot = interactionCharacter.focus.gameObject;
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

    public void RangeCheck() // check for range and line of sight
    {
        // Für Skills die kein Ziel brauchen
        if (!needsTargetAlly && !needsTargetEnemy) { OwnCooldownCheck(); return; } // QueueCheck


        // Falls kein Target aber selbst castable
        else if (interactionCharacter.focus == null && canSelfCastIfNoTarget) { OwnCooldownCheck(); return; } // QueueCheck

        // Spezialfall: Wenn eigentlich freundliches Target gebraucht wird, aber ein Gegner im Target ist, das jedoch nicht gemerkt wird, weil der Skill selfcastable ist, muss die Range nicht gecheckt werden.
        else if (LayerMask.NameToLayer("Enemy") == interactionCharacter.focus.gameObject.layer && needsTargetAlly && canSelfCastIfNoTarget)
        {
            forceTargetPlayer = true;
            OwnCooldownCheck(); // QueueCheck
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

            if (targetInSight) OwnCooldownCheck(); // target in sight // QueueCheck
            else Debug.Log("Target in range but NOT in sight"); // target not in sight

        }
        else Debug.Log("Target not in range: Distance " + distance + " > " + skillRange); // target not in range
    }

    public void OwnCooldownCheck() // checks for own cooldown
    {
        //Debug.Log("OwnCDCheck");
        if (hasOwnCooldown) // has own cooldown
        {
            if (ownCooldownTimeLeft <=0) QueueCheck(); // own cooldown not active //SuperInstantCheck
            else if (ownCooldownTimeLeft <= masterChecks.masterOwnCooldownEarlyTime) QueueCheck(); // time left <= 0.5 //SuperInstantCheck
            else Debug.Log("ERROR: Own cooldown active " + ownCooldownTimeLeft); // own cooldown active
        }
        else QueueCheck(); // has no own cooldown //SuperInstantCheck
    }

    public void QueueCheck() // checks if queue is empty
    {
        //Debug.Log("QueueCheck");
        if ((!isSuperInstant && !masterChecks.masterIsSkillInQueue) || (isSuperInstant && !isSkillInOwnSuperInstantQueue)) SuperInstantCheck(); // OwnCooldownCheck
        else Debug.Log("ERROR: queue full");
    }

    public void SuperInstantCheck() // checks if skill is a SuperInstant
    {
        //Debug.Log("SuperInstantCheck");
        if (isSuperInstant) StartCoroutine(WaitForSkillSuperInstant(ownCooldownTimeLeft));
        else PlaceableAoECheck();
    }

    public void PlaceableAoECheck()
    {
        //Debug.Log("PlaceableAoECheck");
        if (myAreaType == AreaType.CirclePlacable) PlaceableAoESkillProcedure(); // Für Placeable AoEs wird StartCasting() im MasterChecks Skript aufgerufen.
        else GlobalCooldownCheck();
    }

    // Hier werden die Parameter in MasterChecks gesetzt, dass eine Fläche erscheint die mit Linksklick bestätigt werden kann. Wenn das passiert, wird StartCasting() aufgerufen.
    public void PlaceableAoESkillProcedure()
    {
        //Debug.Log("PlaceableAoEProcedure");
        masterChecks.myUnusedSpellIndicator = Instantiate(PlacableAOEIndicator, mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Quaternion.identity);
        masterChecks.myUnusedSpellIndicator.transform.localScale *= skillRadius / indicatorScaleFactor;
        masterChecks.myUnusedSpell = this;
        masterChecks.hasUnusedSpell = true;
        //Debug.Log("Labl Labl");
    }

    public void GlobalCooldownCheck() // checks for GlobalCooldown and skill casting times and waits for skill
    {
        Debug.Log("GlobalCDCheck");
        if ((!hasGlobalCooldown  || !masterChecks.masterGCActive) && !playerStats.isCurrentlyCasting) // no GC and casting trouble
        {
            StartCoroutine(WaitForSkill(Mathf.Max(ownCooldownTimeLeft, masterChecks.masterAnimTimeLeft)));
        }
        else if ((masterChecks.masterGCTimeLeft <= masterChecks.masterGCEarlyTime) && (masterChecks.castTimeCurrent <= masterChecks.masterGCEarlyTime)) // GC early cast
        {
            Debug.Log("Early Cast.");
            StartCoroutine(WaitForSkill(Mathf.Max(ownCooldownTimeLeft, masterChecks.masterGCTimeLeft, masterChecks.masterAnimTimeLeft, masterChecks.castTimeCurrent)));
        }
        else // hasGlobalCooldown && globalCooldownActive // GC active (too early)
        {
            if (masterChecks.castTimeCurrent > masterChecks.masterGCEarlyTime) Debug.Log("ERROR: Casting" + masterChecks.castTimeCurrent+ masterChecks.masterGCEarlyTime);
            else Debug.Log("ERROR A: GC active (too early) " + masterChecks.masterGCTimeLeft + " > " + masterChecks.masterGCEarlyTime);            
            FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
        }
    }

    IEnumerator WaitForSkillSuperInstant(float time)
    {
        isSkillInOwnSuperInstantQueue = true;
        if (time > 0) { FindObjectOfType<AudioManager>().Play("HoverClickUpPitch"); }
        else { FindObjectOfType<AudioManager>().Play("HoverClick"); }
        yield return new WaitForSeconds(time);
        isSkillInOwnSuperInstantQueue = false;
        ownCooldownTimeLeft = ownCooldownTimeModified;
        if (needsMana) { playerStats.ManageMana(-manaCost); }
        StartCasting();
    }

    private IEnumerator WaitForSkill(float time)
    {
        masterChecks.masterIsSkillInQueue = true;
        if (time > 0) { FindObjectOfType<AudioManager>().Play("HoverClickUpPitch"); }
        else { FindObjectOfType<AudioManager>().Play("HoverClick"); }
        yield return new WaitForSeconds(time+ 0.01f);
        masterChecks.masterIsSkillInQueue = false;
        //Debug.Log("... Use Skill");
        StartCasting();
    }



    public virtual void StartCasting()
    {
        TriggerSkillCooldown();
        if (castTimeOriginal <= 0)
        {
            AOECheck();
            SkillEffect();
        }
        else
        {
            if (isSkillChanneling) playerStats.castingBarChanneling = true;
            else playerStats.castingBarChanneling = false;

            playerStats.castingBarImage = tooltipSkillSprite;
            playerStats.castingBarText = tooltipSkillName;
            PLAYER.transform.Find("PlayerParticleSystems").Find("CastingParticles").gameObject.GetComponent<ParticleSystem>().Play();

            castTimeModified = castTimeOriginal / playerStats.actionSpeed.GetValue();
            
            masterChecks.isSkillInterrupted = false;
            masterChecks.castTimeCurrent = castTimeModified;
            masterChecks.castTimeMax = castTimeModified;
            castStarted = true;
            playerStats.isCurrentlyCasting = true;

            if (isSkillChanneling)
            {
                AOECheck();
                // SkillEffect();
                isChannelingSkillEffectActive = true;
            }
        }
    }

    public void TriggerSkillCooldown()
    {
        // Modifiziert den Cooldown basierend auf aktuellem Tempo
        float attackSpeedModifier = 1 / (1 + playerStats.actionSpeed.GetValue());
        ownCooldownTimeModified = ownCooldownTimeBase * attackSpeedModifier;

        // TriggerSkillAnimation
        masterChecks.masterAnimationActive = true;
        masterChecks.masterAnimTimeLeft = masterChecks.masterAnimTime;

        // TriggerGlobalCooldown
        if (hasGlobalCooldown)
        {
            masterChecks.masterGCActive = true;
            masterChecks.masterGCTimeLeft = masterChecks.masterGCTimeModified;
        }

        // TriggerOwnCooldown
        if (hasOwnCooldown) ownCooldownTimeLeft = ownCooldownTimeModified;

        if (needsMana) playerStats.ManageMana(-manaCost);

        if (myAreaType == AreaType.Front) StartCoroutine(FrontAOEIndicator(0.2f));
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
                if (forceTargetPlayer) currentTargets.Add(PLAYER); // SONDERFALL!! Siehe Funktionsbeschreibung.
                else if (canSelfCastIfNoTarget) currentTargets.Add(PLAYER);
                else if (targetSnapShot != null) currentTargets.Add(targetSnapShot);
                break;

            // Skills die einen Kreis um ein Ziel herum betreffen
            case AreaType.CircleAroundTarget:
                // Holt sich alle Targets um das Ziel (Oder um den Spieler falls der Skill ohne passendes Ziel gecastet werden kann).
                List<GameObject> prelimTargetsCírcle = new List<GameObject>(); prelimTargetsCírcle.Clear();

                if (isCastOnSelf) prelimTargetsCírcle.AddRange(GetTargetsInCircleHelper(PLAYER.transform.position, skillRadius)); // Ein Selbst-Skill um den Spieler
                else if (forceTargetPlayer) prelimTargetsCírcle.AddRange(GetTargetsInCircleHelper(PLAYER.transform.position, skillRadius)); // SONDERFALL!! Siehe Funktionsbeschreibung. 
                else if (canSelfCastIfNoTarget) prelimTargetsCírcle.AddRange(GetTargetsInCircleHelper(PLAYER.transform.position, skillRadius)); // Skills die ein Ziel bräuchten, aber notfalls den SPieler nehmen können.
                else if (targetSnapShot != null) prelimTargetsCírcle.AddRange(GetTargetsInCircleHelper(targetSnapShot.transform.position, skillRadius)); // Skills die ein Ziel brauchen

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
                prelimTargetsCircPlacable.AddRange(GetTargetsInCircleHelper(circleAim, skillRadius));

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

    public virtual void SkillEffect() // overridden by each skill seperately
    {
        playerStats.OnCastedSkillCallback();
    }

    public virtual void Update()
    {
        if (masterChecks.masterIsCastFinished && castStarted)
        {
            AOECheck();
            SkillEffect();

            masterChecks.masterIsCastFinished = false;
            castStarted = false;
            isChannelingSkillEffectActive = false;
        }

        if (ownCooldownTimeLeft > 0) ownCooldownTimeLeft -= Time.deltaTime;
        else ownCooldownTimeLeft = 0;
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