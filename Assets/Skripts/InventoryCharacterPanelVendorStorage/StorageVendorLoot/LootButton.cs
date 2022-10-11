using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootButton : MonoBehaviour, IPointerClickHandler
{
    MasterEventTriggerItems masterETItems;

    [SerializeField] private Image icon;
    [SerializeField] private Text title;
    [SerializeField] private Item item;

    private LootWindow lootWindow;

    public Image MyIcon { get => icon; set => icon = value; }
    public Text MyTitle { get => title; set => title = value; }
    public Item MyItem { get => item; set => item = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryScript.MyInstance.AddItem(MyItem))
        {
            gameObject.SetActive(false);
            lootWindow.TakeLoot(MyItem);
            TooltipScreenSpaceUIItems.HideTooltip_Static();
        }
        // Loot Item
        
    }

    private void Awake()
    {
        masterETItems = GetComponent<MasterEventTriggerItems>();
        lootWindow = GetComponentInParent<LootWindow>();
    }

    void MasterETStuffAssignment()
    {
        masterETItems.itemName = MyItem.tooltipItemName;
        masterETItems.itemDescription = MyItem.tooltipItemDescription;
        //masterETItems.buffSprite = Resources.Load<Sprite>("Sprites/BuffSprites/" + buffName);
    }

    private void Update()
    {
        if (MyItem != null)
        {
            MasterETStuffAssignment();
        }
    }
}
