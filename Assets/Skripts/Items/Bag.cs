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

    InventoryScript myInventory;

    public BagButtonScript MyBagButton { get; set; }

    public int MySlotCount { get => slots; set => slots = value; }


    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    public override void Use()
    {
        base.Use();

        myInventory = user.transform.Find("Own Canvases").Find("Canvas Inventory").Find("Inventory").GetComponent<InventoryScript>(); 

        if (myInventory.CanAddBag)
        {
            MyBagScript = Instantiate(bagPrefab, myInventory.transform).GetComponent<InventoryBagScript>();
            MyBagScript.AddSlots(slots);

            if (MyBagButton == null)
            {
                myInventory.AddBag(this);
            }
            else
            {
                myInventory.AddBag(this, MyBagButton);
            }

            Remove();
        }
    }

    public void SetupScript(InventoryScript myInven)
    {
        MyBagScript = Instantiate(bagPrefab, myInven.transform).GetComponent<InventoryBagScript>();
        MyBagScript.AddSlots(slots);
    }

    public override void Awake()
    {
        base.Awake();
        tooltipItemName = "Bag";
        tooltipItemDescription = "X Item Slots";
    }

}
