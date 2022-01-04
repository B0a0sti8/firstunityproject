using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestAttack1 : SkillPrefab
{
    public float damage = 100f;

    public override void MasterETStuff()
    {
        skillDescription = "Deal <color=orange>" + damage + " Damage</color> to any target.";

        base.MasterETStuff();
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        
        Debug.Log("Attack1: " + damage + " Damage");
        //GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += 100f; // DPS-Meter

        DealDamage(damage);
    }

    //public void DealDamage(float damage)
    //{
    //    int missRandom = Random.Range(0, 100);
    //    interactionCharacter.focus.gameObject.GetComponent<EnemyStats>().view.RPC("TakeDamage", RpcTarget.All, damage, missRandom);
    //}
}