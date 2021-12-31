using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MasterSchmuff
{
    public Buff thisBuff;

    public override void BuffEffect(PlayerController playerController)
    {
        base.BuffEffect(playerController);
        playerController._Speed += 5f;
    }

    public override void RemoveBuff(PlayerController playerController)
    {
        // Entferne Buff Effekt
        playerController._Speed -= 5f;
        BuffManager.instance.Remove(thisBuff);
    }
}