using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class HandScript : MonoBehaviour
{
    #region Singleton
    static HandScript instance;

    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }
            return instance;
        }
    }
    #endregion

    Image handImage;
    public string handSkillName;
    public GameObject handButtonSwap;
    public bool actionButtonDragOn = true;
    bool hasItem;

    public IMoveable MyMoveable { get; set; }

    //private Image icon;

    void Start()
    {
        handImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMoveable != null)
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
        return tmp;
    }

    public void Drop()
    {
        MyMoveable = null;
        handImage.color = new Color(0, 0, 0, 0);
        hasItem = false;
        InventoryScript.MyInstance.FromSlot = null;
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

        InventoryScript.MyInstance.FromSlot = null;
    }
}


