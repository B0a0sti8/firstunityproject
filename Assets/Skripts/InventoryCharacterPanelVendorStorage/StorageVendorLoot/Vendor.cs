using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : Interactable
{
    public bool IsOpen { get; set; }

    [SerializeField] private VendorItem[] items;

    public VendorWindow vendorWindow;
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Hallo pleb. Ich bin hier der Chad. Jetzt gib mir dein Geld.");

        if (!IsOpen)
        {
            IsOpen = true;
            vendorWindow.CreatePages(items);
            vendorWindow.Open(this);
        }  
    }

    public override void StopInteracting()
    {
        base.StopInteracting();
        if (IsOpen)
        {
            IsOpen = false;
            vendorWindow.Close();
        }
    }
}
