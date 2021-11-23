using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPrefab : MonoBehaviour
{
    public bool hasGlobalCooldown; //virtual?
    
    bool skillAnimation = false;
    public float saTime = 0.5f;
    float saTimeLeft;

    bool globalCooldownActive = false;
    public float gcdTime = 1.5f;
    public float gcEarlyTime = 0.5f;
    float gcdTimeLeft;

    bool isSkillInQueue = false;


    public void SkillStuff()
    {
        if (!skillAnimation)
        {
            if (hasGlobalCooldown)
            {
                if (!globalCooldownActive)
                {
                    globalCooldownActive = true;
                    gcdTimeLeft = gcdTime;
                    skillAnimation = true;
                    saTimeLeft = saTime;
                    Debug.Log("Use Skill normal");
                    UseSkill();
                }
                else // GlobalCooldown Activ
                {
                    if (gcdTimeLeft <= gcEarlyTime && !isSkillInQueue)
                    {
                        isSkillInQueue = true;
                        StartCoroutine(Wait(gcdTimeLeft));
                        IEnumerator Wait(float time)
                        {
                            yield return new WaitForSeconds(time);
                            globalCooldownActive = true;
                            gcdTimeLeft = gcdTime;
                            skillAnimation = true;
                            saTimeLeft = saTime;
                            Debug.Log("Use Skill eingereiht");
                            UseSkill();
                            isSkillInQueue = false;
                        }
                    }
                    else
                    {
                        Debug.Log("GC Active, zu früh, kein skill :(");
                    }
                }
            }
            else // kein GlobalCooldown Skill
            {
                UseSkill();
            }
        }
        else // Animation wait active
        {
            if (!isSkillInQueue && !hasGlobalCooldown)
            {
                Debug.Log("Animation wait active");
                isSkillInQueue = true;
                StartCoroutine(Wait(saTimeLeft));
                IEnumerator Wait(float time)
                {
                    yield return new WaitForSeconds(time);
                    UseSkill();
                    isSkillInQueue = false;
                }
            }
            else
            {
                Debug.Log("a");
            }
        }
    }

    public virtual void UseSkill()
    {
        //Debug.Log("Using " + gameObject.name);
        // Skill effect !!!!
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
            }
        }
    }
}
