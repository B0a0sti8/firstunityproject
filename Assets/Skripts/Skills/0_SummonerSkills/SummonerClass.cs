using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerClass : MonoBehaviour
{
    Transform PLAYER;

    public float ExplodingImpsDamageModifier;
    public float minionSummoned_CastSpeedModifier;
    public bool hasCastSpeedOnMinionSummonedTalent;


    private void Awake()
    {
        ExplodingImpsDamageModifier = 1;
        PLAYER = transform.parent.parent;
        hasCastSpeedOnMinionSummonedTalent = false;
    }

    public void SummonerClass_OnMinionSummoned()
    {
        //Debug.Log("Has Summoned Minion");
        if (hasCastSpeedOnMinionSummonedTalent)
        {
            PLAYER.GetComponent<PlayerStats>().actionSpeed.AddModifierAdd(minionSummoned_CastSpeedModifier);
            StartCoroutine(CastSpeedForminionSummonedStop(10, minionSummoned_CastSpeedModifier));
        }
    }

    public IEnumerator CastSpeedForminionSummonedStop(float time, float mod)
    {
        //Debug.Log("Has Summoned Minion. Removing Buff");

        yield return new WaitForSeconds(time);
        PLAYER.GetComponent<PlayerStats>().actionSpeed.RemoveModifierAdd(mod);
    }
}
