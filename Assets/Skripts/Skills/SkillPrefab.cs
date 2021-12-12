using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //unnötig?
using Photon.Pun;

public class SkillPrefab : MonoBehaviour
{
    public MasterChecks masterChecks;

    public GameObject PLAYER; // drag in player to acces player (for position in rangeCheck)
    //[HideInInspector]
    //public Player player; // drag in Character to access Player skript (for health)
    [HideInInspector]
    public PlayerController playerController; // drag in Character to access PlayerController skript (for speed)
    [HideInInspector]
    public InteractionCharacter interactionCharacter; // focus skript
    [HideInInspector]
    public PlayerStats playerStats;

    public bool needsTargetEnemy;
    public bool needsTargetAlly;

    public bool needsMana; // optional
    public int manaCost;

    public float skillRange;
    bool targetInSight;

    public bool hasOwnCooldown;
    public float ownCooldownTime; // 0 if hasOwnCooldown = false
    float ownCooldownTimeLeft;
    bool ownCooldownActive = false;

    public bool hasGlobalCooldown;

    public bool isSuperInstant; // can not be true if hasGlobalCooldown is true
    bool isSkillInOwnSuperInstantQueue = false;



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
            Debug.Log("Wait for OwnCooldown ...  " + ownCooldownTimeLeft);
            StartCoroutine(Wait(ownCooldownTimeLeft));
            IEnumerator Wait(float time)
            {
                isSkillInOwnSuperInstantQueue = true;
                if (time > 0) { FindObjectOfType<AudioManager>().Play("HoverClickUpPitch"); }
                else { FindObjectOfType<AudioManager>().Play("HoverClick"); }
                yield return new WaitForSeconds(time);
                isSkillInOwnSuperInstantQueue = false;
                Debug.Log("... Use SuperInstant");
                ownCooldownActive = true;
                ownCooldownTimeLeft = ownCooldownTime;
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
            Debug.Log("Wait for OwnCooldown / Animation ...  " + ownCooldownTimeLeft + " / " + masterChecks.masterAnimTimeLeft);
            StartCoroutine(WaitForSkill(Mathf.Max(ownCooldownTimeLeft, masterChecks.masterAnimTimeLeft)));
        }
        else if (masterChecks.masterGCTimeLeft <= masterChecks.masterGCEarlyTime) // GC early cast
        {
            Debug.Log("Wait for OwnCooldown / GlobalCooldown / Animation ...  " + ownCooldownTimeLeft + " / " + masterChecks.masterGCTimeLeft + " / " + masterChecks.masterAnimTimeLeft);
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
        Debug.Log("... Use Skill");
        TriggerSkill();
    }

    public void TriggerSkill()
    {
        // TriggerGlobalCooldown();
        if (hasGlobalCooldown)
        {
            masterChecks.masterGCActive = true;
            masterChecks.masterGCTimeLeft = masterChecks.masterGCTime;
            foreach (GameObject gObj in globalCooldownSkills) //color all CDSkills grey
            {
                gObj.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
            }
        }

        // TriggerSkillAnimation();
        masterChecks.masterAnimationActive = true;
        masterChecks.masterAnimTimeLeft = masterChecks.masterAnimTime;

        // TriggerOwnCooldown();
        if (hasOwnCooldown)
        {
            ownCooldownActive = true;
            ownCooldownTimeLeft = ownCooldownTime;
        }

        if (needsMana)
        {
            //playerStats.currentMana -= manaCost;
            //playerStats.RemoveMana(manaCost);
            playerStats.GetComponent<PhotonView>().RPC("ManageMana", RpcTarget.All, -manaCost);
        }
        SkillEffect();
    }

    public virtual void SkillEffect() // overridden by each skill seperately
    {
        
    }



    void Update()
    {
        if (ownCooldownTimeLeft > 0)
        {
            ownCooldownTimeLeft -= Time.deltaTime;
            gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = Mathf.Round(ownCooldownTimeLeft).ToString();
            gameObject.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
        }
        else
        {
            if (ownCooldownActive)
            {
                ownCooldownActive = false;
                gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "";
                gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                ownCooldownTimeLeft = 0;
            }
        }
    }

    GameObject[] globalCooldownSkills;
    //GameObject[] textGameObjects;

    void Awake()
    {
        masterChecks = GameObject.Find("Canvas Action Skills").GetComponent<MasterChecks>();

        PLAYER = gameObject.transform.parent.gameObject.transform.parent.gameObject; //GameObject.Find("PLAYER");
        Debug.Log(gameObject.transform.parent.gameObject.transform.parent.gameObject);
        interactionCharacter = PLAYER.GetComponent<InteractionCharacter>();
        //player = PLAYER.GetComponent<Player>();
        playerController = PLAYER.GetComponent<PlayerController>();

        playerStats = PLAYER.GetComponent<PlayerStats>();

        globalCooldownSkills = GameObject.FindGameObjectsWithTag("GlobalCooldownSkill");
        //textGameObjects = GameObject.FindGameObjectsWithTag("WeaponSkillCDText");
    }







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


    //public void TriggerSkillAnimation()
    //{
    //    //skillAnimationActive = true;
    //    masterChecks.masterAnimationActive = true;
    //    //saTimeLeft = saTime;
    //    masterChecks.masterAnimTimeLeft = masterChecks.masterAnimTime;
    //}

    //public void TriggerGlobalCooldown()
    //{
    //    if (hasGlobalCooldown)
    //    {
    //        //globalCooldownActive = true;
    //        masterChecks.masterGCActive = true;
    //        //gcdTimeLeft = gcdTime;
    //        masterChecks.masterGCTimeLeft = masterChecks.masterGCTime;
    //        foreach (GameObject gObj in globalCooldownSkills) //color all CDSkills grey
    //        {
    //            gObj.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
    //        }
    //    }
    //}

    //public void TriggerOwnCooldown()
    //{
    //    if (hasOwnCooldown)
    //    {
    //        ownCooldownActive = true;
    //        ownCooldownTimeLeft = ownCooldownTime;
    //    }
    //}
}



//void Update()
//{
        //if (saTimeLeft > 0)
        //{
        //    saTimeLeft -= Time.deltaTime;
        //}
        //else // time <= 0
        //{
        //    if (skillAnimationActive)
        //    {
        //        skillAnimationActive = false;
        //        masterChecks.masterSkillAnimationActive = false;
        //    }
        //}

//if (gcdTimeLeft > 0)
//{
//    gcdTimeLeft -= Time.deltaTime;
//}
//else // time <= 0
//{
//    if (globalCooldownActive)
//    {
//        globalCooldownActive = false;
//        masterChecks.masterGlobalCooldownActive = false;
//        foreach (GameObject gObj in globalCooldownSkills) //color all CDSkills normal
//        {
//            gObj.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
//        }
//    }
//}

// float distance2 = (playerSkillSkript.transform.position - interactionCharacter.focus.gameObject.transform.position).sqrMagnitude;
// if (distance2 <= skillRange * skillRange)