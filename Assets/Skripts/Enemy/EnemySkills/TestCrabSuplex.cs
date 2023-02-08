using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCrabSuplex : EnemySkillPrefab
{
    [SerializeField] Animator crabSuplexAnimator;

    Transform skillTarget;
    float SuplexVelocity;
    Coroutine supl;

    Coroutine camShake;
    CameraShaker camMam;

    [SerializeField] ShakeData shakeData;
    // Start is called before the first frame update
    void Start()
    {
        crabSuplexAnimator = transform.parent.Find("CrabBoss").GetComponent<Animator>();
        cooldown = 2;
        duration = 1f;
        animationDuration = 1;
        range = 3f;
        //radius = 2f;
        baseDamage = 40;
        SuplexVelocity = 50;
        camMam = GameObject.Find("CameraMama").transform.Find("Main Camera").GetComponent<CameraShaker>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (startedAnimation)
        {
            crabSuplexAnimator.SetFloat("SuplexSpeed", 5f);
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
        skillTarget = GetComponentInParent<EnemyAI>().target;
        DamageOrHealing.DealDamage(transform.parent.gameObject, skillTarget.gameObject, baseDamage / 2);

        skillTarget.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (skillTarget.tag == "Player")
        {
            skillTarget.GetComponent<PlayerController>().enabled = false;
            skillTarget.rotation *= Quaternion.Euler(0, 0, 90);
        }

        supl = StartCoroutine(SuplexMechanik(0.2f, 0.05f, 0.5f));
        camShake = StartCoroutine(CameraShaking(0.25f));


        // Fange an den Spieler über dich drüber zu bewegen.
        base.AtSkillStart();
    }

    public override void SkillEffect()
    {
        // Deal Damage to Player.
        
        base.SkillEffect();
    }

    IEnumerator SuplexMechanik(float start, float end, float end2)
    {
        yield return new WaitForSeconds(start);
        Vector2 direction = (transform.parent.position - skillTarget.position).normalized;
        skillTarget.GetComponent<Rigidbody2D>().velocity = direction * SuplexVelocity;
        yield return new WaitForSeconds(end);
        skillTarget.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(end2);
        skillTarget.rotation *= Quaternion.Euler(0, 0, -90);
        skillTarget.GetComponent<PlayerController>().enabled = true;
    }

    IEnumerator CameraShaking(float start)
    {
        yield return new WaitForSeconds(start);
        camMam.Shake(shakeData);
        if (skillTarget.tag == "Player")
        {
            skillTarget.transform.Find("PlayerParticleSystems").Find("SmashedIntoTheGroundParticles").gameObject.GetComponent<ParticleSystem>().Play();
        }
        
        Debug.Log("Shake!");
    }

}
