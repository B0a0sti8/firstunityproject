using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockActionButtonDrag : MonoBehaviour
{
    Image lockImage;

    private void Awake()
    {
        lockImage = GetComponent<Image>();
    }

    public void ActionButtonDragChange()
    {
        if (HandScript.MyInstance.actionButtonDragOn == true)
        {
            HandScript.MyInstance.actionButtonDragOn = false;
            lockImage.sprite = Resources.Load<Sprite>("SkillSprites/LockClosed");
        }
        else
        {
            HandScript.MyInstance.actionButtonDragOn = true;
            lockImage.sprite = Resources.Load<Sprite>("SkillSprites/LockOpen");
        }
    }
}
