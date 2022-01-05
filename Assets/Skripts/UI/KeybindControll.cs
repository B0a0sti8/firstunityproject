using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindControll : MonoBehaviour
{
    [SerializeField] private CanvasGroup keybindMenue;

    public void OpenCloseMenue()
    {
        keybindMenue.alpha = keybindMenue.alpha > 0 ? 0 : 1; // if > 0 -> 0, else 1
        keybindMenue.blocksRaycasts = keybindMenue.blocksRaycasts == true ? false : true; 
    }

    private void OnKeybindMenue()
    {
        OpenCloseMenue();
    }

}
