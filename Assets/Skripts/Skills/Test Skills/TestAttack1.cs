using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestAttack1 : SkillPrefab
{
    public override void SkillEffect()
    {
        base.SkillEffect();
        // Play Animation
        // Play Soundeffect
        // Skilleffect
        Debug.Log("Attack1");
        GameObject.Find("Canvas Damage Meter").GetComponent<DamageMeter>().totalDamage += 100f; // DPS-Meter

        //interactionCharacter.focus.gameObject.GetComponent<EnemyStats>().TakeDamage(100);
        interactionCharacter.focus.gameObject.GetComponent<EnemyStats>().GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, 100f);
    }
}