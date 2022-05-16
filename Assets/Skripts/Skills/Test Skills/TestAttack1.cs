using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestAttack1 : SkillPrefab
{
    public float damage = 100f;

    public override void Start()
    {
        base.Start();
        isAOECircle = true;
        skillRadius = 10;
    }

    public override void Update()
    {
        tooltipSkillDescription = "Deal <color=orange>" + damage + " Damage</color> to any target.";

        base.Update();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        
        Debug.Log("Attack1: " + damage + " Damage");
        DealDamage(damage);
    }

    //public void DealDamage(float damage)
    //{
    //    int missRandom = Random.Range(0, 100);
    //    interactionCharacter.focus.gameObject.GetComponent<EnemyStats>().view.RPC("TakeDamage", RpcTarget.All, damage, missRandom);
    //}
}