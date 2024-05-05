using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMageTree1Talent1 : Talent
{
    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        if (currentCount >= 2)
        {
            statSkript.maxHealth.RemoveModifierMultiply(1.0f + 0.1f * (currentCount-1));
        }
        statSkript.maxHealth.AddModifierMultiply(1.0f + 0.1f * currentCount);
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();
        if (currentCount >= 1)
        {
            statSkript.maxHealth.RemoveModifierMultiply(1.0f + 0.1f * currentCount);
        }
    }
}
