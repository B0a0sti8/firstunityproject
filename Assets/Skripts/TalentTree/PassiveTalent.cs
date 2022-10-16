using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PassiveTalent : MonoBehaviour
{
    private Image sprite;
    public bool effectActive = false;

    private void Awake()
    {
        sprite = GetComponent<Image>();
    }

    public void Lock()
    {
        sprite.color = Color.grey;
        effectActive = false;
    }

    public void Unlock()
    {
        sprite.color = Color.white;
        effectActive = true;
    }

    public virtual void PassiveTalentEffect()
    {

    }
}
