using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockActionButtonDrag : MonoBehaviour
{
    Image lockImage;
    HandScript myHandScript;

    private void Awake()
    {
        lockImage = GetComponent<Image>();
        myHandScript = transform.parent.parent.Find("Canvas Hand").Find("Hand Image").GetComponent<HandScript>();
    }


    public void ActionButtonDragChange()
    {
        if (myHandScript.actionButtonDragOn == true)
        {
            myHandScript.actionButtonDragOn = false;
            lockImage.sprite = Resources.Load<Sprite>("SkillSprites/LockClosed");
        }
        else
        {
            myHandScript.actionButtonDragOn = true;
            lockImage.sprite = Resources.Load<Sprite>("SkillSprites/LockOpen");
        }
    }
}
