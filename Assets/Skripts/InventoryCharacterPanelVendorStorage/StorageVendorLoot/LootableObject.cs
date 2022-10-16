using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableObject : Interactable
{
    [SerializeField]private LootTable lootTable;

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Hi, ich kann gelootet werden!");
        lootTable.ShowLoot();
    }

    public override void StopInteracting()
    {
        base.Interact();
        Debug.Log("Hey, du bist zu weit weg.");
        LootWindow.MyInstance.Close();
    }
}
