using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class HandScript : MonoBehaviour
{ 
    Image handImage;
    public string handSkillName;
    public GameObject handButtonSwap;
    public bool actionButtonDragOn = true;
    bool hasItem;

    InventoryScript myInventory;
    ActionButton[] allActionButtons;

    public IMoveable MyMoveable { get; set; }

    //private Image icon;

    void Start()
    {
        handImage = GetComponent<Image>();
        myInventory = transform.parent.parent.Find("Canvas Inventory").Find("Inventory").GetComponent<InventoryScript>();
        allActionButtons = transform.parent.parent.Find("Canvas Action Skills").Find("SkillSlots").GetComponentsInChildren<ActionButton>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject() && this.MyMoveable != null)
        {
            DeleteItem();
        }
       

        if (handSkillName == "" && hasItem == false)
        {
            handImage.color = new Color(0, 0, 0, 0);
        }
        else if (hasItem == true)
        {
            handImage.transform.position = Mouse.current.position.ReadValue();
        }
        else
        {
            handImage.transform.position = Mouse.current.position.ReadValue();

            handImage.sprite = Resources.Load<Sprite>("SkillSprites/" + handSkillName);
            handImage.color = Color.white;
        }
    }

    public void TakeMoveable(IMoveable moveable)
    {
         this.MyMoveable = moveable;
         handImage.sprite = moveable.MyIcon;
         handImage.color = Color.white;
         hasItem = true;
    }

    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;
        MyMoveable = null;
        handImage.color = new Color(0, 0, 0, 0);

        for (int i = 0; i < allActionButtons.Length; i++)
        { allActionButtons[i].RemoteUpdateThisButton(); }
        return tmp;
    }

    public void Drop()
    {
        MyMoveable = null;
        handImage.color = new Color(0, 0, 0, 0);
        hasItem = false;
        myInventory.FromSlot = null;

        for (int i = 0; i < allActionButtons.Length; i++)
        { allActionButtons[i].RemoteUpdateThisButton(); }
    }

    public void DeleteItem()
    {
        if (MyMoveable is Item) //  && InventoryScript.MyInstance.FromSlot != null
        {
            Item item = (Item)MyMoveable;
            if (item.MySlot != null)
            {
                item.MySlot.Clear();
            }
            else if (item.MyCharButton != null)
            {
                item.MyCharButton.DequipStuff();
            }
            
        }

        Drop();

        myInventory.FromSlot = null;
    }
}


