using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDragonling : SkillPrefab
{
    public override void Update()
    {
        tooltipSkillDescription = "Deal <color=orange>"  + " Damage</color> to any target.";

        base.Update();
    }
}
