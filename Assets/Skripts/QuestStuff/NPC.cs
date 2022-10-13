using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    [SerializeField] public UIWindowNPC window;
    
    public bool IsInteracting { get; set; }

    public override void Interact()
    {
        base.Interact();
        if (!IsInteracting)
        {
            IsInteracting = true;
            window.Open(this);
        }
    }

    public override void StopInteracting()
    {
        base.StopInteracting();
        if (IsInteracting)
        {
            IsInteracting = false;
            window.Close();
        }
    }
}
