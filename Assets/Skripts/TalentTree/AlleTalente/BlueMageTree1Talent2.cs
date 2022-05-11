using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMageTree1Talent2 : Talent
{
    public override void ActiveTalentEffect()
    {
        base.ActiveTalentEffect();
        statSkript.movementSpeed.AddModifierMultiply(1.1f);
    }

    public override void RemoveActiveTalentEffect()
    {
        base.RemoveActiveTalentEffect();
        for (int i = 0; i < currentCount; i++)
        {
            statSkript.movementSpeed.AddModifierMultiply(1/1.1f);
        }
    }
}
