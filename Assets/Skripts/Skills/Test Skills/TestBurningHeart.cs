using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestBurningHeart : SkillPrefab
{
    public float totalHeal;
    public float healTick;
    private bool skillEffektActive = false;
    private int n;
    private int nMax;

    public override void Start()
    {
        base.Start();
        castTimeOriginal = 5;
        isSkillChanneling = true;
        totalHeal = 200f;
        nMax = 50;
        n = 0;
        isSelfCast = true;
    }

    public override void Update()
    {
        tooltipSkillDescription = "Heal <color=orange>" + totalHeal + " over <color=orange>" + castTimeOriginal + " s.";

        base.Update();
        if (masterChecks.isSkillInterrupted)
        { skillEffektActive = false; }

        if (masterChecks.masterIsCastFinished && skillEffektActive)
        { skillEffektActive = false; masterChecks.masterIsCastFinished = false;  return; }

        if (skillEffektActive)
        {
            n += 1;
            if (n >= nMax)
            {
                n = 0;
                healTick = totalHeal * nMax * Time.deltaTime / castTimeModified;
                Debug.Log(Time.deltaTime); Debug.Log(nMax); Debug.Log(castTimeModified); Debug.Log(totalHeal);
                DoHealing(healTick);
            }
        }
        else
        {
            n = 0;
        }
    }

    public override void SkillEffect()
    {
        base.SkillEffect();
        skillEffektActive = true;
    }
}
