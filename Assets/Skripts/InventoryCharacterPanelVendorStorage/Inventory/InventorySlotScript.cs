using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class InventorySlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;

    public bool onRemoved = false;

    [SerializeField]
    private Text stackSize;

    Transform ownCanvases;

    public InventoryBagScript MyBag { get; set; }

    HandScript myHandScript;
    InventoryScript myInventory;
    UIManager myUIManager;
    CharacterPanelScript myCharPanel;

    public int MyIndex { get; set; }

    public bool IsEmpty
    {
        get
        {
            return MyItems.Count == 0;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            return true;
        }
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }

            return null;
        }
    }

    public Image MyIcon 
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }
    }

    public int MyCount
    {
        get
        {
            return MyItems.Count;
        } 
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    public ObservableStack<Item> MyItems { get => items; }

    MasterEventTriggerItems masterETItems;


    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);

        masterETItems = GetComponent<MasterEventTriggerItems>();
        ownCanvases = transform.parent.parent.parent;

        if (transform.parent.name == "StorageChest")
        {
            myHandScript = ownCanvases.Find("Canvas Hand").Find("Hand Image").GetComponent<HandScript>();
            myInventory = ownCanvases.Find("Canvas Inventory").Find("Inventory").GetComponent<InventoryScript>();
            myUIManager = ownCanvases.parent.Find("GameManager").GetComponent<UIManager>();
            myCharPanel = ownCanvases.Find("CanvasCharacterPanel").Find("CharacterPanel").GetComponent<CharacterPanelScript>();
        }
        else        // Tatsächlicher Inventarslot (Keine Kiste o.ä.)
        {
            myHandScript = transform.parent.parent.parent.parent.Find("Canvas Hand").Find("Hand Image").GetComponent<HandScript>();
            myInventory = transform.parent.parent.GetComponent<InventoryScript>();
            myUIManager = transform.parent.parent.parent.parent.parent.Find("GameManager").GetComponent<UIManager>();
        }

    }

    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;

        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }

            return true;
        }

        return false;
    }

    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            MyItems.Pop();
        }
    }

    public void Clear()
    {
        int initCount = MyItems.Count;

        if (initCount > 0)
        {
            for (int i = 0; i < initCount; i++)
            {
                myInventory.OnItemCountChanged(MyItems.Pop());
            }
            
            //MyItems.Clear();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)          // Wenn Linksklick
        {
            if (myInventory.FromSlot == null && !IsEmpty)        // If we dont have something to move
            {
                if (myHandScript.MyMoveable != null)           // Wenn etwas in der Hand
                {
                    if (MyItem is Bag && myHandScript.MyMoveable is Bag)
                    {   
                         myInventory.SwapBags(myHandScript.MyMoveable as Bag, MyItem as Bag);
                    }
                    else if (myHandScript.MyMoveable is Equipment)
                    {
                        if (MyItem is Equipment && (MyItem as Equipment).MyEquipmentType == (myHandScript.MyMoveable as Equipment).MyEquipmentType)
                        {
                            (MyItem as Equipment).Equip(ownCanvases.parent.GetComponent<NetworkObject>());
                            TooltipScreenSpaceUIItems.HideTooltip_Static();
                            TooltipScreenSpaceUIItems.ShowTooltip_Static(MyItem.tooltipItemName, MyItem.tooltipItemDescription, null);
                            myHandScript.Drop();
                        }
                    }
                }
                else
                {
                    myHandScript.TakeMoveable(MyItem as IMoveable);
                    myInventory.FromSlot = this;
                    onRemoved = true;
                }
            }
            else if (myInventory.FromSlot == null && IsEmpty)
            {
                if (myHandScript.MyMoveable is Bag)
                {
                    Bag bag = (Bag)myHandScript.MyMoveable;

                    if (bag.MyBagScript != MyBag && myInventory.MyEmptySlotCount - bag.MySlotCount > 0)
                    {
                        AddItem(bag);
                        bag.MyBagButton.RemoveBag();
                        myHandScript.Drop();
                    }
                }
                else if (myHandScript.MyMoveable is Equipment)
                {
                    Equipment equipment = (Equipment)myHandScript.MyMoveable;
                    AddItem(equipment);
                    myCharPanel.MySelectedButton.DequipStuff();
                    myHandScript.Drop();
                    Debug.Log(IsEmpty);
                    equipment.MySlot = this;
                }


            }
            else if (myInventory.FromSlot != null)               // If something in hand
            {
                if (PutItemBack() || MergeItems(myInventory.FromSlot) || SwapItems(myInventory.FromSlot) || AddItems(myInventory.FromSlot.MyItems))
                {
                    myHandScript.Drop();
                    myInventory.FromSlot = null;
                    onRemoved = true;
                }
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right && myHandScript.MyMoveable == null)
        {
            UseItem();
            TooltipScreenSpaceUIItems.HideTooltip_Static(); // Refresht Tooltipp
            if (!IsEmpty)
            {

                TooltipScreenSpaceUIItems.ShowTooltip_Static(MyItem.tooltipItemName, MyItem.tooltipItemDescription, null);
            }
        }
    }

    public void UseItem()
    {
        if (MyItem != null)
        {
            MyItem.Use();
        }
        //else if (MyItem is Equipment)
        //{
        //    (MyItem as Equipment).Equip();
        //}
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }

        return false;
    }

    private bool PutItemBack()
    {
        if (myInventory.FromSlot == this)
        {
            myInventory.FromSlot.MyIcon.color = Color.white;
            return true;
        }

        return false;
    }

    private bool SwapItems(InventorySlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }

        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            // Copy all items we need to swap from A
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            
            from.MyItems.Clear();     // Clear Slot A
            from.AddItems(MyItems);   // All items from Slot B and copy to A

            MyItems.Clear();          // Clear Slot B
            AddItems(tmpFrom);      // All items from Slot A copy to B

            return true;
        }

        return false;
    }

    private bool MergeItems(InventorySlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }

        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            int free = MyItem.MyStackSize - MyCount;
            int itemLeftToStack = from.MyItem.MySlot.MyCount;

            for (int i = 0; i < free; i++)
            {
                if (itemLeftToStack != 0)
                {
                    AddItem(from.MyItems.Pop());
                    itemLeftToStack--;
                }
            }

            return true;
        }

        return false;
    }

    private void UpdateSlot()
    {
        myUIManager.UpdateStackSize(this);
    }

    public void MasterETStuffAssignment()
    {
        masterETItems.itemName = MyItem.tooltipItemName;

        masterETItems.itemDescription = MyItem.tooltipItemDescription;

        //masterETItems.buffSprite = Resources.Load<Sprite>("Sprites/BuffSprites/" + buffName);
    }

    private void Update()
    {
         if (IsEmpty)
         {
            if (onRemoved)
            {
                onRemoved = false;
                masterETItems.itemName = "";
                masterETItems.itemDescription = "";
            }

             return;
         }

        MasterETStuffAssignment();
    }
}
