using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySkillPrefab : MonoBehaviour
{
    public float cooldown = 0;
    public float remainingCD;

    public Coroutine isInCast;

    public Coroutine isInAnimation;
    public float animationDuration = 0;
    public bool startedAnimation = false;
    public bool endedAnimation = false;

    public bool skillReady = true;
    public float duration;
    public float range = 0;
    public float radius = 0;
    public float baseDamage = 0;
    public float baseHealing = 0;

    public void CastSkill()
    {
        isInCast = StartCoroutine(CastDelay(duration));
        isInAnimation = StartCoroutine(AnimationDelay(animationDuration));
        this.skillReady = false;
        Debug.Log("Casting Skill");
    }

    public virtual void AtSkillStart()
    {

    }

    public virtual void SkillEffect()
    {

    }

    public virtual void Update()
    {
        if (!this.skillReady)
        {
            if (this.remainingCD > 0)
            { this.remainingCD -= Time.deltaTime; }
            else
            { this.remainingCD = this.cooldown; this.skillReady = true; }
        }
    }

    IEnumerator CastDelay(float delayTime)
    {
        AtSkillStart();
        yield return new WaitForSeconds(delayTime);
        SkillEffect();
    }

    IEnumerator AnimationDelay(float delayTime)
    {
        Debug.Log("Starting Animation");
        startedAnimation = true;
        yield return new WaitForSeconds(delayTime);
        endedAnimation = true;
        Debug.Log("Ending Animation");
    }

}
