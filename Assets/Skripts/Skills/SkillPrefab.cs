using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPrefab : MonoBehaviour
{
    public GameObject playerSkillSkript;

    public bool hasOwnCooldown;
    public float ownCooldownTime;
    float ownCooldownTimeLeft;
    bool ownCooldownActive = false;

    public bool needsTargetEnemy;
    public bool needsTargetAlly;
    public InteractionCharacter interactionCharacter;

    public float skillRange;
    bool targetInSight;

    bool skillAnimation = false;
    public float saTime = 0.5f; // NOTE: similar to gcEarlyTime and NOT gcdTime
    float saTimeLeft;

    public bool hasGlobalCooldown;
    bool globalCooldownActive = false;
    public float gcdTime = 1.5f;
    public float gcEarlyTime = 0.5f;
    float gcdTimeLeft;

    bool isSkillInQueue = false;



    public void StartSkillChecks() // snjens beginnt sein abenteuer
    {
        OwnCooldownCheck();
    }

    public void OwnCooldownCheck() // checks for own cooldown
    {
        if (hasOwnCooldown) // has own cooldown
        {
            if (!ownCooldownActive) // own cooldown not active
            {
                ConditionCheck();
            }
            else // own cooldown active
            {
                Debug.Log("Own cooldown active");
            }
        }
        else // has no own cooldown
        {
            ConditionCheck();
        }
    }

    public virtual void ConditionCheck() // checks for conditions (Mana, Aufladungen, ...)
    {
        TargetCheck();
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
                }
            }
            else
            {
                Debug.Log("Target needed");
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
                }
            }
            else
            {
                Debug.Log("Target needed");
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
            float distance = Vector2.Distance(playerSkillSkript.transform.position, 
                interactionCharacter.focus.gameObject.transform.position);
            if (distance <= skillRange) // target in range
            {
                Debug.Log("Target in range");
                RaycastHit2D[] hit = Physics2D.LinecastAll(playerSkillSkript.transform.position, 
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
                    Debug.Log("Target in sight");
                    UseSkill();
                }
                else // target not in sight
                {
                    Debug.Log("Target NOT in sight");
                }
            }
            else // target not in range
            {
                Debug.Log("Target not in range: Distance " + distance + " > " + skillRange);
            }
        }
        else // for skills that don't need a target
        {
            UseSkill();
        }
    }

    public void UseSkill() // checks for time between skill (e.g. Animation, GlobalCooldown) (+ stuff)
    {
        if (!skillAnimation)
        {
            if (hasGlobalCooldown)
            {
                if (!globalCooldownActive)
                {
                    Debug.Log("Use GCSkill normal");
                    TriggerGlobalCooldown();
                    TriggerSkillAnimation();
                    TriggerOwnCooldown();
                    SkillEffect();
                    // Play normal click
                }
                else // GlobalCooldown Activ
                {
                    if (gcdTimeLeft <= gcEarlyTime && !isSkillInQueue)
                    {
                        isSkillInQueue = true;
                        Debug.Log("Global Cooldown. Waiting for GCSkill ...");
                        // Play uppitch click
                        StartCoroutine(Wait(gcdTimeLeft));
                        IEnumerator Wait(float time)
                        {
                            yield return new WaitForSeconds(time);
                            isSkillInQueue = false;
                            Debug.Log("... Use GCSkill");
                            TriggerGlobalCooldown();
                            TriggerSkillAnimation();
                            TriggerOwnCooldown();
                            SkillEffect();
                        }
                    }
                    else if (isSkillInQueue)
                    {
                        Debug.Log("ERROR GC: queue full");
                        //Play downpitch click
                    }
                    else
                    {
                        Debug.Log("ERROR GC: too early");
                        //Play downpitch click
                    }
                }
            }
            else // kein GlobalCooldown Skill
            {
                Debug.Log("Use InstantSkill normal");
                TriggerSkillAnimation();
                TriggerOwnCooldown();
                SkillEffect();
                // color skill grey -> after cooldown normal
                // Play normal click
            }
        }
        else // Animation wait active
        {
            if (!isSkillInQueue && !hasGlobalCooldown)
            {
                Debug.Log("Animation wait active ... ");
                isSkillInQueue = true;
                // Play uppitch click
                StartCoroutine(Wait(saTimeLeft));
                IEnumerator Wait(float time)
                {
                    yield return new WaitForSeconds(time);
                    isSkillInQueue = false;
                    Debug.Log("... Use InstantSkill");
                    TriggerSkillAnimation();
                    TriggerOwnCooldown();
                    SkillEffect();
                }
            }
            else if (!isSkillInQueue && hasGlobalCooldown && !globalCooldownActive) //???
            {
                Debug.Log("Animation wait active ... ");
                isSkillInQueue = true;
                // Play uppitch click
                StartCoroutine(Wait(saTimeLeft));
                IEnumerator Wait(float time)
                {
                    yield return new WaitForSeconds(time);
                    isSkillInQueue = false;
                    TriggerGlobalCooldown();
                    TriggerSkillAnimation();
                    Debug.Log("... Use GCSkill");
                    TriggerOwnCooldown();
                    SkillEffect();
                }
            }
            else if (isSkillInQueue)
            {
                Debug.Log("ERROR A: queue full;");
                //Play downpitch click
            }
            else // !skillInQueue && hasGlobalCooldown && globalCooldownActive
            {
                Debug.Log("ERROR A: GC active");
                //Play downpitch click
            }
        }
    }

    public virtual void SkillEffect() // overridden by each skill seperately
    {
    }



    void Update()
    {
        if (saTimeLeft > 0)
        {
            saTimeLeft -= Time.deltaTime;
        }
        else // time <= 0
        {
            if (skillAnimation)
            {
                skillAnimation = false;
            }
        }

        if (gcdTimeLeft > 0)
        {
            gcdTimeLeft -= Time.deltaTime;
        }
        else // time <= 0
        {
            if (globalCooldownActive)
            {
                globalCooldownActive = false;
                foreach (GameObject gObj in globalCooldownSkills) //color all CDSkills normal
                {
                    gObj.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }
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
            }
        }
    }

    GameObject[] globalCooldownSkills;
    //GameObject[] textGameObjects;

    void Awake()
    {
        globalCooldownSkills = GameObject.FindGameObjectsWithTag("GlobalCooldownSkill");
        //textGameObjects = GameObject.FindGameObjectsWithTag("WeaponSkillCDText");
    }

    public void TriggerSkillAnimation()
    {
        skillAnimation = true;
        saTimeLeft = saTime;
    }

    public void TriggerGlobalCooldown()
    {
        globalCooldownActive = true;
        gcdTimeLeft = gcdTime;
        foreach (GameObject gObj in globalCooldownSkills) //color all CDSkills grey
        {
            gObj.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
        }
    }

    public void TriggerOwnCooldown()
    {
        if (hasOwnCooldown)
        {
            ownCooldownActive = true;
            ownCooldownTimeLeft = ownCooldownTime;
        }
    }
}


// float distance2 = (playerSkillSkript.transform.position - interactionCharacter.focus.gameObject.transform.position).sqrMagnitude;
// if (distance2 <= skillRange * skillRange)