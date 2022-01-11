using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class SkillPrefab : MonoBehaviour//, IUseable
{
    [HideInInspector]
    public PhotonView photonView;
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



    public void StartSkillChecks() // snjens beginnt sein abenteuer
    {
        if (PLAYER.GetComponent<PlayerStats>().isAlive)
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
            if (playerStats.currentMana >= manaCost)
            {
                TargetCheck();
            }
            else
            {
                Debug.Log("ERROR: not enough mana (Current: " + playerStats.currentMana + ", Needed: " + manaCost + ")");
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
                    //Debug.Log(interactionCharacter.focus.gameObject.layer);
                    RangeCheck();
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
                    //Debug.Log(interactionCharacter.focus.gameObject.layer);
                    Debug.Log("Ally target");
                    RangeCheck();
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
        else // for skills that don't need a target
        {
            RangeCheck();
        }
    }

    public void RangeCheck() // check for range and line of sight
    {
        if (needsTargetEnemy || needsTargetAlly) // // for skills that need a target
        {
            float distance = Vector2.Distance(PLAYER.transform.position, 
                interactionCharacter.focus.gameObject.transform.position);
            if (distance <= skillRange) // target in range
            {
                //Debug.Log("Target in range");
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
                    //Debug.Log("Target in range and in sight");
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
                Debug.Log("Target not in range: Distance " + distance + " > " + skillRange);
                FindObjectOfType<AudioManager>().Play("HoverClickDownPitch");
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
            //Debug.Log("Wait for OwnCooldown ...  " + ownCooldownTimeLeft);
            StartCoroutine(Wait(ownCooldownTimeLeft));
            IEnumerator Wait(float time)
            {
                isSkillInOwnSuperInstantQueue = true;
                if (time > 0) { FindObjectOfType<AudioManager>().Play("HoverClickUpPitch"); }
                else { FindObjectOfType<AudioManager>().Play("HoverClick"); }
                yield return new WaitForSeconds(time);
                isSkillInOwnSuperInstantQueue = false;
                //Debug.Log("... Use SuperInstant");
                ownCooldownActive = true;
                ownCooldownTimeLeft = ownCooldownTimeModified;
                if (needsMana) { playerStats.currentMana -= manaCost; }
                SkillEffect();
            }
        }
        else
        {
            UseSkill3();
        }
    }

    public void UseSkill3() // checks for GlobalCooldown and waits for skill
    {
        if (!hasGlobalCooldown || (hasGlobalCooldown && !masterChecks.masterGCActive)) // no GC trouble
        {
            //Debug.Log("Wait for OwnCooldown / Animation ...  " + ownCooldownTimeLeft + " / " + masterChecks.masterAnimTimeLeft);
            StartCoroutine(WaitForSkill(Mathf.Max(ownCooldownTimeLeft, masterChecks.masterAnimTimeLeft)));
        }
        else if (masterChecks.masterGCTimeLeft <= masterChecks.masterGCEarlyTime) // GC early cast
        {
            //Debug.Log("Wait for OwnCooldown / GlobalCooldown / Animation ...  " + ownCooldownTimeLeft + " / " + masterChecks.masterGCTimeLeft + " / " + masterChecks.masterAnimTimeLeft);
            StartCoroutine(WaitForSkill(Mathf.Max(ownCooldownTimeLeft, masterChecks.masterGCTimeLeft, masterChecks.masterAnimTimeLeft)));
        }
        else // hasGlobalCooldown && globalCooldownActive // GC active (too early)
        {
            Debug.Log("ERROR A: GC active (too early) " + masterChecks.masterGCTimeLeft + " > " + masterChecks.masterGCEarlyTime);
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
        TriggerSkill();
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
            playerStats.ManageManaRPC(-manaCost);
        }

        SkillEffect();
    }

    public virtual void SkillEffect() // overridden by each skill seperately
    {
        
    }

    public virtual void Update()
    {
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

        float attackSpeedModifier = 1 - (playerStats.attackSpeed.GetValue() / 100);
        ownCooldownTimeModified = ownCooldownTimeBase * attackSpeedModifier;
    }

    void Awake()
    {
        PLAYER = transform.parent.transform.parent.gameObject;

        photonView = PLAYER.GetComponent<PhotonView>();

        masterChecks = PLAYER.transform.Find("Own Canvases").transform.Find("Canvas Action Skills").GetComponent<MasterChecks>();

        interactionCharacter = PLAYER.GetComponent<InteractionCharacter>();

        playerStats = PLAYER.GetComponent<PlayerStats>();
    }

    public virtual void Start()
    {
    }

    public void DealDamage(float damage)
    {
        int missRandom = Random.Range(1, 100);
        int critRandom = Random.Range(1, 100);
        float critChance = playerStats.critChance.GetValue();
        float critMultiplier = playerStats.critMultiplier.GetValue();
        interactionCharacter.focus.gameObject.GetComponent<EnemyStats>().view.RPC("TakeDamage", RpcTarget.All, damage, missRandom, critRandom, critChance, critMultiplier);
    }

    public void DoHealing(float healing)
    {
        int critRandom = Random.Range(1, 100);
        float critChance = playerStats.critChance.GetValue();
        float critMultiplier = playerStats.critMultiplier.GetValue();
        playerStats.view.RPC("GetHealing", RpcTarget.All, healing, critRandom, critChance, critMultiplier);
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