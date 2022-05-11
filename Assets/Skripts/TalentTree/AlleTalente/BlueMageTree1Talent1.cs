using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMageTree1Talent1 : Talent
{
    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        statSkript.maxHealth.AddModifierAdd(20);
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();
        for (int i = 0; i < currentCount; i++)
        {
            statSkript.maxHealth.AddModifierAdd(-20);
        }
    }
}
