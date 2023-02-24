using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorButton : MonoBehaviour, IPointerClickHandler
{
    private static Dictionary<ItemQuality, string> nameColors = new Dictionary<ItemQuality, string>()
    {{ ItemQuality.Common, "#ECEBEB8A"},{ ItemQuality.Uncommon, "#05CB198A"},{ ItemQuality.Rare, "#141FC38A"},{ ItemQuality.Epic, "#B512A78A"},{ ItemQuality.Mythic, "#FDB9478A"}};

    MasterEventTriggerItems masterETItems;

    [SerializeField] private Image icon;
    [SerializeField] private Text title;
    [SerializeField] private Text price;
    [SerializeField] private Text quantity;

    private VendorItem vendorItem1;
    public bool onRemoved = false;
    int goldAmount1;
    InventoryScript myInventory;

    private void Awake()
    {
        masterETItems = GetComponent<MasterEventTriggerItems>();
        goldAmount1 = transform.parent.parent.parent.parent.parent.GetComponent<PlayerStats>().goldAmount;
        myInventory = transform.parent.parent.parent.parent.Find("Canvas Inventory").Find("Inventory").GetComponent<InventoryScript>();
    }


    public void AddItem(VendorItem vendorItem)
    {
        this.vendorItem1 = vendorItem;

        if (vendorItem.MyQuantity > 0 || vendorItem.Unlimited)
        {
            icon.sprite = vendorItem.MyItem.MyIcon;
            string titleNew = string.Format("<color={0}>{1}</color>", nameColors[vendorItem.MyItem.itemQuality], vendorItem.MyItem.name);
            title.text = titleNew;
            

            if (!vendorItem.Unlimited)
            {
                quantity.text = vendorItem.MyQuantity.ToString();
            }
            else
            {
                quantity.text = string.Empty;
            }

            if (vendorItem.MyItem.MyPrice > 0)
            {
                price.text = "Price: " + vendorItem.MyItem.MyPrice.ToString() + "Ehre. Bruder. ... Gib. ._.";
            }
            else
            {
                price.text = "Price: Free. Falls du keine Ehre hast. ... Pleb.";
            }

            gameObject.SetActive(true);
        }

        
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if ((goldAmount1 >= vendorItem1.MyItem.MyPrice) && myInventory.AddItem(Instantiate(vendorItem1.MyItem)))
        {
            SellItem();
            onRemoved = true;
        }
    }

    public void MasterETStuffAssignment()
    {
        masterETItems.itemName = vendorItem1.MyItem.tooltipItemName;

        masterETItems.itemDescription = vendorItem1.MyItem.tooltipItemDescription;

        //masterETItems.buffSprite = Resources.Load<Sprite>("Sprites/BuffSprites/" + buffName);
    }

    public void SellItem()
    {
        goldAmount1 -= vendorItem1.MyItem.MyPrice;
        transform.parent.parent.parent.parent.parent.GetComponent<PlayerStats>().goldAmount = goldAmount1;

        if (!vendorItem1.Unlimited)
        {
            vendorItem1.MyQuantity--;
            quantity.text = vendorItem1.MyQuantity.ToString();

            if (vendorItem1.MyQuantity <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (vendorItem1.MyItem == null)
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
