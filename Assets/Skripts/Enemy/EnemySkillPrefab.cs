using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySkillPrefab : MonoBehaviour
{
    float cooldown;
    float remainingCD;
    public float duration;
    bool skillReady = true;

    public bool CastSkill()
    {
        if (skillReady)
        {
            SkillEffect();
            skillReady = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void SkillEffect()
    {

    }

    private void Update()
    {
        if (!skillReady)
        {
            if (remainingCD > 0)
            { remainingCD -= Time.deltaTime; }
            else
            { remainingCD = cooldown; skillReady = true; }
        }
    }
}
