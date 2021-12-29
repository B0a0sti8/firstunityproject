using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestAttack1 : SkillPrefab
{
    public override void MasterETStuff()
    {
        skillDescription = "Test Attack 1";
        base.MasterETStuff();
    }

    //public override void Start()
    //{
    //    skillDescription = "Test Attack 1";

    //    base.Start();
    //}

    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        Debug.Log("Attack1");
        GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += 100f; // DPS-Meter

        int missRandom = Random.Range(0, 100);
        interactionCharacter.focus.gameObject.GetComponent<EnemyStats>().view.RPC("TakeDamage", RpcTarget.All, 100f, missRandom);
    }
}