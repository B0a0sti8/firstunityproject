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
        lootTable.ShowLoot(player.Find("Own Canvases").Find("CanvasLootWindow").Find("LootWindow").GetComponent<LootWindow>());
    }

    public override void StopInteracting()
    {
        base.Interact();
        Debug.Log("Hey, du bist zu weit weg.");
        player.Find("Own Canvases").Find("CanvasLootWindow").Find("LootWindow").GetComponent<LootWindow>().Close();
    }
}
