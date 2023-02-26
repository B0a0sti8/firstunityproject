using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BagButtonScript : MonoBehaviour, IPointerClickHandler
{
    private Bag bag;

    [SerializeField]
    private Sprite full, empty;

    [SerializeField] private int bagIndex;

    HandScript myHandScript;
    InventoryScript myInventory;

    public int MyBagIndex { get => bagIndex; set => bagIndex = value; }

    public Bag MyBag
    {
        get
        {
            return bag;
        }
        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }
            bag = value;
        }
    }

    void Awake() 
    {
        myHandScript = transform.parent.parent.parent.Find("Canvas Hand").Find("Hand Image").GetComponent<HandScript>();
        myInventory = transform.parent.parent.parent.Find("Canvas Inventory").Find("Inventory").GetComponent<InventoryScript>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (myInventory.FromSlot != null && myHandScript.MyMoveable != null && myHandScript.MyMoveable is Bag)
            {
                if (MyBag != null)
                {
                    myInventory.SwapBags(MyBag, myHandScript.MyMoveable as Bag);
                }
                else
                {
                    Bag tmp = (Bag)myHandScript.MyMoveable;
                    tmp.MyBagButton = this;
                    tmp.Use();
                    MyBag = tmp;
                    myHandScript.Drop();
                    myInventory.FromSlot = null;
                }
            }
            else if (Keyboard.current.shiftKey.isPressed)
            {
                myHandScript.TakeMoveable(MyBag);
            }
            else if (bag != null)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    public void RemoveBag()
    {
        myInventory.RemoveBag(MyBag);
        MyBag.MyBagButton = null;

        foreach (Item item in MyBag.MyBagScript.GetItems())
        {
            myInventory.AddItem(item);
        }

        MyBag = null;
    }
}
