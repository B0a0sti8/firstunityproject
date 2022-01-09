using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

    void Start()
    {
        handImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (handSkillName == "")
        {
            handImage.color = new Color(0, 0, 0, 0);
        }
        else
        {
            handImage.transform.position = Mouse.current.position.ReadValue();

            handImage.sprite = Resources.Load<Sprite>("SkillSprites/" + handSkillName);
            handImage.color = Color.white;
        }
    }
}


//public IMoveable MyMoveable { get; set; }

//public void TakeMoveable(IMoveable moveable)
//{
//    this.MyMoveable = moveable;
//    icon.sprite = moveable.MyIcon;
//    icon.color = Color.white;
//}

//public IMoveable Put()
//{
//    IMoveable tmp = MyMoveable;
//    MyMoveable = null;
//    icon.color = new Color(0, 0, 0, 0);
//    return tmp;
//}