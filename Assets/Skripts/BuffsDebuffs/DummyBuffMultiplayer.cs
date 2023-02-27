using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DummyBuffMultiplayer : Buff
{
    public string spriteName;

    public new string buffName = "";
    public new string buffDescription = "";
    public string buffSpriteName = "";
    public new float duration = 0f;
    public override Buff Clone()
    {
        DummyBuffMultiplayer clone = (DummyBuffMultiplayer)this.MemberwiseClone();
        return clone;
    }

    public void StartTicking(float tickTime)
    {
        durationTimeLeft -= tickTime;
    }
}
