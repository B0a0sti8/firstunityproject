using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TestRangeEnemyAoE : EnemySkillPrefab
{
    Vector3 tarPos;
    [SerializeField] GameObject AoE, myAoE;
    List<GameObject> myTargets = new List<GameObject>();

    public Sprite buffImage;
    TheWayOfTheChickenDamageDebuff buff = new TheWayOfTheChickenDamageDebuff();

    //float baseDotDamage;
    //float dotDuration;

    private void Start()
    {
        cooldown = 10f;
        duration = 2f;
        range = 10f;
        radius = 2f;
        baseDamage = 200;
        //baseDotDamage = 20;
        //dotDuration = 10;
    }

    public override void AtSkillStart()
    {
        tarPos = GetComponentInParent<EnemyAI>().target.transform.position;
        myAoE = Instantiate(AoE, tarPos, Quaternion.identity); // Erstelle eine rote / orangene Fläche unter dem Spieler
        myAoE.transform.localScale *= radius;
        myAoE.GetComponent<AoESpellIndicator>().duration = 5;

        base.AtSkillStart();
    }

    public override void SkillEffect()
    {
        Destroy(myAoE);
        Collider2D[] hit = Physics2D.OverlapCircleAll(tarPos, radius, (1 << LayerMask.NameToLayer("Action")) | (1 << LayerMask.NameToLayer("Ally")));

        Buff clone = buff.Clone();
        clone.buffSource = transform.parent.gameObject;

        foreach (Collider2D coll in hit)
        { myTargets.Add(coll.gameObject); }

        foreach (GameObject tar in myTargets)
        {
            DamageOrHealing.DealDamage(transform.parent.gameObject.GetComponent<NetworkBehaviour>(), tar.GetComponent<NetworkBehaviour>(), baseDamage);
            //tar.GetComponent<BuffManager>().AddBuff(clone, buffImage, dotDuration, 1f, baseDotDamage);
        }
        myTargets.Clear();

        base.SkillEffect();
    }
}
