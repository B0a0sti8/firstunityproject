using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;

    public bool onRemoved = false;

    [SerializeField]
    private Text stackSize;

    public InventoryBagScript MyBag { get; set; }

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
                InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
            }
            
            //MyItems.Clear();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)          // Wenn Linksklick
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty)        // If we dont have something to move
            {
                if (HandScript.MyInstance.MyMoveable != null)           // Wenn etwas in der Hand
                {
                    if (MyItem is Bag && HandScript.MyInstance.MyMoveable is Bag)
                    {
                        InventoryScript.MyInstance.SwapBags(HandScript.MyInstance.MyMoveable as Bag, MyItem as Bag);
                    }
                    else if (HandScript.MyInstance.MyMoveable is Equipment)
                    {
                        if (MyItem is Equipment && (MyItem as Equipment).MyEquipmentType == (HandScript.MyInstance.MyMoveable as Equipment).MyEquipmentType)
                        {
                            (MyItem as Equipment).Equip();
                            TooltipScreenSpaceUIItems.HideTooltip_Static();
                            TooltipScreenSpaceUIItems.ShowTooltip_Static(MyItem.tooltipItemName, MyItem.tooltipItemDescription, null);
                            HandScript.MyInstance.Drop();
                        }
                    }
                }
                else
                {
                    HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                    InventoryScript.MyInstance.FromSlot = this;
                    onRemoved = true;
                }
            }
            else if (InventoryScript.MyInstance.FromSlot == null && IsEmpty)
            {
                if (HandScript.MyInstance.MyMoveable is Bag)
                {
                    Bag bag = (Bag)HandScript.MyInstance.MyMoveable;

                    if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.MySlotCount > 0)
                    {
                        AddItem(bag);
                        bag.MyBagButton.RemoveBag();
                        HandScript.MyInstance.Drop();
                    }
                }
                else if (HandScript.MyInstance.MyMoveable is Equipment)
                {
                    Equipment equipment = (Equipment)HandScript.MyInstance.MyMoveable;
                    AddItem(equipment);
                    CharacterPanelScript.MyInstance.MySelectedButton.DequipStuff();
                    HandScript.MyInstance.Drop();
                    Debug.Log(IsEmpty);
                    equipment.MySlot = this;
                }


            }
            else if (InventoryScript.MyInstance.FromSlot != null)               // If something in hand
            {
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) || SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                    onRemoved = true;
                }
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right && HandScript.MyInstance.MyMoveable == null)
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
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
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
        UIManager.MyInstance.UpdateStackSize(this);
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
