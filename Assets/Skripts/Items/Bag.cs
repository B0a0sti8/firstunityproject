using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item
{
    private int slots;

    [SerializeField]
    private GameObject bagPrefab;

    public InventoryBagScript MyBagScript { get; set; }
    public int Slots { get => slots; set => slots = value; }


    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    public override void Use()
    {
        if (InventoryScript.MyInstance.CanAddBag)
        {
            MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<InventoryBagScript>();
            MyBagScript.AddSlots(slots);
            InventoryScript.MyInstance.AddBag(this);
            Remove();
        }
    }
}
