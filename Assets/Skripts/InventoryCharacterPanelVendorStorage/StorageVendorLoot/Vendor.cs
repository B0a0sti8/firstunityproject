using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : NPC
{
    [SerializeField] private VendorItem[] items;

    public VendorItem[] MyItems { get => items; }

    public override void Interact()
    {
        base.Interact();
    }

    public override void StopInteracting()
    {
        base.StopInteracting();
    }
}
