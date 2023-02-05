using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCrabSuplex : EnemySkillPrefab
{
    [SerializeField] Animator crabSuplexAnimator; 
    // Start is called before the first frame update
    void Start()
    {
        crabSuplexAnimator = transform.parent.Find("CrabBoss").GetComponent<Animator>();
        cooldown = 3;
        duration = 1f;
        animationDuration = 2;
        range = 5f;
        //radius = 2f;
        //baseDamage = 200;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (startedAnimation)
        {
            crabSuplexAnimator.SetFloat("SuplexSpeed", 1f);
            crabSuplexAnimator.SetBool("IsSuplexing", true);
            startedAnimation = false;
        }

        if (endedAnimation)
        {
            crabSuplexAnimator.SetBool("IsSuplexing", false);
            endedAnimation = false;
        }
        base.Update();

    }

    public override void AtSkillStart()
    {
        // Fange an den Spieler über dich drüber zu bewegen.
        base.AtSkillStart();
    }

    public override void SkillEffect()
    {
        // Deal Damage to Player.

        base.SkillEffect();
    }

}
