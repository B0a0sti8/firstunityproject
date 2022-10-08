using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryBagScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;

    private CanvasGroup canvasGroup;

    private List<InventorySlotScript> slots = new List<InventorySlotScript>();

    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    public List<InventorySlotScript> MySlots { get => slots; }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (InventorySlotScript slot in MySlots)
            {
                if (slot.IsEmpty)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public List<Item> GetItems()
    {
        List<Item> items = new List<Item>();

        foreach (InventorySlotScript slot in slots)
        {
            if (!slot.IsEmpty)
            {
                foreach (Item item in slot.MyItems)
                {
                    items.Add(item);
                }
            }
        }

        return items;
    }

    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            InventorySlotScript slot = Instantiate(slotPrefab, transform).GetComponent<InventorySlotScript>();
            slot.MyBag = this;
            MySlots.Add(slot);
        }
    }

    public bool AddItem(Item item)
    {
        foreach (InventorySlotScript slot in MySlots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item);
                return true;
            }
        }

        return false;
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
}
