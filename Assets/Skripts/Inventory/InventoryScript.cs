using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryScript : MonoBehaviour
{
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

    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private BagButtonScript[] bagButtons;

    // For Debugging
    [SerializeField]
    private Item[] items;

    public bool CanAddBag
    {
        get { return bags.Count < 6; }
    }


    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(3);
        bag.Use();

        bag = (Bag)Instantiate(items[1]);
        bag.Initialize(6);
        bag.Use();
    }

    public void AddBag(Bag bag)
    {
        foreach (BagButtonScript bagButton in bagButtons)
        {
            if (bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                bags.Add(bag);
                break;
            }
        }
    }

    public void AddItem(Item item)
    {
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                return;
            }
        }
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
        Debug.Log("Hallo");
        HealthPotion healthPot = (HealthPotion)Instantiate(items[2]);
        AddItem(healthPot);
    }
}
