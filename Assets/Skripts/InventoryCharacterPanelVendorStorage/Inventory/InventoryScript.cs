using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void ItemCountChanged(Item item);

public class InventoryScript : MonoBehaviour
{
    #region Singleton
    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }
    #endregion

    [SerializeField] private InventorySlotScript fromSlot;

    private List<Bag> bags = new List<Bag>();

    public event ItemCountChanged itemCountChangedEvent;

    [SerializeField]
    private BagButtonScript[] bagButtons;

    // For Debugging
    [SerializeField]
    private Item[] items;

    public bool CanAddBag
    {
        get { return bags.Count < 6; }
    }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MyEmptySlotCount;
            }

            return count;
        }
    }

    public int MyTotalSlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MySlots.Count;
            }

            return count;
        }
    }

    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount;
        }
    }

    public InventorySlotScript FromSlot 
    {
        get
        {
            return fromSlot;
        }

        set
        {
            fromSlot = value;
            if (value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
    }

    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(20);
        bag.Use();

        bag = (Bag)Instantiate(items[1]);
        bag.Initialize(6);
        bag.Use();

        bag = (Bag)Instantiate(items[1]);
        bag.Initialize(6);
        AddItem(bag);

        bag = (Bag)Instantiate(items[1]);
        bag.Initialize(12);
        AddItem(bag);


    }

    public void AddBag(Bag bag)
    {
        foreach (BagButtonScript bagButton in bagButtons)
        {
            if (bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                bags.Add(bag);
                bag.MyBagButton = bagButton;
                bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
                break;
            }
        }
    }

    public void AddBag(Bag bag, BagButtonScript bagButton)
    {
        bags.Add(bag);
        bagButton.MyBag = bag;
        bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
    }

    public void RemoveBag(Bag bag)
    {
        bags.Remove(bag);
        Destroy(bag.MyBagScript.gameObject);
    }

    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount = (MyTotalSlotCount - oldBag.Slots) + newBag.Slots;

        if (newSlotCount - MyFullSlotCount >= 0)
        {
            // Do Swap
            List<Item> bagItems = oldBag.MyBagScript.GetItems();

            RemoveBag(oldBag);

            newBag.MyBagButton = oldBag.MyBagButton;
            newBag.Use();

            foreach (Item item in bagItems)
            {
                if (item != newBag) // No dublicates of the bag
                {
                    AddItem(item);
                }
            }

            AddItem(oldBag);

            HandScript.MyInstance.Drop();
            MyInstance.fromSlot = null;
        }
    }

    public bool AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return true;
            }
        }

        return PlaceInEmpty(item);
    }

    public bool PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                OnItemCountChanged(item);
                return true;
            }
        }

        return false;
    }

    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in bags)
        {
            foreach (InventorySlotScript slots in bag.MyBagScript.MySlots)
            {
                if (slots.StackItem(item))
                {
                    OnItemCountChanged(item);
                    return true;
                }
            }
        }

        return false;
    }

    public void OpenClose()
    {
        bool closedBag = bags.Find(x => !x.MyBagScript.IsOpen);
        //if true: Open all closed bags
        //if false: close all open bags

        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.IsOpen != closedBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    private void Update()
    {
        
    }

    void OnAddBag()
    {
        Equipment helmet = (Equipment)Instantiate(items[3]);
        AddItem(helmet);

        Equipment helmet1 = (Equipment)Instantiate(items[4]);
        AddItem(helmet1);

        HealthPotion healthPot = (HealthPotion)Instantiate(items[2]);
        AddItem(healthPot);
    }

    public int GetItemCount(string type)
    {
        int itemCount = 0;
        foreach (Bag bag in bags)
        {
            foreach (InventorySlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.name.ToLower() == type.ToLower())
                {
                    itemCount += slot.MyItems.Count;
                }
            }
        }

        return itemCount;
    }

    public Stack<Item> GetItems(string type, int count)
    {
        Stack<Item> items = new Stack<Item>();

        foreach (Bag bag in bags)
        {
            foreach (InventorySlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.name.ToLower() == type.ToLower())
                {
                    foreach (Item item in slot.MyItems)
                    {
                        items.Push(item);

                        if (items.Count == count)
                        {
                            return items;
                        }
                    }
                }
            }
        }

        return items;
    }

    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }

}
