using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPrefab : MonoBehaviour
{
    public GameObject playerSkillSkript;

    public bool hasGlobalCooldown;

    public bool needsTargetEnemy;
    public bool needsTargetAlly;
    public InteractionCharacter interactionCharacter;

    public float skillRange;

    bool skillAnimation = false;
    public float saTime = 0.5f; // NOTE: similar to gcEarlyTime and NOT gcdTime
    float saTimeLeft;

    bool globalCooldownActive = false;
    public float gcdTime = 1.5f;
    public float gcEarlyTime = 0.5f;
    float gcdTimeLeft;

    bool isSkillInQueue = false;

    // 1. Vorraussetzungen (Mana, Aufladungen, ...)
    // 2. Target
    // 3. Range + Line of sight
    // 4. Animation + Cooldown

    public void ConditionCheck()
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
                    Debug.Log(interactionCharacter.focus.gameObject.layer);
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

    public void RangeCheck() // check for range
    {
        if (needsTargetEnemy || needsTargetAlly)
        {
            float distance = Vector2.Distance(playerSkillSkript.transform.position, interactionCharacter.focus.gameObject.transform.position);
            if (distance <= skillRange)
            {
                Debug.Log("Target in range");
            }
            else
            {
                Debug.Log("Target not in range");
            }
        }
        else // for skills that don't need a target
        {
            UseSkill();
        }
        // Line of sight
    }

    // BaseCooldownCheck needed!
    // andere voraussetzungen
    public void UseSkill() // checks for time between skill (e.g. GlobalCooldown) (+ stuff)
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
                // unterscheidung normaler Cooldown !!
                Debug.Log("Use InstantSkill normal");
                TriggerSkillAnimation();
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
}
