using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item
{
    [SerializeField] private int slots;

    [SerializeField]
    private GameObject bagPrefab;

    public InventoryBagScript MyBagScript { get; set; }

    public BagButtonScript MyBagButton { get; set; }

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

            if (MyBagButton == null)
            {
                InventoryScript.MyInstance.AddBag(this);
            }
            else
            {
                InventoryScript.MyInstance.AddBag(this, MyBagButton);
            }

            Remove();
        }
    }
    public override void Awake()
    {
        base.Awake();
        tooltipItemName = "Bag";
        tooltipItemDescription = "X Item Slots";
    }

}
